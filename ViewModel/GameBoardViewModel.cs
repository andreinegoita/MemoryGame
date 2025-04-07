using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using MemoryGame.Model;
using MemoryGame.ViewModel.Commands;
using MemoryGame.View;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace MemoryGame.ViewModel
{
    public class GameBoardViewModel : INotifyPropertyChanged
    {
        private readonly int _rows;
        private readonly int _columns;
        private GameTileModel _firstSelectedTile;
        private GameTileModel _secondSelectedTile;
        private bool _isBusy;
        private readonly Random _random = new Random();
        private readonly MainViewModel _mainViewModel;
        private bool _gameOver;


        public ObservableCollection<GameTileModel> Tiles { get; }

        public ICommand TileClickCommand { get; }

        public int Rows => _rows;
        public int Columns => _columns;
        public string SelectedCategory { get; }


        public string TimerDisplay => RemainingTime.ToString(@"mm\:ss");


        private DispatcherTimer _gameTimer;
        private TimeSpan _remainingTime = TimeSpan.FromMinutes(2); 

        public TimeSpan RemainingTime
        {
            get => _remainingTime;
            set
            {
                _remainingTime = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TimerDisplay));
            }
        }
        public bool IsGameOver
        {
            get => _gameOver;
            set
            {
                if (_gameOver != value)
                {
                    _gameOver = value;
                    OnPropertyChanged(nameof(IsGameOver));
                    OnPropertyChanged(nameof(TimerDisplay));
                }
            }
        }

        public GameBoardViewModel(int rows, int columns, string selectedCategory,TimeSpan time, MainViewModel mainViewModel)
        {
            _rows = rows;
            _columns = columns;
            SelectedCategory = selectedCategory;
            Tiles = new ObservableCollection<GameTileModel>();
            RemainingTime = time;
            _mainViewModel = mainViewModel;
            _gameOver = false;
            _gameTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _gameTimer.Tick += GameTimer_Tick;
            _gameTimer.Start();
            
            
            GenerateTiles();

            TileClickCommand = new RelayCommand(FlipTile);
            _mainViewModel = mainViewModel;
        }
        public GameBoardViewModel(GameState state,MainViewModel mainViewModel)
        {
            _rows = state.Rows;
            _columns = state.Columns;
            SelectedCategory = state.SelectedCategory;
            Tiles = new ObservableCollection<GameTileModel>(state.Tiles);
            TileClickCommand = new RelayCommand(FlipTile);
            RemainingTime = state.RemainingTime;
            _mainViewModel = mainViewModel;
            _gameTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _gameTimer.Tick += GameTimer_Tick;
            _gameTimer.Start();

        }
        private void GenerateTiles()
        {
            int totalTiles = _rows * _columns;
            int numberOfPairs = totalTiles / 2;

            string categoryFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Icons", SelectedCategory);

            if (!Directory.Exists(categoryFolder))
            {
                MessageBox.Show($"Folderul nu există: {categoryFolder}");
                return;
            }

            string[] imageFiles = Directory.GetFiles(categoryFolder)
                .Where(file => file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                               file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                               file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                               file.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            if (imageFiles.Length < numberOfPairs)
            {
                MessageBox.Show($"Nu sunt suficiente imagini în {SelectedCategory}. Ai nevoie de {numberOfPairs}, dar sunt doar {imageFiles.Length}.");
                return;
            }

            var selectedImages = imageFiles.OrderBy(x => _random.Next()).Take(numberOfPairs).ToList();
            var pairedImages = selectedImages.Concat(selectedImages).OrderBy(x => _random.Next()).ToList();

            foreach (var imagePath in pairedImages)
            {
                Tiles.Add(new GameTileModel(imagePath));
            }

            OnPropertyChanged(nameof(Tiles));
        }

        private async void FlipTile(object parameter)
        {
            if (parameter is not GameTileModel tile || tile.IsFlipped || tile.IsMatched || _isBusy)
                return;
            var mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri("Assets/sounds/Click.mp3", UriKind.Relative));
            mediaPlayer.Play();

            if (_firstSelectedTile != null && _secondSelectedTile != null)
            {
                _isBusy = true;
                await Task.Delay(500); 
                if (!_firstSelectedTile.IsMatched && !_secondSelectedTile.IsMatched)
                {
                    AgitateTiles(_firstSelectedTile, _secondSelectedTile);
                    _firstSelectedTile.IsFlipped = false;
                    _secondSelectedTile.IsFlipped = false;
                    OnPropertyChanged(nameof(Tiles));
                }
                _firstSelectedTile = null;
                _secondSelectedTile = null;
                _isBusy = false;
            }

            tile.IsFlipped = true;
            OnPropertyChanged(nameof(Tiles));

            if (_firstSelectedTile == null)
            {
                _firstSelectedTile = tile;
            }
            else
            {
                _secondSelectedTile = tile;
                if (_firstSelectedTile.ImagePath == _secondSelectedTile.ImagePath)
                {
                    _firstSelectedTile.IsMatched = true;
                    _secondSelectedTile.IsMatched = true;
                    _firstSelectedTile = null;
                    _secondSelectedTile = null;
                    OnPropertyChanged(nameof(Tiles));
                    CheckGameWon();
                }
            }
        }
        private async void AgitateTiles(GameTileModel firstTile, GameTileModel secondTile)
        {
            
            firstTile.IsAgitating = true;
            secondTile.IsAgitating = true;
            await Task.Delay(350);
            firstTile.IsAgitating = false;
            secondTile.IsAgitating = false;
        }


        private async void CheckGameWon()
        {
            if (Tiles.All(tile => tile.IsMatched))
            {

                _gameTimer.Stop();
                _gameTimer.Tick -= GameTimer_Tick;
                if (_gameTimer.IsEnabled)
                {
                    _gameTimer.IsEnabled = false;
                }
                UserManager.UpdateUserStatistics(_mainViewModel.CurrentUserName, true);

                MessageBox.Show("Ai câștigat!", "Felicitări", MessageBoxButton.OK, MessageBoxImage.Information);


                await Task.Delay(100);


                _mainViewModel.CurrentView = new PlayMenu
                {
                    DataContext = new PlayMenuViewModel(_mainViewModel)
                };
            }
        }


        public GameState GetCurrentState()
        {
            return new GameState
            {
                Name = "Joc curent",
                SelectedCategory = SelectedCategory,
                Rows = _rows,
                Columns = _columns,
                Tiles = Tiles.ToList(),
                RemainingTime = RemainingTime
            };
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            
            if (_gameOver)
            {
                return;
            }

            if (RemainingTime.TotalSeconds > 0)
            {
                RemainingTime = RemainingTime.Subtract(TimeSpan.FromSeconds(1));
            }
            else
            {
                _gameTimer.Stop();
                UserManager.UpdateUserStatistics(_mainViewModel.CurrentUserName, false);
                MessageBox.Show("Timpul a expirat!", "Game Over", MessageBoxButton.OK, MessageBoxImage.Warning);
                _mainViewModel.CurrentView = new PlayMenu
                {
                    DataContext = new PlayMenuViewModel(_mainViewModel)
                };
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

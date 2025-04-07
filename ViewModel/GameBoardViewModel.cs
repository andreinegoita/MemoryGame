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
using System.Windows.Media;
using MemoryGame.Model;
using MemoryGame.ViewModel.Commands;
using MemoryGame.View;

namespace MemoryGame.ViewModel
{
    public class GameBoardViewModel : INotifyPropertyChanged
    {
        private readonly int _rows;
        private readonly int _columns;
        private readonly Random _random = new Random();
        private readonly MainViewModel _mainViewModel;
        private DispatcherTimer _gameTimer;
        private TimeSpan _remainingTime;
        private GameTileModel _firstSelectedTile;
        private GameTileModel _secondSelectedTile;
        private bool _isBusy;
        private bool _gameOver;

        public ObservableCollection<GameTileModel> Tiles { get; }

        public ICommand TileClickCommand { get; }

        public int Rows => _rows;
        public int Columns => _columns;
        public string SelectedCategory { get; }

        public string TimerDisplay => RemainingTime.ToString(@"mm\:ss");

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
                    OnPropertyChanged();
                }
            }
        }

        public GameBoardViewModel(int rows, int columns, string selectedCategory, TimeSpan time, MainViewModel mainViewModel)
        {
            _rows = rows;
            _columns = columns;
            SelectedCategory = selectedCategory;
            RemainingTime = time;
            _mainViewModel = mainViewModel;

            Tiles = new ObservableCollection<GameTileModel>();
            TileClickCommand = new RelayCommand(FlipTile);

            InitializeTimer();
            GenerateTiles();
        }

        public GameBoardViewModel(GameState state, MainViewModel mainViewModel)
        {
            _rows = state.Rows;
            _columns = state.Columns;
            SelectedCategory = state.SelectedCategory;
            RemainingTime = state.RemainingTime;
            _mainViewModel = mainViewModel;

            Tiles = new ObservableCollection<GameTileModel>(state.Tiles);
            TileClickCommand = new RelayCommand(FlipTile);

            InitializeTimer();
        }

        private void InitializeTimer()
        {
            _gameTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
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

            var imageFiles = Directory.GetFiles(categoryFolder)
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

            var selectedImages = imageFiles.OrderBy(_ => _random.Next()).Take(numberOfPairs).ToList();
            var pairedImages = selectedImages.Concat(selectedImages).OrderBy(_ => _random.Next()).ToList();

            foreach (var imagePath in pairedImages)
                Tiles.Add(new GameTileModel(imagePath));

            OnPropertyChanged(nameof(Tiles));
        }

        private async void FlipTile(object parameter)
        {
            if (parameter is not GameTileModel tile || tile.IsFlipped || tile.IsMatched || _isBusy)
                return;

            PlaySound("Assets/sounds/Click.mp3");

            if (_firstSelectedTile != null && _secondSelectedTile != null)
            {
                _isBusy = true;
                await Task.Delay(500);

                if (!_firstSelectedTile.IsMatched && !_secondSelectedTile.IsMatched)
                {
                    PlaySound("Assets/sounds/wrong answer Sound effect .mp3");
                    await AgitateTiles(_firstSelectedTile, _secondSelectedTile);
                    _firstSelectedTile.IsFlipped = false;
                    _secondSelectedTile.IsFlipped = false;
                }

                _firstSelectedTile = null;
                _secondSelectedTile = null;
                _isBusy = false;
            }

            tile.IsFlipped = true;

            if (_firstSelectedTile == null)
            {
                _firstSelectedTile = tile;
            }
            else
            {
                _secondSelectedTile = tile;
                if (_firstSelectedTile.ImagePath == _secondSelectedTile.ImagePath)
                {
                    PlaySound("Assets/sounds/Correct answer Sound effect.mp3");
                    _firstSelectedTile.IsMatched = true;
                    _secondSelectedTile.IsMatched = true;
                    _firstSelectedTile = null;
                    _secondSelectedTile = null;
                    CheckGameWon();
                }
            }
        }

        private async Task AgitateTiles(GameTileModel firstTile, GameTileModel secondTile)
        {
            firstTile.IsAgitating = true;
            secondTile.IsAgitating = true;
            await Task.Delay(350);
            firstTile.IsAgitating = false;
            secondTile.IsAgitating = false;
        }

        private void PlaySound(string relativePath)
        {
            var player = new MediaPlayer();
            player.Open(new Uri(relativePath, UriKind.Relative));
            player.Play();
        }

        private async void CheckGameWon()
        {
            if (Tiles.All(tile => tile.IsMatched))
            {
                StopTimer();
                UserManager.UpdateUserStatistics(_mainViewModel.CurrentUserName, true);
                PlaySound("Assets/sounds/Victory Sound Effect.mp3");

                await Task.Delay(100);

                _mainViewModel.CurrentView = new WinGame
                {
                    DataContext = new WinGameModel(_mainViewModel)
                };
            }
        }

        public void StopTimer()
        {
            if (_gameTimer != null)
            {
                _gameTimer.Stop();
                _gameTimer.Tick -= GameTimer_Tick;
                _gameTimer = null;
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (_gameOver) return;

            if (RemainingTime.TotalSeconds > 0)
            {
                RemainingTime = RemainingTime.Subtract(TimeSpan.FromSeconds(1));
            }
            else
            {
                StopTimer();
                UserManager.UpdateUserStatistics(_mainViewModel.CurrentUserName, false);
                PlaySound("Assets/sounds/Lose.mp3");

                _mainViewModel.CurrentView = new LoseGame
                {
                    DataContext = new LoseGameViewModel(_mainViewModel)
                };
            }
        }

        public GameState GetCurrentState() => new GameState
        {
            Name = "Joc curent",
            SelectedCategory = SelectedCategory,
            Rows = _rows,
            Columns = _columns,
            Tiles = Tiles.ToList(),
            RemainingTime = RemainingTime
        };

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

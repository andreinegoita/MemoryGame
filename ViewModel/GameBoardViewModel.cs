using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using MemoryGame.Model;
using MemoryGame.ViewModel.Commands;

namespace MemoryGame.ViewModel
{
    public class GameBoardViewModel : INotifyPropertyChanged
    {
        private readonly int _rows;
        private readonly int _columns;
        private GameTileModel _firstSelectedTile;
        private GameTileModel _secondSelectedTile;
        private readonly Random _random = new Random();

        public ObservableCollection<GameTileModel> Tiles { get; }

        public ICommand TileClickCommand { get; }

        public int Rows => _rows;
        public int Columns => _columns;
        public string SelectedCategory { get; }

        public GameBoardViewModel(int rows, int columns, string selectedCategory)
        {
            _rows = rows;
            _columns = columns;
            SelectedCategory = selectedCategory;
            Tiles = new ObservableCollection<GameTileModel>();

            GenerateTiles();

            TileClickCommand = new RelayCommand(FlipTile);
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

        private void FlipTile(object parameter)
        {
            if (parameter is GameTileModel tile && !tile.IsFlipped && !tile.IsMatched)
            {
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
                    }
                    else
                    {
                        _firstSelectedTile.IsFlipped = false;
                        _secondSelectedTile.IsFlipped = false;
                    }

                    _firstSelectedTile = null;
                    _secondSelectedTile = null;

                    OnPropertyChanged(nameof(Tiles));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

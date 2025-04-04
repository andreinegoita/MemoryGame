﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
        private bool _isBusy;
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
        public GameBoardViewModel(GameState state)
        {
            _rows = state.Rows;
            _columns = state.Columns;
            SelectedCategory = state.SelectedCategory;
            Tiles = new ObservableCollection<GameTileModel>(state.Tiles);
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

        private async void FlipTile(object parameter)
        {
            if (parameter is not GameTileModel tile || tile.IsFlipped || tile.IsMatched || _isBusy)
                return;

            if (_firstSelectedTile != null && _secondSelectedTile != null)
            {
                _isBusy = true;
                await Task.Delay(500); 
                if (!_firstSelectedTile.IsMatched && !_secondSelectedTile.IsMatched)
                {
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

        private void CheckGameWon()
        {
            if (Tiles.All(tile => tile.IsMatched))
            {
                MessageBox.Show("Ai câștigat!", "Felicitări", MessageBoxButton.OK, MessageBoxImage.Information);
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
                Tiles = Tiles.ToList()
            };
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

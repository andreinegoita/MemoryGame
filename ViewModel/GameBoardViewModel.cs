using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
            SelectedCategory = selectedCategory;
        }

        private void GenerateTiles()
        {
            int totalTiles = _rows * _columns;
            string[] values = Enumerable.Range(1, totalTiles / 2).Select(i => i.ToString()).ToArray();
            string[] pairedValues = values.Concat(values).OrderBy(x => Guid.NewGuid()).ToArray();

            foreach (var value in pairedValues)
            {
                Tiles.Add(new GameTileModel(value));
            }
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

                    if (_firstSelectedTile.Value == _secondSelectedTile.Value)
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

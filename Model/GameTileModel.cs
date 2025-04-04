using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MemoryGame.Model
{
    public class GameTileModel : INotifyPropertyChanged
    {
        public bool IsMatched { get; set; }
        public bool IsFlipped { get; set; }

        private string _imagePath;
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public GameTileModel(string imagePath)
        {
            ImagePath = imagePath;
            IsMatched = false;
            IsFlipped = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

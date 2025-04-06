using MemoryGame.Model;
using MemoryGame.View;
using MemoryGame.ViewModel.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MemoryGame.ViewModel
{
    public class StatisticsViewModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _mainViewModel;

        public ObservableCollection<PlayerStatModel> PlayerStats { get; } = new ObservableCollection<PlayerStatModel>();

        public ICommand BackCommand { get; }

        public StatisticsViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            BackCommand = new RelayCommand(BackToMenu);

            LoadStatistics();
        }

        private void LoadStatistics()
        {
            PlayerStats.Clear();

            foreach (var user in UserManager.GetAllUsers())
            {
                PlayerStats.Add(new PlayerStatModel
                {
                    PlayerName = user.Name,
                    GamesPlayed = user.Statistics.GamesPlayed,
                    GamesWon = user.Statistics.GamesWon,
                    WinRate = user.Statistics.WinRate
                });
            }
        }

        private void BackToMenu(object parameter)
        {
            _mainViewModel.CurrentView = new PlayMenu
            {
                DataContext = new PlayMenuViewModel(_mainViewModel)
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class PlayerStatModel
    {
        public string PlayerName { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public double WinRate { get; set; }
    }
}
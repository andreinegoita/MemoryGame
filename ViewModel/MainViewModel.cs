using MemoryGame.View;
using MemoryGame.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MemoryGame.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {

        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public ICommand StartCommand { get; }

        public MainViewModel()
        {
            CurrentView = new View.MainMenuView();

            StartCommand = new RelayCommand(SwitchToStartUpMenu);
        }

        private void SwitchToStartUpMenu(object obj)
        {
            CurrentView = new StartUpMenuView
            {
                DataContext = new StartUpMenuViewModel(this) 
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

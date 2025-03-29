using MemoryGame.View;
using MemoryGame.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MemoryGame.ViewModel
{
    public class PlayMenuViewModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _mainViewModel;
        public ICommand ExitCommand { get; }



        public PlayMenuViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            ExitCommand = new RelayCommand(Exit);
        }

        private void Exit(object parameter)
        {
            _mainViewModel.CurrentView = new StartUpMenuView
            {
                DataContext = new StartUpMenuViewModel(_mainViewModel)
            };
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

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
        public ICommand FileCommand { get; }
        public ICommand OptionsCommand { get; }

        public ICommand HelpCommand { get; }


        public PlayMenuViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            ExitCommand = new RelayCommand(Exit);
            FileCommand = new RelayCommand(File);
            OptionsCommand = new RelayCommand(Options);
            HelpCommand = new RelayCommand(Help);
        }

        private void Exit(object parameter)
        {
            _mainViewModel.CurrentView = new StartUpMenuView
            {
                DataContext = new StartUpMenuViewModel(_mainViewModel)
            };
        }

        private void File(object parameter)
        {
            _mainViewModel.CurrentView= new FIleWindow
            {
                DataContext = new FileViewModel(_mainViewModel)
            };
        }

        private void Options(object parameter)
        {
            _mainViewModel.CurrentView = new OptionsWindow
            {
                DataContext = new OptionsViewModel(_mainViewModel)
            };
        }

        private void Help(object parameter)
        {
            _mainViewModel.CurrentView = new HelpWindow
            {
                DataContext = new HelpViewModel(_mainViewModel)
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

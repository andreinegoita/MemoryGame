using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MemoryGame.ViewModel.Commands;
using MemoryGame.View;

namespace MemoryGame.ViewModel
{
    public class OptionsViewModel
    {

        private readonly MainViewModel _mainViewModel;

        public ICommand StandardCommand { get; }

        public ICommand CustomCommand { get; }


        public ICommand ExitCommand { get; }

        public OptionsViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            ExitCommand = new RelayCommand(Exit);
        }

        private void Exit(object parameter)
        {
            _mainViewModel.CurrentView = new PlayMenu
            {
                DataContext = new PlayMenuViewModel(_mainViewModel)
            };
        }
    }
}

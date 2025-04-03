using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MemoryGame.View;
using MemoryGame.ViewModel.Commands;

namespace MemoryGame.ViewModel
{
    public class HelpViewModel
    {

        private readonly MainViewModel _mainViewModel;

        public ICommand ExitCommand { get; }
        public HelpViewModel(MainViewModel mainViewModel)
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

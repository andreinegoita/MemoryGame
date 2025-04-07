using MemoryGame.View;
using MemoryGame.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MemoryGame.ViewModel
{
    public class LoseGameViewModel
    {
        private readonly MainViewModel _mainViewModel;
        public ICommand ReturnToMainMenuCommand { get; set; }

        public LoseGameViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            ReturnToMainMenuCommand = new RelayCommand(ReturnToMainMenu);
        }

        private void ReturnToMainMenu(object parameter)
        {
            _mainViewModel.CurrentView = new MainMenuView();
            _mainViewModel.GameRows = 4;
            _mainViewModel.GameColumns = 4;
            _mainViewModel.SelectedCategory = "Default";
            _mainViewModel.CustomTime = TimeSpan.FromMinutes(2);
        }
    }
}

using System.Windows;
using System.Windows.Input;
using MemoryGame.View;
using MemoryGame.ViewModel.Commands;

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
            StandardCommand = new RelayCommand(Standard);
            CustomCommand = new RelayCommand(Custom);
            ExitCommand = new RelayCommand(Exit);
        }

        private void Standard(object parameter)
        {

            _mainViewModel.GameRows = 4;
            _mainViewModel.GameColumns = 4;
            MessageBox.Show("Setările standard (4x4) au fost aplicate!");
        }

        private void Custom(object parameter)
        {
            var customWindow = new CustomOptionsWindow();
            customWindow.DataContext = new CustomOptionsViewModel();
            var result = customWindow.ShowDialog();
            if (result == true)
            {
                var customVM = customWindow.DataContext as CustomOptionsViewModel;
                if (customVM != null)
                {

                    _mainViewModel.GameRows = customVM.SelectedRows;
                    _mainViewModel.GameColumns = customVM.SelectedColumns;
                }
            }
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

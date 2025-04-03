using System.Windows;
using System.Windows.Input;
using MemoryGame.View;
using MemoryGame.ViewModel.Commands;

namespace MemoryGame.ViewModel
{
    public class FileViewModel
    {
        private readonly MainViewModel _mainViewModel;

        public ICommand ExitCommand { get; }
        public ICommand CategoryCommand { get; }
        public ICommand NewGameCommand { get; }

        public FileViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            ExitCommand = new RelayCommand(Exit);
            CategoryCommand = new RelayCommand(Category);
            NewGameCommand = new RelayCommand(NewGame);
        }

        private void Exit(object parameter)
        {
            _mainViewModel.CurrentView = new PlayMenu
            {
                DataContext = new PlayMenuViewModel(_mainViewModel)
            };
        }

        private void Category(object parameter)
        {
            _mainViewModel.CurrentView = new CategoryWindow
            {
                DataContext = new CategoryViewModel(_mainViewModel)
            };
        }

        private void NewGame(object parameter)
        {
            if ((_mainViewModel.GameRows * _mainViewModel.GameColumns) % 2 != 0)
            {
                MessageBox.Show("Numărul de jetoane trebuie să fie par!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _mainViewModel.CurrentView = new GameBoardView
            {
                DataContext = new GameBoardViewModel(_mainViewModel.GameRows, _mainViewModel.GameColumns)
            };
        }
    }
}

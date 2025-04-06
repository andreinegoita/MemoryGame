using System;
using System.Windows;
using System.Windows.Input;
using MemoryGame.View;
using MemoryGame.ViewModel.Commands;

namespace MemoryGame.ViewModel
{
    public class TimeInputViewModel 
    {
        private readonly MainViewModel _mainViewModel;
        private readonly Window _window;

        public TimeInputViewModel(MainViewModel mainViewModel, Window window)
        {
            _mainViewModel = mainViewModel;
            _window = window;
            Minutes = "2";
            Seconds = "0";
            ConfirmCommand = new RelayCommand(Confirm);
        }

        public string Minutes { get; set; }
        public string Seconds { get; set; }

        public ICommand ConfirmCommand { get; }

        private void Confirm(object parameter)
        {
            if (int.TryParse(Minutes, out int min) && int.TryParse(Seconds, out int sec))
            {
                TimeSpan time = new TimeSpan(0, min, sec);
                _mainViewModel.CustomTime = time;

                // Start GameBoardView
                _mainViewModel.CurrentView = new GameBoardView
                {
                    DataContext = new GameBoardViewModel(_mainViewModel.GameRows, _mainViewModel.GameColumns, _mainViewModel.SelectedCategory, time,_mainViewModel)
                };

                _window.Close();
            }
            else
            {
                MessageBox.Show("Introduceți valori numerice valide pentru minute și secunde.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

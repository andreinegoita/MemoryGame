using MemoryGame.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace MemoryGame.ViewModel
{
    public class AddUserViewModel : INotifyPropertyChanged
    {
        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public Action CloseAction { get; set; } // Permite închiderea ferestrei

        public AddUserViewModel()
        {
            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Confirm(object parameter)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                CloseAction?.Invoke(); // Închide fereastra
            }
            else
            {
                MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Cancel(object parameter)
        {
            UserName = null;
            CloseAction?.Invoke(); // Închide fereastra
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}

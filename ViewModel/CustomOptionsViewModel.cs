using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MemoryGame.ViewModel.Commands;
using MemoryGame.View;

namespace MemoryGame.ViewModel
{
    public class CustomOptionsViewModel : INotifyPropertyChanged
    {
        private int _selectedRows = 4;
        private int _selectedColumns = 4;

        public int SelectedRows
        {
            get => _selectedRows;
            set
            {
                if (value >= 2 && value <= 6)
                {
                    _selectedRows = value;
                    OnPropertyChanged();
                }
            }
        }

        public int SelectedColumns
        {
            get => _selectedColumns;
            set
            {
                if (value >= 2 && value <= 6)
                {
                    _selectedColumns = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ConfirmCommand { get; }

        public CustomOptionsViewModel()
        {
            ConfirmCommand = new RelayCommand(Confirm);
        }

        private void Confirm(object parameter)
        {
            // Se așteaptă ca parameter-ul să fie fereastra curentă.
            if (parameter is CustomOptionsWindow window)
            {
                // Setăm DialogResult pentru a semnala succesul și închidem fereastra.
                window.DialogResult = true;
                window.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

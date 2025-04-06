using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MemoryGame.View;
using MemoryGame.ViewModel.Commands;

namespace MemoryGame.ViewModel
{
    public class AboutViewModel
    {

        private readonly MainViewModel _mainViewModel;

        public ICommand ExitCommand { get; private set; }

        public ICommand OpenEmailCommand { get; private set; }

        public AboutViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            ExitCommand = new RelayCommand(Exit);
            OpenEmailCommand = new RelayCommand(OpenEmail);
        }

        private void Exit(object parameter)
        {
            _mainViewModel.CurrentView = new HelpWindow
            {
                DataContext = new HelpViewModel(_mainViewModel)
            };
        }

        private void OpenEmail(object parameter)
        {
            Process.Start(new ProcessStartInfo("mailto:andrei.negoita@student.unitbv.ro") { UseShellExecute = true });
        }

    }
}

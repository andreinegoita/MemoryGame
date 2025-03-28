using MemoryGame.Services;
using MemoryGame.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MemoryGame.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private MusicPlayerService _musicPlayerService;

        public ICommand PlayCommand { get; }
        public ICommand StopCommand { get; }

        public SettingsViewModel()
        {
            _musicPlayerService = MusicPlayerService.Instance;

            PlayCommand = new RelayCommand(Play);
            StopCommand = new RelayCommand(Stop);
        }


        private void Play(object parameter) => _musicPlayerService.PlayMusic();
        private void Stop(object parameter) => _musicPlayerService.StopMusic();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

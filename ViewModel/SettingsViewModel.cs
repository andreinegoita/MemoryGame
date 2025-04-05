using MemoryGame.Model;
using MemoryGame.Services;
using MemoryGame.View;
using MemoryGame.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.IO;

namespace MemoryGame.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private MusicPlayerService _musicPlayerService;
        private readonly MainViewModel _mainViewModel;

        public ICommand PlayCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand ExitCommand { get; }

        public SettingsViewModel(MainViewModel mainViewModel)
        {
            _musicPlayerService = MusicPlayerService.Instance;
            _mainViewModel = mainViewModel;
            PlayCommand = new RelayCommand(Play);
            StopCommand = new RelayCommand(Stop);
            ExitCommand = new RelayCommand(Exit);
        }


        private void Play(object parameter) => _musicPlayerService.PlayMusic();
        private void Stop(object parameter) => _musicPlayerService.StopMusic();

        private void Exit(object parameter)
        {
            // 1. Confirmare de ieșire
            var result = MessageBox.Show(
                "Vrei să salvezi progresul înainte de a ieși?",
                "Confirmare",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Cancel)
                return;

            if (_mainViewModel.CurrentView is GameBoardView gameBoardView &&
                gameBoardView.DataContext is GameBoardViewModel gameVm)
            {
                if (result == MessageBoxResult.Yes)
                {
                    SaveUserProgress(gameVm);
                }
            }


            _mainViewModel.CurrentView = new PlayMenu
            {
                DataContext = new PlayMenuViewModel(_mainViewModel)
            };


            if (parameter is System.Windows.Window window)
            {
                window.Close();
            }
        }

        private void SaveUserProgress(GameBoardViewModel gameVm)
        {
            string saveFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Saved Games");


            if (!Directory.Exists(saveFolderPath))
            {
                Directory.CreateDirectory(saveFolderPath);
            }


            string userName = _mainViewModel.CurrentUserName;


            var user = new User(userName)
            {
                GameBoardState = gameVm.Tiles.Select(t => new TileState
                {
                    ImagePath = t.ImagePath,
                    IsFlipped = t.IsFlipped
                }).ToList()
            };


            string userFilePath = Path.Combine(saveFolderPath, $"{userName}.json");

            List<User> users = new List<User>();

            if (File.Exists(userFilePath))
            {
                string existingData = File.ReadAllText(userFilePath);
                try
                {
                    users = JsonSerializer.Deserialize<List<User>>(existingData) ?? new List<User>();
                }
                catch
                {
                    MessageBox.Show("Fișierul este corupt. Se va rescrie.");
                }
            }


            user.SavedGames.Add(new GameSave
            {
                GameState = gameVm.GetCurrentState(), 
                SaveDate = DateTime.Now
            });


            users.Add(user);

            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(userFilePath, JsonSerializer.Serialize(users, options));
        }





        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

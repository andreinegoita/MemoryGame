using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using MemoryGame.Model;
using MemoryGame.Services;
using MemoryGame.View;
using MemoryGame.ViewModel.Commands;

namespace MemoryGame.ViewModel
{
    public class StartUpMenuViewModel : INotifyPropertyChanged
    {
        private int _currentImageIndex;
        private string _selectedImage;
        private readonly MainViewModel _mainViewModel;
        public ObservableCollection<string> ImageOptions { get; set; }
        public ObservableCollection<User> Users { get; set; }

        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));

                if (_selectedUser != null)
                {
                    SelectedImage = _selectedUser.SelectedImage;
                }
            }
        }

        public string SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                OnPropertyChanged(nameof(SelectedImage));
            }
        }

        public ICommand NextImageCommand { get; }
        public ICommand PreviousImageCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand PlayCommand { get; }
        public ICommand CancelCommand { get; }

        public StartUpMenuViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            Users = UserService.LoadUsers();

            ImageOptions = new ObservableCollection<string>
            {
                "/Assets/Users/8168-sonic-pfpsgg.gif",
                "/Assets/Users/1336-fabian-pfpsgg.png",
                "/Assets/Users/4113-luffy-pfpsgg.png",
                "/Assets/Users/5517-lildeady-pfpsgg.png",
                "/Assets/Users/7442-tyler-the-creator-pfpsgg.png"
            };

            _currentImageIndex = 0;
            SelectedImage = ImageOptions[0];

            NextImageCommand = new RelayCommand(_ => NextImage());
            PreviousImageCommand = new RelayCommand(_ => PreviousImage());
            AddUserCommand = new RelayCommand(_ => OpenAddUserWindow());
            DeleteUserCommand = new RelayCommand(_ => DeleteUser(), _ => SelectedUser != null);
            PlayCommand = new RelayCommand(_ => StartGame(), _ => SelectedUser != null);
            CancelCommand = new RelayCommand(SwitchMainMenu);
        }

        private void OpenAddUserWindow()
        {
            var addUserWindow = new AddUserWindow();
            var viewModel = new AddUserViewModel();
            addUserWindow.DataContext = viewModel;
            viewModel.CloseAction = () => addUserWindow.Close();
            addUserWindow.ShowDialog();

            if (!string.IsNullOrWhiteSpace(viewModel.UserName))
            {
                var newUser = new User { Name = viewModel.UserName, SelectedImage = SelectedImage };
                Users.Add(newUser);
                UserService.SaveUsers(Users);
            }
        }

        private void DeleteUser()
        {
            if (SelectedUser != null)
            {
                Users.Remove(SelectedUser);
                SelectedUser = null;
                UserService.SaveUsers(Users);
            }
        }

        private void NextImage()
        {
            _currentImageIndex = (_currentImageIndex + 1) % ImageOptions.Count;
            SelectedImage = ImageOptions[_currentImageIndex];
        }

        private void PreviousImage()
        {
            _currentImageIndex = (_currentImageIndex - 1 + ImageOptions.Count) % ImageOptions.Count;
            SelectedImage = ImageOptions[_currentImageIndex];
        }

        private void StartGame()
        {
            if (SelectedUser != null)
            {
                SelectedUser.SelectedImage = SelectedImage;
                UserService.SaveUsers(Users);
                MessageBox.Show($"Game started for {SelectedUser.Name}!");
            }
        }

        private void SwitchMainMenu(object obj)
        {
            _mainViewModel.CurrentView = new MainMenuView();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

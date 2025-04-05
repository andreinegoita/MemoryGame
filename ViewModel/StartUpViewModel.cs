using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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

            string userImagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Icons","F1","Helmets");

            if (!Directory.Exists(userImagesFolder))
            {
                MessageBox.Show($"Folderul nu există: {userImagesFolder}");
                ImageOptions = new ObservableCollection<string>();
            }
            else
            {

                var imageFiles = Directory.GetFiles(userImagesFolder)
                    .Where(file => file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                   file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                   file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                   file.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                    .Select(path => path.Replace(AppDomain.CurrentDomain.BaseDirectory, string.Empty).Replace("\\", "/"))
                    .ToList();

                ImageOptions = new ObservableCollection<string>(imageFiles);
               
            }

            _currentImageIndex = 0;

            SelectedImage = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ImageOptions[_currentImageIndex]);



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
            SelectedImage = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ImageOptions[_currentImageIndex]);
        }


        private void PreviousImage()
        {
            _currentImageIndex = (_currentImageIndex - 1 + ImageOptions.Count) % ImageOptions.Count;
            SelectedImage = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ImageOptions[_currentImageIndex]);
        }


        private void StartGame()
        {
            if (SelectedUser != null)
            {
                SelectedUser.SelectedImage = SelectedImage;
                UserService.SaveUsers(Users);
                MessageBox.Show($"Game started for {SelectedUser.Name}!");
                _mainViewModel.CurrentView = new PlayMenu
                {
                    DataContext = new PlayMenuViewModel(_mainViewModel) 
                };
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

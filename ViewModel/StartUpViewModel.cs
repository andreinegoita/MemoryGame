using MemoryGame.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MemoryGame.Model;
using MemoryGame.View;

namespace MemoryGame.ViewModel
{
    public class StartUpMenuViewModel : INotifyPropertyChanged
    {
        private int _currentImageIndex;
        private string _selectedImage;

        public ObservableCollection<string> ImageOptions { get; set; }
        public ObservableCollection<User> Users { get; set; }=new ObservableCollection<User>();

        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
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

        private void OpenAddUserWindow()
        {
            var addUserWindow = new AddUserWindow();
            var viewModel = new AddUserViewModel();

            addUserWindow.DataContext = viewModel;
            viewModel.CloseAction = () => addUserWindow.Close();

            addUserWindow.ShowDialog();

            if (!string.IsNullOrWhiteSpace(viewModel.UserName))
            {
                Users.Add(new User { Name = viewModel.UserName });
            }
        }


        public ICommand DeleteUserCommand { get; }

        private void DeleteUser()
        {
            if (SelectedUser != null)
            {
                Users.Remove(SelectedUser);
                SelectedUser = null; // Resetare selecție
            }
        }

        private bool CanDeleteUser()
        {
            return SelectedUser != null;
        }

        public StartUpMenuViewModel()
        {
            ImageOptions = new ObservableCollection<string>
            {
                "/Assets/Users/8168-sonic-pfpsgg.gif",
                "/Assets/Users/1336-fabian-pfpsgg.png",
                "/Assets/Users/4113-luffy-pfpsgg.png",
                "/Assets/Users/5517-lildeady-pfpsgg.png",
                "/Assets/Users/7442-tyler-the-creator-pfpsgg.png",
                "/Assets/Users/ccbbe672da735eb688531af38104c66d.jpg",
                "/Assets/Users/5f17444866bfbe3030bcf82faff3c68b.jpg",
                "/Assets/Users/21cec04a1daef96dad10405131b41e45.jpg",
                "/Assets/Users/d124cf0275f0ee14da828f1a84d0aa3b.jpg",
                "/Assets/Users/751197fc16e5ad4facc97d0ccf6ce42b.jpg",
                "/Assets/Users/2a9bd72cb276c78aa3a82ddaf8ce57bf.jpg",
            };

            _currentImageIndex = 0;
            SelectedImage = ImageOptions[1];
            Users.Add(new User { Name = "Alice" });

            NextImageCommand = new RelayCommand(_ => NextImage());
            PreviousImageCommand = new RelayCommand(_ => PreviousImage());
            AddUserCommand = new RelayCommand(_ => OpenAddUserWindow());
            DeleteUserCommand = new RelayCommand(_ => DeleteUser(), _ => CanDeleteUser());
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

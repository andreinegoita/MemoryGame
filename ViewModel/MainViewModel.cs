﻿using MemoryGame.Model;
using MemoryGame.View;
using MemoryGame.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MemoryGame.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {

        private object _currentView;
        private string _currentUserName;
        public string CurrentUserName
        {
            get => _currentUserName;
            set
            {
                if (_currentUserName != value)
                {
                    _currentUserName = value;
                    OnPropertyChanged();
                }
            }
        }

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        private int _gameRows = 4;
        public int GameRows
        {
            get => _gameRows;
            set
            {
                _gameRows = value;
                OnPropertyChanged();
            }
        }

        private int _gameColumns = 4;
        public int GameColumns
        {
            get => _gameColumns;
            set
            {
                _gameColumns = value;
                OnPropertyChanged();
            }
        }

        public GameState CurrentGameState { get; set; }

        private string _selectedCategory = "Default";
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }


        public ICommand StartCommand { get; }

        public ICommand OpenSettingsCommand { get; }

        public MainViewModel()
        {
            CurrentView = new View.MainMenuView();

            StartCommand = new RelayCommand(SwitchToStartUpMenu);
            OpenSettingsCommand = new RelayCommand(OpenSettingsWindow);
        }

        private void SwitchToStartUpMenu(object obj)
        {
            CurrentView = new StartUpMenuView
            {
                DataContext = new StartUpMenuViewModel(this) 
            };
        }

        private void OpenSettingsWindow(object obj)
        {
            
            var settingsWindow = new SettingsWindow(this);
            settingsWindow.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

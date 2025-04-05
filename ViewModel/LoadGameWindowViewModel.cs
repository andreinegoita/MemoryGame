using MemoryGame.Model;
using MemoryGame.View;
using MemoryGame.ViewModel.Commands;
using MemoryGame.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Input;
using System.Windows;
using System.IO;
public class LoadGameWindowViewModel : INotifyPropertyChanged
{
    private readonly MainViewModel _mainViewModel;
    public ObservableCollection<GameSave> SavedGames { get; set; }
    public ICommand LoadGameCommand { get; }
    public ICommand CancelCommand { get; }

    private GameSave _selectedGame;
    public GameSave SelectedGame
    {
        get => _selectedGame;
        set
        {
            _selectedGame = value;
            OnPropertyChanged(nameof(SelectedGame));
        }
    }

    public LoadGameWindowViewModel(MainViewModel mainViewModel, string userName)
    {
        _mainViewModel = mainViewModel;

        SavedGames = new ObservableCollection<GameSave>(LoadSavedGamesForUser(userName));


        LoadGameCommand = new RelayCommand(LoadGame, (param) => true);
        CancelCommand = new RelayCommand(CloseWindow);
    }

    private List<GameSave> LoadSavedGamesForUser(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            MessageBox.Show("Numele utilizatorului nu este setat!");
            return new List<GameSave>();
        }

        string saveFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Saved Games");
        string userFilePath = Path.Combine(saveFolderPath, $"{userName}.json");

        List<GameSave> savedGames = new List<GameSave>();

        if (File.Exists(userFilePath))
        {
            string existingData = File.ReadAllText(userFilePath);
            try
            {

                var users = JsonSerializer.Deserialize<List<User>>(existingData);
                if (users != null)
                {
                    foreach (var user in users.Where(u => u.Name == userName))
                    {
                        if (user.SavedGames != null)
                            savedGames.AddRange(user.SavedGames);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fișierul este corupt sau invalid.\nEroare: {ex.Message}");
            }
        }

        return savedGames;
    }

    private void LoadGame(object parameter)
    {
        if (SelectedGame == null)
        {
            MessageBox.Show("Te rog selectează un joc pentru încărcare.");
            return;
        }


        _mainViewModel.CurrentView = new GameBoardView
        {
            DataContext = new GameBoardViewModel(SelectedGame.GameState)
        };
    }

    private void CloseWindow(object parameter)
    {

        Application.Current.Windows.OfType<Window>().LastOrDefault()?.Close();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

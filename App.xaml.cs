using MemoryGame.Services;
using System.Configuration;
using System.Data;
using System.Windows;

namespace MemoryGame;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var musicPlayerService = MusicPlayerService.Instance;
    }
}


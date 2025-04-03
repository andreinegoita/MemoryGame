using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MemoryGame.Services
{
    public class MusicPlayerService
    {
        private static MusicPlayerService _instance;
        private MediaPlayer _mediaPlayer;
        private List<string> _playlist;
        private int _currentSongIndex;

        public static MusicPlayerService Instance => _instance ??= new MusicPlayerService();



        public string CurrentSong => _playlist[_currentSongIndex];

        private MusicPlayerService()
        {
            _mediaPlayer = new MediaPlayer();


            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            _playlist = new List<string>
            {
               // System.IO.Path.Combine(baseDirectory, "Assets", "Sounds", "Untitled.mp3"),
            };



            _currentSongIndex = 0;
            _mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            PlayMusic();
        }


        public void PlayMusic()
        {
            if (_playlist.Count == 0) return;
            _mediaPlayer.Open(new Uri(_playlist[_currentSongIndex], UriKind.Absolute));
            _mediaPlayer.Play();
        }

        public void StopMusic()
        {
            _mediaPlayer.Stop();
        }

        public void NextSong()
        {
            _currentSongIndex = (_currentSongIndex + 1) % _playlist.Count;
            PlayMusic();
        }


        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            NextSong();
        }


    }
}

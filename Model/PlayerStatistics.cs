using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Model
{
    public class PlayerStatistics
    {
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }

        public double WinRate => GamesPlayed > 0 ? (double)GamesWon / GamesPlayed * 100 : 0;

        public void AddGame(bool won)
        {
            GamesPlayed++;
            if (won)
            {
                GamesWon++;
            }
        }
    }

}

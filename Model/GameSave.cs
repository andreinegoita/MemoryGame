using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Model
{
    public class GameSave
    {
        public GameState GameState { get; set; }
        public DateTime SaveDate { get; set; }
    }
}

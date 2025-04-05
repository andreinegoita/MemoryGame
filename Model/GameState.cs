using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Model
{
    public class GameState
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string SelectedCategory { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public List<GameTileModel> Tiles { get; set; }
    }

}

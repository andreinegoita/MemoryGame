using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Model
{
    public class GameTileModel
    {
        public string Value { get; set; } // Conținutul jetonului (ex: literă, simbol, număr)
        public bool IsMatched { get; set; } // Dacă jetonul a fost descoperit
        public bool IsFlipped { get; set; } // Dacă jetonul este momentan întors

        public GameTileModel(string value)
        {
            Value = value;
            IsMatched = false;
            IsFlipped = false;
        }
    }
}

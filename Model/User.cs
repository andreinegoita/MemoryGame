﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Model
{
    public class User
    {
        public Guid Id { get; set; }=Guid.NewGuid();
        public string Name { get; set; }
        public string SelectedImage { get; set; }

        public List<TileState> GameBoardState { get; set; }

        public List<GameSave> SavedGames { get; set; }=new List<GameSave>();

        public User(string name)
        {
            Name = name;
            GameBoardState = new List<TileState>();
        }
    }
}

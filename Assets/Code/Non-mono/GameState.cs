using System;
using System.Collections.ObjectModel;

namespace MagnetGame
{
    public struct Player
    {
        public Position Position;
        public bool Polarity;
    }

    public struct GameState
    {
        public string Visualize()
        {
            // TODO: Implement function
            return null;
        }
        public BoardField[] Fields;
        public Player Player;
        public Level Level;
    }
}
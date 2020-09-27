using System;
using System.Collections.ObjectModel;

namespace MagnetGame
{
    public class Level
    {
        // TODO: Add Json Serialization
        public ReadOnlyCollection<BoardField> Fields;
        public readonly int LevelWidth;
        public readonly int LevelHeight;
        public readonly Player StartingPlayer;
        public readonly Position Endpoint;

        public int FieldIndex(int x, int y)
        {
            if(x >= LevelWidth || x < 0)
            {
                throw new ArgumentException("Illegal x index " + x + " , must be between 0 and " + LevelWidth + ".");
            }
            if(y >= LevelWidth || y < 0)
            {
                throw new ArgumentException("Illegal y index " + y + " , must be between 0 and " + LevelHeight + ".");
            } 
            return y*LevelWidth + x;
        }

        public GameState ToState()
        {
            var stateFields = new BoardField[Fields.Count]; 
            Fields.CopyTo(stateFields, 0);
            return new GameState{Fields = stateFields, Player = StartingPlayer, Level = this};
        }
    }
}
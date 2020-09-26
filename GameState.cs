using System;
using System.Collections.ObjectModel;

namespace Magnet_game
{
    public struct Field
    {
        public enum FieldType
        {
            Empty,
            Wall,
            Hole,
            Magnet

        }
        public FieldType FieldState;
        public int MagnetStrength;
        public bool MagnetPolarity;
    }

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
        public Field[] Fields;
        public Player Player;
        public Level Level;
    }

    public class Level
    {
        // TODO: Add Json Serialization
        public ReadOnlyCollection<Field> Fields;
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
            var stateFields = new Field[Fields.Count]; 
            Fields.CopyTo(stateFields, 0);
            return new GameState{Fields = stateFields, Player = StartingPlayer, Level = this};
        }
    }
}
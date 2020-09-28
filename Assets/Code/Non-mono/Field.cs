using System;

namespace MagnetGame
{
    [Serializable]
    public struct BoardField
    {
        public enum FieldType
        {
            Empty,
            Wall,
            Hole,
            Endpoint,
            Magnet,
            Player

        }
        public FieldType FieldState;
        public int MagnetStrength;
        public bool MagnetPolarity;
    }
}
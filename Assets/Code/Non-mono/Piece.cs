using System;

namespace MagnetGame
{
    [Serializable]
    public struct Piece
    {
        public enum PieceType
        {
            Empty,
            Wall,
            Hole,
            Endpoint,
            Magnet,
            Player
        }
        public PieceType Type;
        public int MagnetStrength;
        public bool MagnetPolarity;
    }
}
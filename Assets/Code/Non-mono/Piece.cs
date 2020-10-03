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
            Door,
            Lock,
            Player
        }
        public PieceType Type;
        public int MagnetStrength;
        public bool MagnetPolarity;
    }
}
namespace MagnetGame
{
    public struct BoardField
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
}
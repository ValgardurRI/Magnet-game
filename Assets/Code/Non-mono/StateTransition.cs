using System;
using System.Collections.Generic;

namespace MagnetGame
{
    public struct StateTransition
    {
        // List the positions that piece got moved to from magnet attraction
        public IEnumerable<Position> AttractionPostitions;
        public IEnumerable<Tuple<Position, Position>> MagnetRepulsions;
        public GameState NewState;
    }
}
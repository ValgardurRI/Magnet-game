using UnityEngine;
using System.Collections;

namespace MagnetGame
{
    [CreateAssetMenu(fileName = "MagnetColors", menuName = "Config/MagnetColors", order = 1)]
    public class MagnetColors : ScriptableObject {
        public Color PositiveColor;
        public Color NegativeColor;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MagnetGame.Consts;

namespace MagnetGame
{
    public class MonoPlayer : Draggable
    {
        [SerializeField]
        protected MagnetColors colors;
        public void Setup(Vector2 anchorMin, Vector2 anchorMax, BaseBoard board, Piece pieceType, int magnetStrength, bool polarity)
        {
            Setup(anchorMin, anchorMax, board, pieceType);
            GetComponent<Image>().color = polarity == POSITIVE ? colors.PositiveColor : colors.NegativeColor;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static MagnetGame.Consts;

namespace MagnetGame
{
    public class MonoLock : Draggable
    {
        [SerializeField]
        protected MagnetColors colors;
        protected int lockId;
        public void Setup(Vector2 anchorMin, Vector2 anchorMax, BaseBoard board, Piece pieceType, int lockId, bool polarity)
        {
            this.lockId = lockId;
            Setup(anchorMin, anchorMax, board, pieceType);
            GetComponent<Image>().color = polarity == POSITIVE ? colors.PositiveColor : colors.NegativeColor;
            transform.GetComponentInChildren<TextMeshProUGUI>().text = ((char)('A' + Mathf.Abs(basePiece.MagnetStrength) - 1)).ToString();
        }
    }
}

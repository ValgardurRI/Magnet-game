using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using static MagnetGame.Consts;

namespace MagnetGame
{
    public class MonoMagnet : Draggable
    {
        [SerializeField]
        protected MagnetColors colors;
        public void Setup(Vector2 size, Vector2 position, BaseBoard board, int magnetStrength, bool polarity)
        {
            Setup(size, position, board);

            // set color of magnet color field
            GetComponent<Image>().color = polarity == POSITIVE ? colors.PositiveColor : colors.NegativeColor;
            
            char sign = polarity == POSITIVE ? '+' : '-';
            transform.GetComponentInChildren<TextMeshProUGUI>().text = sign + magnetStrength.ToString();
        }
    }
}

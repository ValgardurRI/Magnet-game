﻿using System.Collections;
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
        public void Setup(Vector2 size, Vector2 position, BaseBoard board, int magnetStrength, bool polarity)
        {
            Setup(size, position, board);
            GetComponent<Image>().color = polarity == POSITIVE ? colors.PositiveColor : colors.NegativeColor;
        }
    }
}

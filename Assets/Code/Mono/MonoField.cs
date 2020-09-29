using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagnetGame
{
    public class MonoField : MonoBehaviour
    {
        public Draggable piece;

        public void Setup(Vector2 size, Vector2 position)
        {
            transform.position = position;
            ((RectTransform)transform).sizeDelta = size;
        }
    }
}

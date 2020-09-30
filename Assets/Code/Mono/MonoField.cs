using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagnetGame
{
    public class MonoField : MonoBehaviour
    {
        public Draggable piece;
        public int FieldId { get; private set;}

        public void Setup(Vector2 size, Vector2 position, int fieldId)
        {
            transform.position = position;
            ((RectTransform)transform).sizeDelta = size;
            FieldId = fieldId;
        }
    }
}

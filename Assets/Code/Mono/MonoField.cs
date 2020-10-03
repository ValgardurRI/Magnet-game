using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagnetGame
{
    public class MonoField : MonoBehaviour
    {
        public Draggable piece;
        public int FieldId { get; private set;}

        public void Setup(int fieldId)
        {
            FieldId = fieldId;
        }
    }
}

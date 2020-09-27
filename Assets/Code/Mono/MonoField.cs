using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagnetGame
{
    public class MonoField : MonoBehaviour
    {
        public void Setup(Vector2 size, Vector2 position)
        {
            transform.position = position;
            ((RectTransform)transform).sizeDelta = size;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MagnetGame.Consts;

namespace MagnetGame
{
    public class MonoPlayer : MonoBehaviour
    {
        public static Color PositiveColor;
        public static Color NegativeColor; 
        public void Setup(Vector2 size, Vector2 position, int magnetStrength, bool polarity)
        {
            transform.position = position;
            ((RectTransform)transform).sizeDelta = size;
            GetComponent<Image>().color = polarity == POSITIVE ? PositiveColor : NegativeColor;
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

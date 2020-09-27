using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MagnetGame.Consts;

namespace MagnetGame
{
    public class MonoMagnet : MonoBehaviour
    {
        [SerializeField]
        protected MagnetColors colors;
        public void Setup(Vector2 size, Vector2 position, int magnetStrength, bool polarity)
        {
            transform.position = position;
            ((RectTransform)transform).sizeDelta = size;
            GetComponent<Image>().color = polarity == POSITIVE ? colors.PositiveColor : colors.NegativeColor;
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

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using static MagnetGame.Consts;
using UnityEngine.EventSystems;

namespace MagnetGame
{
    public class PolarityButton : MonoBehaviour, IPointerClickHandler
    {
        public bool polarity;
        public IMagnetic subject;

        public MagnetColors colors;
        private void Start()
        {
            subject = transform.GetComponentInParent<IMagnetic>();
            GetComponent<Image>().color = polarity == POSITIVE ? colors.PositiveColor : colors.NegativeColor;

        }

        public void ChangePolarity()
        {
            subject.ChangePolarity(polarity);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ChangePolarity();
        }
    }
}
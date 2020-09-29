using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MagnetGame
{
    public abstract class DesignerToolbarItem : MonoBehaviour, IPointerClickHandler
    {
        private DesignerToolbar toolbar;
        [HideInInspector]
        public Image background;

        private void Awake()
        {
            toolbar = transform.GetComponentInParent<DesignerToolbar>();
            background = transform.Find("background").GetComponent<Image>();
            background.color = new Color(0, 0, 0, 0);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            toolbar.Select(this);
        }

        public abstract void FieldClickEffect(MonoField field, DesignerBoard board);
        public abstract void SelectEffect(DesignerBoard board);
        public abstract void DeselectEffect(DesignerBoard board);
    }
}
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MagnetGame
{
    [RequireComponent(typeof(Image))]
    public class Draggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        // Configuration variables
        bool draggable = true;

        // Bookkeeping variables
        private Vector2 currentPos;
        private Vector3 inputDelta;
        private bool dragging;
        protected BaseBoard board;

        public virtual void Setup(Vector2 size, Vector2 position, BaseBoard board)
        {
            transform.position = position;
            ((RectTransform)transform).sizeDelta = size;
            this.board = board;
            SetDraggable(draggable);
        }

        public void SetDraggable(bool value)
        {
            draggable = value;
            foreach(var image in transform.GetComponentsInChildren<Image>())
            {
                image.raycastTarget = value;
            }
        }

        private void Update()
        {
            if (dragging)
            {

                transform.position = Input.mousePosition + inputDelta;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (draggable)
            {
                inputDelta = transform.position - Input.mousePosition;
                currentPos = transform.position;
                dragging = true;
                transform.SetAsLastSibling();
            }
            else
            {
                eventData.pointerDrag = null;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(draggable)
                transform.position = eventData.position + (Vector2)inputDelta;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(draggable)
            {
                dragging = false;
                MonoField cell = Utils.GetComponentFromRaycast<MonoField>(Input.mousePosition);
                if (cell == null)
                {
                    transform.position = currentPos;
                }
                else
                {
                    var placement = board.Place(this, cell);
                    transform.position = placement != null ? (Vector3)placement : (Vector3)currentPos; 
                }
            }
        }
    }
}
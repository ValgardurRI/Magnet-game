using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MagnetGame
{
    [RequireComponent(typeof(Image))]
    public class Draggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool CanDrag { get; private set; } = true;

        public Piece basePiece;

        // Bookkeeping variables
        private Vector2 currentPos;
        private Vector3 inputDelta;
        private bool dragging;
        protected BaseBoard board;

        public virtual void Setup(Vector2 anchorMin, Vector2 anchorMax, BaseBoard board, Piece pieceType)
        {
            var rectTransform = ((RectTransform)transform);
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            this.board = board;
            SetDraggable(CanDrag);
            basePiece = pieceType;
        }

        public void SetDraggable(bool value)
        {
            CanDrag = value;
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
            if (CanDrag)
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
            if(CanDrag)
                transform.position = eventData.position + (Vector2)inputDelta;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(CanDrag)
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
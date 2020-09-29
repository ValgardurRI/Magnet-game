using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace MagnetGame
{
    public class DesignerBoard : BaseBoard, IPointerClickHandler
    {
        public DesignerToolbar toolbar;

        public override void Setup()
        {
            base.Setup();
            SetAllPiecesDraggable(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var field = Utils.GetComponentFromRaycast<MonoField>(Input.mousePosition);
            if(field != null)
            {
                toolbar.TriggerClick(field);
            }
        }

        public void RemoveFromField(MonoField field)
        {
            if(field.piece != null)
            {
                Destroy(field.piece.gameObject);
                field.piece = null;
            }
        }

        public void SetAllPiecesDraggable(bool value)
        {
            foreach(var field in fields)
            {
                if(field.piece != null)
                {
                    field.piece.SetDraggable(value);
                }
            }
        }

        public override Vector3? Place(Draggable piece, MonoField square)
        {
            if (square.piece == null)
            {
                // find and remove piece on other field
                foreach (var field in fields)
                {
                    if (field != null && field.piece == piece)
                    {
                        field.piece = null;
                    }
                }

                // set piece on new field
                square.piece = piece;

                return square.transform.position;
            }
            return null;
        }

    }
}
using UnityEngine;
using System.Collections;

namespace MagnetGame
{
    public class MoveItem : DesignerToolbarItem
    {
        public override void DeselectEffect(DesignerBoard board)
        {
            board.SetAllPiecesDraggable(false);
        }

        public override void FieldClickEffect(MonoField field, DesignerBoard board){}

        public override void SelectEffect(DesignerBoard board)
        {
            board.SetAllPiecesDraggable(true);
        }
    }
}
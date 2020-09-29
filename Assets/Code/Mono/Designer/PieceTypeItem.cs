using UnityEngine;
using System.Collections;

namespace MagnetGame
{
    public class PieceTypeItem : DesignerToolbarItem
    {
        public BoardField pieceType;

        public override void DeselectEffect(DesignerBoard board){}

        public override void FieldClickEffect(MonoField field, DesignerBoard board)
        {
            var piece = board.AddPiece(pieceType, field);
            piece.SetDraggable(false);
        }

        public override void SelectEffect(DesignerBoard board){}
    }
}
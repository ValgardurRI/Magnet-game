using UnityEngine;
using System.Collections;
using MagnetGame;
using System;

namespace MagnetGame
{
    public class TrashCanItem : DesignerToolbarItem
    {
        public override void DeselectEffect(DesignerBoard board){}

        public override void FieldClickEffect(MonoField field, DesignerBoard board)
        {
            board.RemoveFromField(field);
        }

        public override void SelectEffect(DesignerBoard board){}
    }
}
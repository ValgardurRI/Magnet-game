using UnityEngine;
using System.Collections;
using MagnetGame;
using UnityEngine.UI;
using static MagnetGame.Consts;

namespace MagnetGame
{
    public class PlayerItem : DesignerToolbarItem, IMagnetic
    {
        public MagnetColors colors;
        private bool polarity = POSITIVE;

        private void Start()
        {
            ChangePolarity(polarity);    
        }

        public void ChangePolarity(bool value)
        {
            polarity = value;
            transform.Find("PlayerImage").GetComponent<Image>().color = polarity == POSITIVE ? colors.PositiveColor : colors.NegativeColor;
        }

        public override void DeselectEffect(DesignerBoard board){}

        public override void FieldClickEffect(MonoField field, DesignerBoard board)
        {
            var piece = board.AddPiece(new Piece { Type = Piece.PieceType.Player, MagnetPolarity = polarity }, field);
            piece.SetDraggable(false);
        }

        public override void SelectEffect(DesignerBoard board){}
    }
}
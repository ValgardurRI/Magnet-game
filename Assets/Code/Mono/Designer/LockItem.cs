using UnityEngine;
using System.Collections;
using MagnetGame;
using UnityEngine.UI;
using static MagnetGame.Consts;
using TMPro;

namespace MagnetGame
{
    public class LockItem : DesignerToolbarItem, IMagnetic
    {
        public MagnetColors colors;
        private int polarityStrength = 1;

        private void Start()
        {
            SetIndicators();
        }

        private void SetIndicators()
        {
            transform.Find("ColorFill").GetComponent<Image>().color = polarityStrength > 0 ? colors.PositiveColor : colors.NegativeColor;
            GetComponentInChildren<TextMeshProUGUI>().text = ((char)('A' + Mathf.Abs(polarityStrength) - 1)).ToString();
        }

        public void ChangePolarity(bool value)
        {
            polarityStrength += value == POSITIVE ? 1 : -1;
            if(polarityStrength == 0)
            {
                polarityStrength += value == POSITIVE ? 1 : -1;
            }
            SetIndicators();
        }

        public override void DeselectEffect(DesignerBoard board) { }

        public override void FieldClickEffect(MonoField field, DesignerBoard board)
        {
            var piece = board.AddPiece(new Piece { Type = Piece.PieceType.Lock, MagnetPolarity = (polarityStrength > 0), MagnetStrength = Mathf.Abs(polarityStrength) }, field);
            piece.SetDraggable(false);
        }

        public override void SelectEffect(DesignerBoard board) { }
    }
}
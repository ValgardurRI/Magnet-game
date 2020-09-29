using UnityEngine;
using System.Collections;
using MagnetGame;
using UnityEngine.UI;
using static MagnetGame.Consts;
using TMPro;

namespace MagnetGame
{
    public class MagnetItem : DesignerToolbarItem, IMagnetic
    {
        public MagnetColors colors;
        private int polarityStrength = 1;

        private void Start()
        {
            SetIndicators();
        }

        private void SetIndicators()
        {
            transform.Find("MagnetColor").GetComponent<Image>().color = polarityStrength > 0 ? colors.PositiveColor : colors.NegativeColor;
            GetComponentInChildren<TextMeshProUGUI>().text = (polarityStrength > 0 ? '+' : '-') + Mathf.Abs(polarityStrength).ToString();
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
            var piece = board.AddPiece(new BoardField { FieldState = BoardField.FieldType.Magnet, MagnetPolarity = (polarityStrength > 0), MagnetStrength = Mathf.Abs(polarityStrength) }, field);
            piece.SetDraggable(false);
        }

        public override void SelectEffect(DesignerBoard board) { }
    }
}
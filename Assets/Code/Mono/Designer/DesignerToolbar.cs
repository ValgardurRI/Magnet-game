using UnityEngine;
using System.Collections;
using MagnetGame;
using System.Collections.Generic;
using System;
using UnityEditor;
using JetBrains.Annotations;
using UnityEngine.UI;

namespace MagnetGame
{
    public class DesignerToolbar : MonoBehaviour
    {
        public DesignerBoard board;
        public List<DesignerToolbarItem> items;
        protected DesignerToolbarItem selected;

        public void Select(DesignerToolbarItem item)
        {
            if(selected != null)
            {
                selected.DeselectEffect(board);
                selected.background.color = new Color(0, 0, 0, 0);
            }
            selected = item;
            selected.SelectEffect(board);
            selected.background.color = Color.blue;
        }

        public void TriggerClick(MonoField field)
        {
            if(selected != null)
            {
                selected.FieldClickEffect(field, board);
            }
        }
    }
}
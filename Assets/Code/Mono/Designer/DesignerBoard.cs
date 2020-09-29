using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEngine.Timeline;

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
            void RemoveFromOldField()
            {
                foreach (var field in fields)
                {
                    if (field != null && field.piece == piece)
                    {
                        field.piece = null;
                    }
                }
            }

            if (square.piece == null)
            {
                RemoveFromOldField();
                // set piece on new field
                square.piece = piece;

                return square.transform.position;
            }
            else if(square.piece != piece)
            {
                // Both pieces must be magnets
                bool stackMagnetCondition = piece.basePiece.Type == Piece.PieceType.Magnet;
                stackMagnetCondition &= square.piece.basePiece.Type == piece.basePiece.Type;

                // Neither magnet can be 0
                stackMagnetCondition &= piece.basePiece.MagnetStrength != 0;
                stackMagnetCondition &= square.piece.basePiece.MagnetStrength != 0;

                // Magnet polarities must be equal
                stackMagnetCondition &= piece.basePiece.MagnetPolarity == square.piece.basePiece.MagnetPolarity;

                if (stackMagnetCondition)
                {
                    var newMagnet = new Piece { MagnetStrength = (square.piece.basePiece.MagnetStrength + piece.basePiece.MagnetStrength), MagnetPolarity = piece.basePiece.MagnetPolarity, Type = piece.basePiece.Type };
                    bool oldDragability = square.piece.CanDrag; 
                    RemoveFromField(square);
                    RemoveFromOldField();
                    Destroy(piece.gameObject);
                    var newPiece = AddPiece(newMagnet, square);
                    newPiece.SetDraggable(oldDragability);
                }
            }
            return null;
        }

        [CustomEditor(typeof(DesignerBoard))]
        public class DesignerEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DesignerBoard myBoard = (DesignerBoard)target;
                myBoard.level = (Level)EditorGUILayout.ObjectField("Level", myBoard.level, typeof(Level), true);
                EditorGUILayout.LabelField("Level string");
                myBoard.levelString = EditorGUILayout.TextArea(myBoard.levelString, GUILayout.MaxHeight(75));
                /*
                if (myBoard.editMode)
                {
                    if (GUILayout.Button("Set board"))
                    {
                        myBoard.SetLevel();
                    }
                }
                else
                {
                    if (GUILayout.Button("Copy level string to clipboard"))
                    {
                        EditorGUIUtility.systemCopyBuffer = myBoard.levelString;
                    }
                }
                */

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.IntField("Columns", myBoard.Columns);
                EditorGUILayout.IntField("Rows", myBoard.Rows);
                EditorGUI.EndDisabledGroup();

                myBoard.configuration = (BoardConfiguration)EditorGUILayout.ObjectField("Configuration", myBoard.configuration, typeof(BoardConfiguration), true);
            }
        }
    }
}
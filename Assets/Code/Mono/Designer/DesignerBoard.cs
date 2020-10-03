using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Timeline;

namespace MagnetGame
{
    [System.Serializable]
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

        public void DeletePiece(MonoField field)
        {
            if(field.piece != null)
            {
                Destroy(field.piece.gameObject);
                field.piece = null;
                level.Fields[field.FieldId].Type = Piece.PieceType.Empty;
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
                        level.Fields[field.FieldId].Type = Piece.PieceType.Empty;
                    }
                }
            }

            if (square.piece == null)
            {
                RemoveFromOldField();
                // set piece on new field
                square.piece = piece;
                level.Fields[square.FieldId] = piece.basePiece;
                return square.transform.position;
            }
            else if(!square.piece.CanDrag)
            {
                // For replacing pieces when placed
                RemoveFromOldField();
                DeletePiece(square);
                square.piece = piece;
                level.Fields[square.FieldId] = piece.basePiece;
                return square.transform.position;
            }
            // Both pieces must be magnets
            bool stackMagnetCondition = piece.basePiece.Type == Piece.PieceType.Magnet;
            stackMagnetCondition &= square.piece != piece;
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
                DeletePiece(square);
                RemoveFromOldField();
                Destroy(piece.gameObject);
                var newPiece = AddPiece(newMagnet, square);
                newPiece.SetDraggable(oldDragability);
            }
            return null;
        }

        public void SaveLevel()
        {
            #if UNITY_EDITOR
            string path = "Assets/Objects/ScriptableObjects/Levels/";
            ScriptableObject temp = Instantiate(level);
            AssetDatabase.CreateAsset(temp, path + level.name + ".asset");
            level = (Level)temp;
            Debug.Log("Level " + level.name + " saved to folder " + path);
            #else
            Debug.LogError("Saving levels is not supported in built project");
            #endif
        }

        #if UNITY_EDITOR
        [CustomEditor(typeof(DesignerBoard))]
        public class DesignerEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DesignerBoard myBoard = (DesignerBoard)target;
                myBoard.level = EditorGUILayout.ObjectField("Level", myBoard.level, typeof(Level), true) as Level;
                if(myBoard.level == null)
                {
                    Undo.RecordObject(target, "Some Random text");
                    myBoard.level = ScriptableObject.CreateInstance<Level>();
                    myBoard.level.UpdateArraySize();
                }
                else
                {
                    myBoard.level.name = EditorGUILayout.TextField("Level name", myBoard.level.name);
                    EditorGUI.BeginChangeCheck();
                    myBoard.level.LevelWidth = EditorGUILayout.IntField("Columns", myBoard.level.LevelWidth);
                    myBoard.level.LevelHeight = EditorGUILayout.IntField("Rows", myBoard.level.LevelHeight);
                    if(EditorGUI.EndChangeCheck())
                    {
                        myBoard.level.UpdateArraySize();
                        if(EditorApplication.isPlaying)
                        {
                            myBoard.ClearBoard();
                            myBoard.InstantiateBoard();
                        }
                    }
                    if (GUILayout.Button("Save level"))
                    {
                        myBoard.SaveLevel();
                    }
                }

                myBoard.configuration = (BoardConfiguration)EditorGUILayout.ObjectField("Configuration", myBoard.configuration, typeof(BoardConfiguration), true);
                if(GUI.changed)
                    EditorUtility.SetDirty(target);
            }
        }
        #endif
    }
}
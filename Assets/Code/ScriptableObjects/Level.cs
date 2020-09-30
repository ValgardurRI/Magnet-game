using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static MagnetGame.Consts;

namespace MagnetGame
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Level", menuName = "Level", order = 1)]
    public class Level : ScriptableObject
    {
        public int LevelWidth;
        public int LevelHeight;
        public Piece[] Fields;

        [SerializeField]
        protected Font VisualizationFont;
        protected bool showFields;
        public int FieldIndex(int x, int y)
        {
            if (x >= LevelWidth || x < 0)
            {
                throw new ArgumentException("Illegal x index " + x + ", value must be 0-" + (LevelWidth - 1) + ".");
            }
            if (y >= LevelHeight || y < 0)
            {
                throw new ArgumentException("Illegal y index " + y + ", value must be 0-" + (LevelHeight - 1) + ".");
            }
            return y*LevelWidth + x;
        }

        public GameState ToState()
        {
            var stateFields = new Piece[Fields.Length];
            Fields.CopyTo(stateFields, 0);
            return new GameState{Fields = stateFields, Level = this};
        }

        public string Visualize(bool richText = false)
        {
            string visualization = "";
            for(int y = 0; y < LevelHeight; y++)
            {
                for (int x = 0; x < LevelWidth; x++)
                {
                    // Square value
                    string nextBit = "?";
                    var field = Fields[FieldIndex(x, y)];
                    if (field.Type == Piece.PieceType.Wall)
                        nextBit = "X";
                    else if (field.Type == Piece.PieceType.Hole)
                        nextBit = "O";
                    else if (field.Type == Piece.PieceType.Empty)
                        nextBit = " ";
                    else if (field.Type == Piece.PieceType.Endpoint)
                    {
                        nextBit = "#";
                        if (richText)
                            nextBit = "<color=green>" + nextBit + "</color>";
                    }
                    else
                    {
                        if (field.Type == Piece.PieceType.Magnet)
                            nextBit = field.MagnetStrength.ToString();
                        if (field.Type == Piece.PieceType.Player)
                            nextBit = "\u0394";
                        if (richText)
                        {
                            if (field.MagnetPolarity == POSITIVE)
                                nextBit = "<color=red>" + nextBit + "</color>";
                            else
                                nextBit = "<color=blue>" + nextBit + "</color>";
                        }
                    }
                    visualization += nextBit;

                    // Borders
                    if (x == LevelWidth - 1)
                    {
                        visualization += "\n";
                    }
                    else
                    {
                        visualization += "/";
                    }
                }
                if(y != LevelHeight - 1)
                    visualization += String.Concat(Enumerable.Repeat("-", 2*LevelWidth - 1)) + "\n";
            }

            return visualization;
        }

        public void UpdateArraySize()
        {
            LevelWidth = Mathf.Clamp(LevelWidth, 1, 20);
            LevelHeight = Mathf.Clamp(LevelHeight, 1, 20);
            Array.Resize(ref Fields, LevelWidth * LevelHeight);
        }

        private void OnValidate()
        {
            UpdateArraySize();
        }

        [CustomEditor(typeof(Level))]
        public class LevelEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                Level myLevel = (Level)target;
                EditorGUI.BeginChangeCheck();
                int test1 = EditorGUILayout.IntField("Level Width", myLevel.LevelWidth);
                int test2 = EditorGUILayout.IntField("Level Height", myLevel.LevelHeight);
                if(EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Level size changed");
                    myLevel.LevelWidth = test1;
                    myLevel.LevelHeight = test2;
                    myLevel.OnValidate();
                    serializedObject.Update();
                }
                
                // Field related
                myLevel.showFields = EditorGUILayout.Foldout(myLevel.showFields, "Fields");
                if(myLevel.showFields)
                {
                    var fieldsProperty = serializedObject.FindProperty("Fields");
                    EditorGUI.indentLevel += 1;
                    for(int i = 0; i < fieldsProperty.arraySize; i++)
                    {
                        int x = i % myLevel.LevelWidth;
                        int y = i / myLevel.LevelWidth;
                        EditorGUILayout.PropertyField(fieldsProperty.GetArrayElementAtIndex(i), new GUIContent("Field (" + x + "," + y + ")"));
                    }
                    EditorGUI.indentLevel -= 1;
                }
                
                // Visualization related
                EditorGUILayout.LabelField("Level visualization");
                EditorGUI.BeginDisabledGroup(true);
                var oldFont = GUI.skin.font;
                GUI.skin.font = myLevel.VisualizationFont;
                var style = new GUIStyle("TextArea")
                {
                    richText = true
                };
                EditorGUILayout.TextArea(myLevel.Visualize(true), style);
                GUI.skin.font = oldFont;
                EditorGUI.EndDisabledGroup();

                myLevel.VisualizationFont = (Font)EditorGUILayout.ObjectField("Visualization Font", myLevel.VisualizationFont, typeof(Font), true);
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
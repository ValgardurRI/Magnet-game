using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static MagnetGame.Consts;

namespace MagnetGame
{
    [Serializable]
    [CreateAssetMenu(fileName = "Level", menuName = "Level", order = 1)]
    public class Level : ScriptableObject
    {
        // TODO: Add Json Serialization
        public int LevelWidth;
        public int LevelHeight;
        public BoardField[] Fields;

        [SerializeField]
        protected Font VisualizationFont;
        protected bool showFields;
        public int FieldIndex(int x, int y)
        {
            if (x >= LevelWidth || x < 0)
            {
                throw new ArgumentException("Illegal x index " + x + ", value must be 0-" + (LevelWidth - 1) + ".");
            }
            if (y >= LevelWidth || y < 0)
            {
                throw new ArgumentException("Illegal y index " + y + ", value must be 0-" + (LevelHeight - 1) + ".");
            }
            return y*LevelWidth + x;
        }

        public GameState ToState()
        {
            var stateFields = new BoardField[Fields.Length];
            Fields.CopyTo(stateFields, 0);
            return new GameState{Fields = stateFields, Level = this};
        }

        public string Visualize(bool richText = false)
        {
            string visualization = "";
            for(int y = 0; y < LevelHeight + LevelWidth; y++)
            {
                if(y == LevelHeight + LevelWidth - 1)
                {
                    break;
                }
                for(int x = 0; x < LevelWidth + LevelHeight; x++)
                {
                    string nextBit = "!";
                    if(x == LevelWidth + LevelHeight - 1)
                    {
                        nextBit = "\n";
                    }
                    else if (y % 2 == 1)
                    {
                        nextBit = "-";
                    }
                    else if (x % 2 == 1)
                    {
                        nextBit = "/";
                    }
                    else
                    {
                        // TODO: This only works for even x and y. Fix this.
                        var field = Fields[FieldIndex(x/2, y/2)];
                        if (field.FieldState == BoardField.FieldType.Wall)
                            nextBit = "X";
                        else if (field.FieldState == BoardField.FieldType.Hole)
                            nextBit = "O";
                        else if (field.FieldState == BoardField.FieldType.Empty)
                            nextBit = " ";
                        else if (field.FieldState == BoardField.FieldType.Endpoint)
                        {
                            nextBit = "#";
                            if (richText)
                                nextBit = "<color=green>" + nextBit + "</color>";
                        }
                        else
                        {
                            if (field.FieldState == BoardField.FieldType.Magnet)
                                nextBit = field.MagnetStrength.ToString();
                            if (field.FieldState == BoardField.FieldType.Player)
                                nextBit = "\u0394";
                            if (richText)
                            {
                                if (field.MagnetPolarity == POSITIVE)
                                    nextBit = "<color=red>" + nextBit + "</color>";
                                else
                                    nextBit = "<color=blue>" + nextBit + "</color>";
                            }
                        }

                    }
                    visualization += nextBit;
                }

            }

            return visualization;
        }

        private void OnValidate()
        {
            LevelWidth = Mathf.Clamp(LevelWidth, 1, 20);
            LevelHeight = Mathf.Clamp(LevelHeight, 1, 20);
            Array.Resize(ref Fields, LevelWidth * LevelHeight);
        }

        [CustomEditor(typeof(Level))]
        public class LevelEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                Level myLevel = (Level)target;
                EditorGUI.BeginChangeCheck();
                myLevel.LevelWidth = EditorGUILayout.IntField("Level Width", myLevel.LevelWidth);
                myLevel.LevelHeight = EditorGUILayout.IntField("Level Height", myLevel.LevelHeight);
                if(EditorGUI.EndChangeCheck())
                {
                    myLevel.OnValidate();
                    serializedObject.FindProperty("Fields").arraySize = myLevel.LevelWidth * myLevel.LevelHeight;
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
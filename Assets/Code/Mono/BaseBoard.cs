using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MagnetGame
{
    class BaseBoard: MonoBehaviour
    {
        public int Rows => level != null ? level.LevelHeight : 0;
        public int Columns => level != null ? level.LevelWidth : 0;

        [SerializeField]
        protected string levelString;
        [SerializeField]
        protected bool editMode;

        [SerializeField]
        protected BoardConfiguration configuration;
        protected MonoPlayer player;
        protected IEnumerable<MonoMagnet> magnets;
        protected MonoField[] fields;
        protected StateMachine stateMachine;

        [SerializeField]
        protected Level level;

        void Start()
        {
            Setup();
        }

        public virtual void Setup()
        {
            if(level != null)
            {
                fields = new MonoField[Rows*Columns];
                magnets = new List<MonoMagnet>();
                InstantiateFields();
            }
        }

        public void SetLevel()
        {
            SetLevel(levelString);
        }

        public void SetLevel(string levelString)
        {

        }

        protected virtual void InstantiateFields() 
        {
            Rect boardRect = ((RectTransform)transform).rect;
            float fieldWidth = boardRect.width/Columns;
            float fieldHeight = boardRect.height/Rows;
            Vector2 fieldDimensions = new Vector2(fieldWidth, fieldHeight);
            Vector2 fieldOriginPoint = (Vector2)transform.position - new Vector2(boardRect.width, boardRect.height)/2 + fieldDimensions/2;
            Transform fieldTransform = transform.Find("fields");
            for (int y = 0; y < Rows; y++)
            {
                for(int x = 0; x < Columns; x++)
                {
                    //create the field and give it a descriptive name.
                    MonoField newfield = MonoField.Instantiate(configuration.fieldPrefab, fieldTransform);
                    newfield.name = "Field(" + x + ", " + y + ")"; 
                    //positioning
                    Vector2 fieldPosition = new Vector2(x*fieldWidth, y*fieldHeight) + fieldOriginPoint;
                    //Setup
                    /*
                    fields[level.FieldIndex(x,y)] = newfield;
                    fields[level.FieldIndex(x,y)].Setup(fieldDimensions, fieldPosition);
                    */
                    
                    fields[y*Columns + x] = newfield;
                    fields[y*Columns + x].Setup(fieldDimensions, fieldPosition);
                }
            }
        }

        [CustomEditor(typeof(BaseBoard))]
        public class BoardEditor : Editor 
        {
            public override void OnInspectorGUI()
            {
                BaseBoard myBoard = (BaseBoard)target;
                myBoard.level = (Level)EditorGUILayout.ObjectField("Level", myBoard.level, typeof(Level), true);
                myBoard.editMode = EditorGUILayout.Toggle("Board edit mode", myBoard.editMode);
                EditorGUILayout.LabelField("Level string");
                EditorGUI.BeginDisabledGroup(!myBoard.editMode);
                myBoard.levelString = EditorGUILayout.TextArea(myBoard.levelString, GUILayout.MaxHeight(75));
                EditorGUI.EndDisabledGroup();
                if(myBoard.editMode)
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

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.IntField("Columns", myBoard.Columns);
                EditorGUILayout.IntField("Rows", myBoard.Rows);
                EditorGUI.EndDisabledGroup();

                myBoard.configuration = (BoardConfiguration)EditorGUILayout.ObjectField("Configuration", myBoard.configuration, typeof(BoardConfiguration), true);
            }
        }
    }
}
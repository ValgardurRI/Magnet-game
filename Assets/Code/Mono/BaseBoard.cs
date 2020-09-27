using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MagnetGame
{
    class BaseBoard: MonoBehaviour
    {

        [SerializeField]
        protected int _rows;
        [SerializeField]
        protected int _columns;
        public int Rows => _rows;
        public int Columns => _columns;

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
        protected Level level;

        void Start()
        {
            Setup();
        }

        public virtual void Setup()
        {
            fields = new MonoField[_rows*_columns];
            magnets = new List<MonoMagnet>();
            InstantiateFields();
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
            float fieldWidth = boardRect.width/_columns;
            float fieldHeight = boardRect.height/_rows;
            Vector2 fieldDimensions = new Vector2(fieldWidth, fieldHeight);
            Vector2 fieldOriginPoint = (Vector2)transform.position - new Vector2(boardRect.width, boardRect.height)/2 + fieldDimensions/2;
            Transform fieldTransform = transform.Find("fields");
            for (int y = 0; y < _rows; y++)
            {
                for(int x = 0; x < _columns; x++)
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
                    
                    fields[y*_columns + x] = newfield;
                    fields[y*_columns + x].Setup(fieldDimensions, fieldPosition);
                }
            }
        }

        [CustomEditor(typeof(BaseBoard))]
        public class BoardEditor : Editor 
        {
            public override void OnInspectorGUI()
            {
                BaseBoard myBoard = (BaseBoard)target;
                myBoard.editMode = EditorGUILayout.Toggle("Board edit mode", myBoard.editMode);
                EditorGUILayout.ObjectField("Configuration", myBoard.configuration, typeof(BoardConfiguration), true);
                EditorGUILayout.LabelField("Level string");
                if(myBoard.editMode)
                {
                    myBoard.levelString = EditorGUILayout.TextArea(myBoard.levelString, GUILayout.MaxHeight(75));
                    if(GUILayout.Button("Set board"))
                    {
                        myBoard.SetLevel();
                    }
                    
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.IntField("Columns", myBoard._columns);
                    EditorGUILayout.IntField("Rows", myBoard._rows);
                    EditorGUI.EndDisabledGroup();
                }
                else
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextArea(myBoard.levelString, GUILayout.MaxHeight(75));
                    EditorGUI.EndDisabledGroup();
                    if(GUILayout.Button("Copy level string to clipboard"))
                    {
                        EditorGUIUtility.systemCopyBuffer = myBoard.levelString;
                    }
                    myBoard._columns = EditorGUILayout.IntField("Columns", myBoard._columns);
                    myBoard._rows = EditorGUILayout.IntField("Rows", myBoard._rows);
                }
            }
        }
    }
}
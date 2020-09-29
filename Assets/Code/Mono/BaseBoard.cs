using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace MagnetGame
{
    [RequireComponent(typeof(AspectRatioFitter))]
    public abstract class BaseBoard: MonoBehaviour
    {
        public int Rows => level != null ? level.LevelHeight : 0;
        public int Columns => level != null ? level.LevelWidth : 0;

        [SerializeField]
        protected string levelString;

        [SerializeField]
        protected BoardConfiguration configuration;
        protected MonoPlayer player;
        protected MonoField[] fields;
        protected StateMachine stateMachine;
        protected Transform fieldTransform;
        protected Transform pieceTransform;
        protected Vector2 fieldSize;

        [SerializeField]
        protected Level level;

        void Start()
        {
            Setup();
        }
        public abstract Vector3? Place(Draggable piece, MonoField square);

        public virtual void Setup()
        {
            if(level != null)
            {
                fields = new MonoField[Rows*Columns];
                GetComponent<AspectRatioFitter>().aspectRatio = Columns / (float)Rows;
                fieldTransform = transform.Find("fields");
                pieceTransform = transform.Find("fieldPieces");

                Rect boardRect = ((RectTransform)transform).rect;
                float fieldWidth = boardRect.width / Columns;
                float fieldHeight = boardRect.height / Rows;
                fieldSize = new Vector2(fieldWidth, fieldHeight);
                InstantiateBoard();
            }
        }

        public void SetLevel()
        {
            SetLevel(levelString);
        }

        public void SetLevel(string levelString)
        {

        }

        public Draggable AddPiece(Piece piece, MonoField field)
        {
            Draggable draggablePiece = null;
            if (piece.Type == Piece.PieceType.Wall)
            {
                var wall = Instantiate(configuration.wallPrefab, pieceTransform);
                wall.Setup(fieldSize, field.transform.position, this, piece);
                draggablePiece = wall;
            }
            else if (piece.Type == Piece.PieceType.Hole)
            {
                var hole = Instantiate(configuration.holePrefab, pieceTransform);
                hole.Setup(fieldSize, field.transform.position, this, piece);
                draggablePiece = hole;
            }
            else if (piece.Type == Piece.PieceType.Endpoint)
            {
                var endpoint = Instantiate(configuration.endpointPrefab, pieceTransform);
                endpoint.Setup(fieldSize, field.transform.position, this, piece);
                draggablePiece = endpoint;
            }
            else if (piece.Type == Piece.PieceType.Magnet)
            {
                var magnet = Instantiate(configuration.magnetPrefab, pieceTransform);
                magnet.Setup(fieldSize, field.transform.position, this, piece, piece.MagnetStrength, piece.MagnetPolarity);
                draggablePiece = magnet;
            }
            else if (piece.Type == Piece.PieceType.Player)
            {
                var player = Instantiate(configuration.playerPrefab, pieceTransform);
                player.Setup(fieldSize, field.transform.position, this, piece, piece.MagnetStrength, piece.MagnetPolarity);
                draggablePiece = player;
            }
            Place(draggablePiece, field);
            return draggablePiece;
        }

        protected virtual void InstantiateBoard() 
        {
            Rect boardRect = ((RectTransform)transform).rect;
            float fieldWidth = boardRect.width/Columns;
            float fieldHeight = boardRect.height/Rows;
            Vector2 fieldOriginPoint = (Vector2)transform.position - new Vector2(boardRect.width, boardRect.height)/2 + fieldSize/2;
            for (int y = 0; y < Rows; y++)
            {
                for(int x = 0; x < Columns; x++)
                {
                    //create the field and give it a descriptive name.
                    MonoField newfield = MonoField.Instantiate(configuration.fieldPrefab, fieldTransform);
                    newfield.name = "Field(" + x + ", " + y + ")"; 
                    
                    //positioning
                    Vector2 fieldPosition = new Vector2(x*fieldWidth, (Rows-y-1)*fieldHeight) + fieldOriginPoint;

                    //Setup
                    fields[level.FieldIndex(x,y)] = newfield;
                    fields[level.FieldIndex(x,y)].Setup(fieldSize, fieldPosition);

                    // Instantiate board pieces
                    var piece = level.Fields[level.FieldIndex(x, y)];
                    AddPiece(piece, fields[level.FieldIndex(x, y)]);
                }
            }
        }
    }
}
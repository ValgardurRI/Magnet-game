using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace MagnetGame
{
    [RequireComponent(typeof(AspectRatioFitter))]
    [System.Serializable]
    public abstract class BaseBoard: MonoBehaviour
    {
        public int Rows => level != null ? level.LevelHeight : 0;
        public int Columns => level != null ? level.LevelWidth : 0;

        [SerializeField]
        protected BoardConfiguration configuration;
        protected MonoPlayer player;
        protected MonoField[] fields;
        protected StateMachine stateMachine;
        protected Transform fieldTransform;
        protected Transform pieceTransform;
        //protected Vector2 fieldSize;

        [SerializeField]
        protected Level level;

        void Start()
        {
            Setup();
        }
        public abstract Vector3? Place(Draggable piece, MonoField square);

        public virtual void Setup()
        {
            fieldTransform = transform.Find("fields");
            pieceTransform = transform.Find("fieldPieces");
            if(level != null)
            {
                InstantiateBoard();
            }
        }

        public Draggable AddPiece(Piece piece, MonoField field)
        {
            var fieldAnchorMin = ((RectTransform)field.transform).anchorMin;
            var fieldAnchorMax = ((RectTransform)field.transform).anchorMax;
            var fieldSize = new Vector2(1,1);
            Draggable draggablePiece = null;
            if (piece.Type == Piece.PieceType.Wall)
            {
                var wall = Instantiate(configuration.wallPrefab, pieceTransform);
                wall.Setup(fieldAnchorMin, fieldAnchorMax, this, piece);
                draggablePiece = wall;
            }
            else if (piece.Type == Piece.PieceType.Hole)
            {
                var hole = Instantiate(configuration.holePrefab, pieceTransform);
                hole.Setup(fieldAnchorMin, fieldAnchorMax, this, piece);
                draggablePiece = hole;
            }
            else if (piece.Type == Piece.PieceType.Endpoint)
            {
                var endpoint = Instantiate(configuration.endpointPrefab, pieceTransform);
                endpoint.Setup(fieldAnchorMin, fieldAnchorMax, this, piece);
                draggablePiece = endpoint;
            }
            else if (piece.Type == Piece.PieceType.Magnet)
            {
                var magnet = Instantiate(configuration.magnetPrefab, pieceTransform);
                magnet.Setup(fieldAnchorMin, fieldAnchorMax, this, piece, piece.MagnetStrength, piece.MagnetPolarity);
                draggablePiece = magnet;
            }
            else if (piece.Type == Piece.PieceType.Player)
            {
                var player = Instantiate(configuration.playerPrefab, pieceTransform);
                player.Setup(fieldAnchorMin, fieldAnchorMax, this, piece, piece.MagnetStrength, piece.MagnetPolarity);
                draggablePiece = player;
            }
            else
            {
                return null;
            }
            Place(draggablePiece, field);
            return draggablePiece;
        }
        
        protected void ClearBoard()
        {
            foreach(Transform child in fieldTransform)
            {
                Destroy(child.gameObject);
            }
            foreach(Transform child in pieceTransform)
            {
                Destroy(child.gameObject);
            }
        }

        protected virtual void InstantiateBoard() 
        {
            fields = new MonoField[Rows*Columns];
            GetComponent<AspectRatioFitter>().aspectRatio = Columns / (float)Rows;

            Vector2 boardSize = new Vector2(((RectTransform)transform).rect.width * transform.lossyScale.x, ((RectTransform)transform).rect.height * transform.lossyScale.y);
            for (int y = 0; y < Rows; y++)
            {
                for(int x = 0; x < Columns; x++)
                {
                    // Create the field and give it a descriptive name.
                    MonoField newField = MonoField.Instantiate(configuration.fieldPrefab, fieldTransform);
                    newField.name = "Field(" + x + ", " + y + ")"; 
                    
                    // Positioning
                    //Vector2 fieldPosition = new Vector2(x*fieldWidth, (Rows-y-1)*fieldHeight) + fieldOriginPoint;

                    // Anchoring
                    Vector2 minAnchor = new Vector2(x/(float)Columns, y/(float)Rows);
                    Vector2 maxAnchor = new Vector2((x + 1)/(float)Columns, (y + 1)/(float)Rows);
                    var newFieldTransform = (RectTransform)newField.transform;
                    newFieldTransform.anchorMin = minAnchor;
                    newFieldTransform.anchorMax = maxAnchor;

                    // Setup
                    fields[level.FieldIndex(x,y)] = newField;
                    fields[level.FieldIndex(x,y)].Setup(level.FieldIndex(x,y));

                    // Instantiate board pieces
                    var piece = level.Fields[level.FieldIndex(x, y)];
                    var monoPiece = AddPiece(piece, fields[level.FieldIndex(x, y)]);
                }
            }
        }
    }
}
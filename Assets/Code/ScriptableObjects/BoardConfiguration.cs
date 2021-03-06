﻿using UnityEngine;
using System.Collections;

namespace MagnetGame
{
    [CreateAssetMenu(fileName = "BoardConfiguration", menuName = "Config/BoardConfiguration", order = 1)]
    public class BoardConfiguration : ScriptableObject {
        public MonoPlayer playerPrefab;
        public MonoMagnet magnetPrefab;
        public MonoField fieldPrefab;
        public MonoDoor doorPrefab; 
        public MonoLock lockPrefab;
        public Draggable wallPrefab;
        public Draggable holePrefab;
        public Draggable endpointPrefab;
    }
}
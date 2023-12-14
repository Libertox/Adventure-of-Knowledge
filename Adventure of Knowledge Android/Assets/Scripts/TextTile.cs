using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    public class TextTile:MonoBehaviour
    {
        public enum TileType
        {
            Result,
            Answer,
            Question,
        }

        public Vector3 StartPosition { get; private set; }
        [field: SerializeField] public TextTileVisual GameTileVisual { get; private set; }

        [SerializeField] private TileType type;

        public bool IsResultTile() => type == TileType.Result;

        public bool IsAnswerTile() => type == TileType.Answer;

        public void SetNewTileType(TileType tileType) => type = tileType;

        public void SetStartPosition(Vector3 startPositon) => StartPosition = startPositon;

        public virtual bool DetectResultTile(TextTile detectTile) => false;
  
    }
}

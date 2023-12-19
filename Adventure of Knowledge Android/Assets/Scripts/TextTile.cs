using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    public class TextTile:MonoBehaviour,IDraggable
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

        private DragState dragState;
        private Vector2 tileSize;
        private readonly float movementSpeed = 16f;

        private void Awake()
        {
            BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
            tileSize = new Vector2(boxCollider2D.size.x, boxCollider2D.size.y);

            dragState = DragState.None;
        }
        public virtual void Start() => GameManager.Instance.OnNewStageLoaded += GameManager_OnNewStageLoaded;

        private void GameManager_OnNewStageLoaded(object sender, EventArgs e)
        {
            transform.position = StartPosition;
            dragState = DragState.None;
        }

        public bool IsResultTile() => type == TileType.Result;

        public void SetNewTileType(TileType tileType) => type = tileType;

        public void SetStartPosition(Vector3 startPositon) => StartPosition = startPositon;

        public virtual bool DetectResultTile(TextTile detectTile) => false;

        public void Drag()
        {
            if (GameManager.Instance.IsPause()) return;

            dragState = DragState.IsDrag;
        }

        public void Drop()
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(transform.position, tileSize, 1f);
            foreach (var collider2D in collider2Ds)
            {
                if (collider2D.TryGetComponent(out TextTile detectGameTile))
                {
                    if (DetectResultTile(detectGameTile)) 
                    {
                        dragState = DragState.None;
                        return;
                    } 
                }
            }

            dragState = DragState.IsDrop;
        }

        private void Update()
        {
            if (dragState == DragState.None) return;

            if (dragState == DragState.IsDrop)
            {
                transform.position = Vector3.MoveTowards(transform.position, StartPosition, Time.deltaTime * movementSpeed);
                return;
            }

            FollowTouchPosition();
        }

        private void FollowTouchPosition()
        {
            float posZOffset = 1f;
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(GameInputManager.Instance.GetControllerPosition());
            touchPosition.z = transform.position.z;

            touchPosition.z = StartPosition.z - posZOffset;
            transform.position = touchPosition;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnNewStageLoaded -= GameManager_OnNewStageLoaded;
        }
    }
}

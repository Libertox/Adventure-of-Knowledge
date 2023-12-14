using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{ 
    public class TextTileController:MonoBehaviour
    {
        private readonly float movementSpeed = 16f;

        private Vector2 tileSize;
        private TextTile gameTile;

        private bool isDrag = true;
        
        private void Awake()
        {
            BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
            tileSize = new Vector2(boxCollider2D.size.x, boxCollider2D.size.y);

            gameTile = GetComponent<TextTile>();
            isDrag = true;
        }

        private void Start() => GameManager.Instance.OnNewStageLoaded += GameManager_OnNewStageLoaded;
      
        private void GameManager_OnNewStageLoaded(object sender, EventArgs e)
        {
            transform.position = gameTile.StartPosition;
            isDrag = true;
        }

        private void OnMouseDrag()
        {
            if (!gameTile.IsAnswerTile() || GameManager.Instance.IsPause()) return;

            float posZOffset = 0.1f;
            isDrag = true;
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(GameInputManager.Instance.GetControllerPosition());
            touchPosition.z = gameTile.transform.position.z;

            touchPosition.z = gameTile.StartPosition.z - posZOffset;
            transform.position = touchPosition;
     
        }

        private void OnMouseUp()
        {
            if (!gameTile.IsAnswerTile()) return;

            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(transform.position, tileSize, 1f);
            foreach(var collider2D in collider2Ds)
            {
                if(collider2D.TryGetComponent(out TextTile detectGameTile))
                {
                    if (gameTile.DetectResultTile(detectGameTile)) return;                              
                }
            }

            isDrag = false;
        }

        private void Update()
        {
            if (isDrag || !gameTile.IsAnswerTile()) return;

            transform.position = Vector3.MoveTowards(transform.position, gameTile.StartPosition, Time.deltaTime * movementSpeed);
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnNewStageLoaded -= GameManager_OnNewStageLoaded;
        }
    }
}

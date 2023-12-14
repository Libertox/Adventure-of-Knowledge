using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace AdventureOfKnowledge.LetterGame
{
    public class LetterTile:TextTile
    {
        public char AnswerLetter { get;private set; }

        private ObjectPool<LetterTile> letterTilePool;

        private void Start() => GameTileVisual.OnDisappeared += GameTileVisual_OnDisappeared;
       
        private void GameTileVisual_OnDisappeared(object sender, EventArgs e)
        {
            if (gameObject.activeInHierarchy)
                letterTilePool.Release(this);
        }

        public void SetAnswer(char answer)
        {
            AnswerLetter = answer;
            GameTileVisual.UppdateText(answer.ToString().ToUpper());
        }

        public void SetObjectPool(ObjectPool<LetterTile> objectPool) => letterTilePool = objectPool;
        
        public override bool DetectResultTile(TextTile detectTile)
        {
            LetterTile letterTile = detectTile as LetterTile;
            LetterGameManager letterGameManager = GameManager.Instance as LetterGameManager;
            if(letterTile.IsResultTile() && letterGameManager.CheckCorrectAnswer(letterTile.AnswerLetter,AnswerLetter))
            {
                transform.position = detectTile.transform.position;
                detectTile.SetNewTileType(TileType.Question);
                SetNewTileType(TileType.Question);
                return true;
            }
            return false;
        }

    }
}

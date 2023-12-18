using AdventureOfKnowledge.MathGame;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    public class MathTile: TextTile
    {    
        public int AnswerNumber { get; private set; }

        public override void Start() 
        {
            base.Start();
            SetStartPosition(transform.position);
        } 
        

        public void SetAnswer(int answer)
        {
            AnswerNumber = answer;
            GameTileVisual.UppdateText(answer.ToString());
        }

        public override bool DetectResultTile(TextTile detectTile)
        {
            MathGameManager mathGameManager = GameManager.Instance as MathGameManager;
            if (detectTile.IsResultTile() && mathGameManager.CheckCorrectAnswer(AnswerNumber))
            {
                transform.position = detectTile.transform.position;
                return true;
            }

            return false;
        }

    }
}

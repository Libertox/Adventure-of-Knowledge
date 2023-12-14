using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    public class GameSpeechBubble:SpeechBubble
    {
        [SerializeField] private DialogMessageSO correctAnswer;
        [SerializeField] private DialogMessageSO wrongAnswer;

        [TextArea]
        [SerializeField] private string tutorialMessage;


        private void Start()
        {
            GameManager.Instance.OnCorrectAnswer += GameManager_OnCorrectAnswer;
            GameManager.Instance.OnWrongAnswer += GameManager_OnWrongAnswer;
            GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
        }

        private void GameManager_OnGameStarted(object sender, EventArgs e)
        {
            StartCoroutine(SlowlyWriteDialog(tutorialMessage));
        }

        private void GameManager_OnWrongAnswer(object sender, EventArgs e)
        {
            if (isWriting) return;

            StartCoroutine(SlowlyWriteDialog(wrongAnswer.GetRandomDialogFromList()));
        }

        private void GameManager_OnCorrectAnswer(object sender, EventArgs e)
        {
            if (isWriting) return;

            StartCoroutine(SlowlyWriteDialog(correctAnswer.GetRandomDialogFromList()));
        }
    }
}

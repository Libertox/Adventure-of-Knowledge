
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace AdventureOfKnowledge.LetterGame
{
    public class LetterGameManager:GameManager
    {
        private const float NEW_STAGE_LOAD_TIME = 1f;

        [SerializeField] private LetterGameSettingsSO letterGameSettingsSO;

        private DifficultyLetterGameSettings difficultyLetterGameSettings;

        private int stage;
        private int correctNumberLetter;
        private int demandNumberCorrectLetter;

        private LetterFieldCreator letterFieldCreator;


        private void Awake()
        {
            if (!Instance)
                Instance = this;

            gameTimer = GetComponent<GameTimer>();
            letterFieldCreator = GetComponent<LetterFieldCreator>();
            OnGameStarted += LetterGameManager_OnGameStarted;
            OnCorrectAnswer += LetterGameManager_OnCorrectAnswer;

        }

        private void LetterGameManager_OnCorrectAnswer(object sender, System.EventArgs e) 
        {
            IncreaseStage();
            correctNumberLetter = 0;
        } 

        private void IncreaseStage()
        {
            stage++;
            if (stage >= difficultyLetterGameSettings.MaxStage)
                InvokeFinishGameEvent(difficultyLetterGameSettings);
            else
                StartCoroutine(LoadNewStageCoroutine());
        }

        private IEnumerator LoadNewStageCoroutine()
        {
            yield return new WaitForSeconds(NEW_STAGE_LOAD_TIME);
            
            demandNumberCorrectLetter = letterFieldCreator.DrawField();
            InvokeNewStageLoadEvent();
        }

        private void LetterGameManager_OnGameStarted(object sender, OnGameStartedEventArgs e)
        {
            difficultyLetterGameSettings = letterGameSettingsSO.GetDifficultyLevelSettings(e.difficultyLevel);
            letterFieldCreator.SetDifficultyLetterGameSettings(difficultyLetterGameSettings);
            demandNumberCorrectLetter = letterFieldCreator.DrawField();
        }

        public bool CheckCorrectAnswer(char correctAnswer, char selectAnswer)
        {
            IncreaseMoveNumber();
            if (string.Equals(correctAnswer.ToString(),selectAnswer.ToString(),StringComparison.OrdinalIgnoreCase))
            {
                correctNumberLetter++;
                if(demandNumberCorrectLetter <= correctNumberLetter)
                    InvokeCorrectAnswerEvent();

                return true;
            }
            InvokeWrongAnswerEvent();
            return false;
        }

    }
}

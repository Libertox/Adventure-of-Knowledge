
using System;
using UnityEngine;

namespace AdventureOfKnowledge
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; protected set; }

        public event EventHandler<OnGameStartedEventArgs> OnGameStarted;
        public event EventHandler<OnGameFinishedEventArgs> OnGameFinished;
        public event EventHandler<OnMovedEventArgs> OnMoved;
        public event EventHandler OnWrongAnswer;
        public event EventHandler OnCorrectAnswer;
        public event EventHandler OnNewStageLoaded;

        public class OnMovedEventArgs : EventArgs { public int numberOfMove; }
        public class OnGameFinishedEventArgs : EventArgs { public int gameScore; }
        public class OnGameStartedEventArgs: EventArgs { public DifficultyLevel difficultyLevel; }

        public int NumberOfMoves { get; private set; }

        private bool isPause;

        protected GameTimer gameTimer;

        public void SetDiffucltyLevel(DifficultyLevel difficultyLevel)
        {
            OnGameStarted?.Invoke(this, new OnGameStartedEventArgs {difficultyLevel = difficultyLevel});
        }

        public void RestartGame() 
        {
            SceneLoader.LoadTheSameScene();
            MusicManager.Instance.SaveClipTime();
        } 
      
        public void IncreaseMoveNumber()
        {
            NumberOfMoves++;
            OnMoved?.Invoke(this, new OnMovedEventArgs
            {
                numberOfMove = NumberOfMoves
            });
        }

        protected void InvokeFinishGameEvent(DifficultyLevelSettings difficultyLevelSettings) 
        {
            int gameScore = CalculateGameScore(difficultyLevelSettings);
            int curremtDiamondAmount = SaveSystem.LoadDiamondAmount();
            OnGameFinished?.Invoke(this, new OnGameFinishedEventArgs { gameScore = gameScore });

            SoundManager.Instance.PlayCompleteGameSound();

            curremtDiamondAmount += gameScore;
            SaveSystem.SaveDiamondAmount(curremtDiamondAmount);

            if (IsTheBestLevelTime((int)gameTimer.GameTime, difficultyLevelSettings.level))
                SaveSystem.SaveTheBestLevelScore((int)gameTimer.GameTime, difficultyLevelSettings.level, SceneLoader.GetActiveSceneName());
        }


        protected void InvokeCorrectAnswerEvent()
        {
            SoundManager.Instance.PlayCorrectAnswerSound();
            OnCorrectAnswer?.Invoke(this, EventArgs.Empty);
        } 

        protected void InvokeWrongAnswerEvent() => OnWrongAnswer?.Invoke(this, EventArgs.Empty);

        protected void InvokeNewStageLoadEvent() => OnNewStageLoaded?.Invoke(this, EventArgs.Empty);

        public virtual int CalculateGameScore(DifficultyLevelSettings difficultyLevelSettings) 
        {
            int lostPointOfTime = gameTimer.GameTime > difficultyLevelSettings.PointLossTime ? (int)(gameTimer.GameTime - difficultyLevelSettings.PointLossTime) : 0;
            int lostPointOfMoves = NumberOfMoves > difficultyLevelSettings.PointLossMove ? NumberOfMoves - difficultyLevelSettings.PointLossMove : 0;
            int gameScore = difficultyLevelSettings.MaxPointToEarn - lostPointOfMoves - lostPointOfTime;
      
            if (gameScore < difficultyLevelSettings.MinPointToEarn) return difficultyLevelSettings.MinPointToEarn;
            else return gameScore;
        }

        private bool IsTheBestLevelTime(int currentScore, DifficultyLevel difficultyLevel) => currentScore < SaveSystem.LoadTheBestLevelScore(difficultyLevel, SceneLoader.GetActiveSceneName());

        public void SetPause(bool isPause) => this.isPause = isPause;

        public bool IsPause() => isPause;
    }
}



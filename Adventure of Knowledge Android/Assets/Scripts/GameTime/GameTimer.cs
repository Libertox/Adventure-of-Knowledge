using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    public class GameTimer:MonoBehaviour
    {
        public static event EventHandler<OnTimeChangedEventArgs> OnTimeChanged;
        public class OnTimeChangedEventArgs : EventArgs { public int second; }

        public float GameTime { get; private set; }

        private bool isStartTimer;

        private void Start()
        {
            Time.timeScale = 1f;
            GameManager.Instance.OnGameStarted += MemoryGameManager_OnGameStarted;
            GameManager.Instance.OnGameFinished += GameManager_OnGameFinished;
        }

        private void GameManager_OnGameFinished(object sender, GameManager.OnGameFinishedEventArgs e) => isStartTimer = false;
       
        private void MemoryGameManager_OnGameStarted(object sender, EventArgs e) => isStartTimer = true;
        

        private void Update()
        {
            if (!isStartTimer) return;

            GameTime += Time.deltaTime;
         
            OnTimeChanged?.Invoke(this, new OnTimeChangedEventArgs
            {
                second = (int)GameTime,
            });
                
        }
    }
}

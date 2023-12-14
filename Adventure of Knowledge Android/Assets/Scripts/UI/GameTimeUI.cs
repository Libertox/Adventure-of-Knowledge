using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AdventureOfKnowledge.UI
{ 
    public class GameTimeUI:MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI gameTimeText;

        private void Start()
        {
            GameTimer.OnTimeChanged += GameTimer_OnTimeChanged;

            GameManager.Instance.OnGameStarted += MemoryGameManager_OnGameStarted;

            Hide();
        }

        private void GameTimer_OnTimeChanged(object sender, GameTimer.OnTimeChangedEventArgs e) => gameTimeText.SetText($"TIME: {e.second}");
            
        private void MemoryGameManager_OnGameStarted(object sender, EventArgs e) => Show();
        

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);

        private void OnDestroy()
        {
            GameTimer.OnTimeChanged -= GameTimer_OnTimeChanged;
        }   
    }
}

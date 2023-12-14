using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AdventureOfKnowledge.UI
{
    public class NumberOfMovesTextUI:MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI numberOfMovesText;
        [SerializeField] private TextUIAnimation textUIAnimation;

        private void Start()
        {
            GameManager.Instance.OnMoved += MemoryGameManager_OnMoved;

            GameManager.Instance.OnGameStarted += MemoryGameManager_OnGameStarted;

            Hide();
        }

        private void MemoryGameManager_OnGameStarted(object sender, EventArgs e) => Show();
        

        private void MemoryGameManager_OnMoved(object sender, GameManager.OnMovedEventArgs e)
        {
            numberOfMovesText.SetText($"MOVES: {e.numberOfMove}");
            textUIAnimation.ChangeSizeText();
        }

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);
    }
}

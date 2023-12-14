using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AdventureOfKnowledge
{
    public class CompleteLevelUI:MonoBehaviour
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private Button backToMenuButton;

        [SerializeField] private TextMeshProUGUI scoreText;

        [SerializeField] private RectTransform panelTransform;

        private void Awake()
        {
            restartButton.onClick.AddListener(() => 
            {
                SoundManager.Instance.PlayButtonSound();
                GameManager.Instance.RestartGame();
            });

            backToMenuButton.onClick.AddListener(() => 
            {
                SoundManager.Instance.PlayButtonSound();
                SceneLoader.LoadScene(GameScene.LevelChoiceMenu);
            });
        }

        private void Start()
        {
            GameManager.Instance.OnGameFinished += MemoryGameManager_OnGameFinished;

            Hide();
        }

        private void MemoryGameManager_OnGameFinished(object sender, GameManager.OnGameFinishedEventArgs e)
        {
            float animationEndValue = 15f;
            float animationDuration = 1f;

            Show();
            panelTransform.DOAnchorPos3DY(animationEndValue, animationDuration);
            scoreText.SetText($"{e.gameScore}");
        }

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);

    }
}

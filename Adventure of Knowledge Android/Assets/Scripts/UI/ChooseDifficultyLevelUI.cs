using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdventureOfKnowledge.UI
{
    public class ChooseDifficultyLevelUI:MonoBehaviour
    {
        [SerializeField] private Button easyLevelButton;
        [SerializeField] private Button mediumLevelButton;
        [SerializeField] private Button hardLevelButton;

        private void Awake()
        {
            easyLevelButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonSound();
                GameManager.Instance.SetDiffucltyLevel(DifficultyLevel.Easy);
                Hide();
            });

            mediumLevelButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonSound();
                GameManager.Instance.SetDiffucltyLevel(DifficultyLevel.Medium);
                Hide();
            });

            hardLevelButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonSound();
                GameManager.Instance.SetDiffucltyLevel(DifficultyLevel.Hard);
                Hide();
            });

        }

        private void Hide() => gameObject.SetActive(false);

    }
}

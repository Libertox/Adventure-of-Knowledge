using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AdventureOfKnowledge.UI
{
    public class LevelChoiceElementUI:MonoBehaviour
    {
        [SerializeField] private Button loadLevelButton;

        [SerializeField] private FadeIamgeUI fadeIamgeUI;

        [SerializeField] private GameScene gameSceneToLoad;

        [SerializeField] private TextMeshProUGUI easyScoreText;
        [SerializeField] private TextMeshProUGUI mediumScoreText;
        [SerializeField] private TextMeshProUGUI hardScoreText;

        private void Awake()
        {
            Time.timeScale = 1f;

            UpdateTheBestScoreText();

            loadLevelButton.onClick.AddListener(() => 
            { 
                SoundManager.Instance.PlayButtonSound(); 
                fadeIamgeUI.FadeToBlack(() => SceneLoader.LoadScene(gameSceneToLoad)); 
            });     
        }

        private void UpdateTheBestScoreText()
        {
            SaveManager.LoadTheBestLevelScore(DifficultyLevel.Easy, gameSceneToLoad.ToString(),(callback) => 
            {
                easyScoreText.text = callback.Value == null ? "EASY " : "EASY " + callback.Value.ToString();
            });

            SaveManager.LoadTheBestLevelScore(DifficultyLevel.Medium, gameSceneToLoad.ToString(), (callback) =>
            {
                mediumScoreText.text = callback.Value == null ? "MEDIUM " : "MEDIUM " + callback.Value.ToString();
            });

            SaveManager.LoadTheBestLevelScore(DifficultyLevel.Hard, gameSceneToLoad.ToString(), (callback) =>
            {
                hardScoreText.text = callback.Value == null ? "HARD " : "HARD " + callback.Value.ToString();
            });
        }

    }
}

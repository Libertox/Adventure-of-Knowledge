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

            loadLevelButton.onClick.AddListener(() => 
            { 
                SoundManager.Instance.PlayButtonSound(); 
                fadeIamgeUI.FadeToBlack(() => SceneLoader.LoadScene(gameSceneToLoad)); 
            });

            UpdateTheBestScoreText();
        }

        private void UpdateTheBestScoreText()
        {
            int easyBestTime = SaveSystem.LoadTheBestLevelScore(DifficultyLevel.Easy, gameSceneToLoad.ToString());
            int mediumBestTime = SaveSystem.LoadTheBestLevelScore(DifficultyLevel.Medium, gameSceneToLoad.ToString());
            int hardBestTime = SaveSystem.LoadTheBestLevelScore(DifficultyLevel.Hard, gameSceneToLoad.ToString());

            easyScoreText.text = easyBestTime == int.MaxValue?"EASY " : "EASY " + easyBestTime.ToString();
            mediumScoreText.text = mediumBestTime == int.MaxValue? "MEDIUM ": "MEDIUM " + mediumBestTime.ToString();
            hardScoreText.text = hardBestTime == int.MaxValue? "HARD " : "HARD " + hardBestTime.ToString();

        }

    }
}

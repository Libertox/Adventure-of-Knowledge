using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdventureOfKnowledge.UI
{
    public class PauseMenuUI:MonoBehaviour
    {
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button quitButton;

        private ButtonAnimation resumeButtonAnimation;

        private void Awake()
        {
            resumeButtonAnimation = resumeButton.GetComponent<ButtonAnimation>();

            pauseButton.onClick.AddListener(() => 
            {
                SoundManager.Instance.PlayButtonSound();
                Time.timeScale = 0f;
                GameManager.Instance.SetPause(true);
                Show();
            });

            resumeButton.onClick.AddListener(() => 
            {
                resumeButtonAnimation.ScaleDown();
                SoundManager.Instance.PlayButtonSound();    
                Time.timeScale = 1f;
                GameManager.Instance.SetPause(false);
                Hide();
            });


            restartButton.onClick.AddListener(() => 
            { 
                SoundManager.Instance.PlayButtonSound(); 
                GameManager.Instance.RestartGame(); 
            });

            quitButton.onClick.AddListener(() => 
            { 
                SoundManager.Instance.PlayButtonSound(); 
                SceneLoader.LoadScene(GameScene.LevelChoiceMenu); 
            });

        }

        private void Start() => Hide();


        private void Show() => gameObject.SetActive(true);
        
        private void Hide() => gameObject.SetActive(false);

}
}

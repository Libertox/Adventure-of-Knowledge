using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdventureOfKnowledge.UI
{
    public class MainMenuUI:MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button fortuneWheelButton;
        [SerializeField] private Button monsterCreatorButton;

        [SerializeField] private FadeIamgeUI fadeIamgeUI;

        private void Awake()
        {       
            playButton.onClick.AddListener(() => 
            { 
                SoundManager.Instance.PlayButtonSound();
                fadeIamgeUI.FadeToBlack(() => SceneLoader.LoadScene(GameScene.LevelChoiceMenu)); 
            });

            fortuneWheelButton.onClick.AddListener(() => 
            { 
                SoundManager.Instance.PlayButtonSound(); 
                fadeIamgeUI.FadeToBlack(() => SceneLoader.LoadScene(GameScene.FortuneWheel)); 
            });

            monsterCreatorButton.onClick.AddListener(() => 
            { 
                SoundManager.Instance.PlayButtonSound(); 
                fadeIamgeUI.FadeToBlack(() => SceneLoader.LoadScene(GameScene.MonsterCreator)); 
            });
        }
    }
}

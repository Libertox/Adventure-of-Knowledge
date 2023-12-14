using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace AdventureOfKnowledge.UI
{
    public class BackMainMenuButtonUI:MonoBehaviour
    {
        [SerializeField] private Button backMainMenuButton;

        [SerializeField] private FadeIamgeUI fadeIamgeUI;

        private void Awake()
        {
            backMainMenuButton.onClick.AddListener(() => 
            {
                SoundManager.Instance.PlayButtonSound();
                MusicManager.Instance.SaveClipTime();
                fadeIamgeUI.FadeToBlack(() => SceneLoader.LoadScene(GameScene.MainMenu));
            });
        }
    }


}

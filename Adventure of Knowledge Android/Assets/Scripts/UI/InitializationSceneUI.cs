using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace AdventureOfKnowledge.UI
{
    public class InitializationSceneUI:MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private Button continueButton;

        [SerializeField] private FadeIamgeUI fadeIamgeUI;
        private void Awake()
        {
 
            if (SaveSystem.LoadPlayerName() != "")
            {
                SceneLoader.LoadScene(GameScene.MainMenu);
                return;
            }
         
            continueButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonSound();
                SaveSystem.SavePlayerName(nameInputField.text);
                fadeIamgeUI.FadeToBlack(() => SceneLoader.LoadScene(GameScene.MainMenu));
            });

            nameInputField.onValueChanged.AddListener((string value) =>
            {
                if(value != "")
                   continueButton.gameObject.SetActive(true);                
                else
                   continueButton.gameObject.SetActive(false);      
            });

            continueButton.gameObject.SetActive(false);
            
        }
    }
}

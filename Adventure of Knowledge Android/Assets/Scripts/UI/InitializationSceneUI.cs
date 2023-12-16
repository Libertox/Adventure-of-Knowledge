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
            SaveManager.InitializeDatabase();

            SaveManager.LoadPlayerName((callback) =>
            {
                string playerName = "";

                if(callback.Value != null)
                    playerName = callback.Value.ToString();

                if (playerName != "")
                    SceneLoader.LoadScene(GameScene.MainMenu);
                else
                    fadeIamgeUI.FadeFromBlack(() => { fadeIamgeUI.Hide(); });
            });
        

            continueButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonSound();
                SaveManager.SavePlayerName(nameInputField.text);
               
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

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    public class MainMenuSpeechBubble:SpeechBubble
    {
        [SerializeField] private DialogMessageSO dialogMessageSO;
        [SerializeField] private float timeBetweenDialog;

        private float time;

        private void Start()
        {
            SaveManager.LoadPlayerName((callback) =>
            {
                string greetMessage = "HELLO \n" + callback.Value.ToString() + " !!!";
                StartCoroutine(SlowlyWriteDialog(greetMessage));
            });     
        }

        private void Update()
        {
            time += Time.deltaTime;
            if(time > timeBetweenDialog)
            {
                time = 0;
                StartCoroutine(SlowlyWriteDialog(dialogMessageSO.GetRandomDialogFromList()));
            }
        }

    }
}

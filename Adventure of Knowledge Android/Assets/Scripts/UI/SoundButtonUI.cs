using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdventureOfKnowledge.UI
{
    public class SoundButtonUI:MonoBehaviour
    {

        [SerializeField] private Button changeSoundVolumeButton;

        [SerializeField] private GameObject disableLine;

        private bool isMute;


        private void Awake()
        {
            changeSoundVolumeButton.onClick.AddListener(() => 
            {
                ChangeSoundVolume();
                SoundManager.Instance.PlayButtonSound();         
            });
        }
        private void Start()
        {
            isMute = !SoundManager.Instance.IsActive();
            disableLine.SetActive(isMute);
        }

        private void ChangeSoundVolume()
        {
            isMute = !isMute;

            disableLine.SetActive(isMute);

            int soundVolume = isMute ? 0 : 1;
            SoundManager.Instance.SetMusicVolume(soundVolume);
        }
    }
}

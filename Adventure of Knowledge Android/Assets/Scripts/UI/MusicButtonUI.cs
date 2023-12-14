using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdventureOfKnowledge.UI
{
    public class MusicButtonUI:MonoBehaviour
    {
        [SerializeField] private Button changeMusicVolumeButton;

        [SerializeField] private GameObject disableLine;

        private bool isMute;


        private void Awake()
        {
            changeMusicVolumeButton.onClick.AddListener(() => 
            {
                SoundManager.Instance.PlayButtonSound();
                ChangeMusicVolume();
            });
        }

        private void Start()
        {
            isMute = !MusicManager.Instance.IsActive();
            disableLine.SetActive(isMute);
        }

        private void ChangeMusicVolume()
        {
            isMute = !isMute;

            disableLine.SetActive(isMute);

            int musicVolume = isMute ? 0 : 1;
            MusicManager.Instance.SetMusicVolume(musicVolume);
        }
    }
}

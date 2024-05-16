using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager:MonoBehaviour
    {
        private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

        public static MusicManager Instance { get; private set; }

        [SerializeField] private  float defaultMusicVolume = 0.4f;

        private AudioSource musicSource;
        private float clipTime;

        private void Awake()
        {
            Instance = this;
            gameObject.SetActive(false);

            musicSource = GetComponent<AudioSource>();
            musicSource.volume = defaultMusicVolume;

            musicSource.time = PlayerPrefs.GetFloat(musicSource.clip.name);

            bool isActive = PlayerPrefs.GetInt(PLAYER_PREFS_MUSIC_VOLUME, 1) == 1;

            //gameObject.SetActive(isActive);      
        }

        public bool IsActive() => gameObject.activeInHierarchy;

        public void SetMusicVolume(int mute) 
        {
            PlayerPrefs.SetInt(PLAYER_PREFS_MUSIC_VOLUME, mute);

            bool isActive = mute == 1;

            if(isActive)
                musicSource.time = clipTime;
            else 
                clipTime = musicSource.time;

            gameObject.SetActive(isActive);           
        }

        public void SaveClipTime() => PlayerPrefs.SetFloat(musicSource.clip.name, musicSource.time);
    }
}

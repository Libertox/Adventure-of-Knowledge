using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager:MonoBehaviour
    {
        private const string PLAYER_PREFS_SOUND_VOLUME = "SoundVolume";
        private const int DEFAULT_SOUND_VOLUME = 1;

        public static SoundManager Instance { get; private set; }


        [SerializeField] private SoundEffectsClipsSO soundEffectsClips;
        private AudioSource soundSource;
        

        private void Awake()
        {
            Instance = this;

            soundSource = GetComponent<AudioSource>();

            bool isMute = PlayerPrefs.GetInt(PLAYER_PREFS_SOUND_VOLUME, DEFAULT_SOUND_VOLUME) == 0;
            gameObject.SetActive(!isMute);
        }

        public void SetMusicVolume(int mute)
        {
            bool isMute = mute == 0;
            gameObject.SetActive(!isMute);
            PlayerPrefs.SetInt(PLAYER_PREFS_SOUND_VOLUME, mute);
        }

        public bool IsActive() => gameObject.activeInHierarchy;

        private void PlayAudioClip(AudioClip audioClip) 
        {
            if(!IsActive()) return;

            soundSource.PlayOneShot(audioClip);
        }
           
        private void PlayAudioClip(AudioClip[] audioClip)
        {
            if (!IsActive()) return;

            int randomClipIndex = UnityEngine.Random.Range(0, audioClip.Length);
            soundSource.PlayOneShot(audioClip[randomClipIndex]);
        }

        public void PlayButtonSound() => PlayAudioClip(soundEffectsClips.ButtonClips);

        public void PlayBuySound() => PlayAudioClip(soundEffectsClips.BuyClip);

        public void PlayCorrectAnswerSound() => PlayAudioClip(soundEffectsClips.CorrectAnswerClip);

        public void PlayCompleteGameSound() => PlayAudioClip(soundEffectsClips.VictoryClip);

        public void PlayInteractSound() => PlayAudioClip(soundEffectsClips.InteractClip);

        public void PlaySpinningWheelSound() => soundSource.PlayOneShot(soundEffectsClips.SpinningWheelClip);
       
        public void StopSound() => soundSource.Stop();
      
    }
}

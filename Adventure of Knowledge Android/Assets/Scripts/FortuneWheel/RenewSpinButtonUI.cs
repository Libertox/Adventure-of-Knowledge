using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdventureOfKnowledge.FortuneWheel
{
    public class RenewSpinButtonUI:MonoBehaviour
    {

        private CurrentDate currentRenewSpinDate;

        private void Awake()
        {
            SaveManager.LoadRenewSpinTime((callback) =>
            {
                if(callback.Value != null)
                {
                    currentRenewSpinDate = JsonUtility.FromJson<CurrentDate>(callback.Value.ToString());         
                }
                else
                {
                    currentRenewSpinDate = new CurrentDate();
                }

            });

            Button renewButton = GetComponent<Button>();
            renewButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonSound();
                AdsManager.Instance.ShowRewardAds();
                
            });

            SpinTimer.OnCanSpined += SpinTimer_OnCanSpined;

        }

        private void SpinTimer_OnCanSpined(object sender, SpinTimer.OnCanSpinedEventArgs e)
        {
            if(!currentRenewSpinDate.CheckItTheCurrentDate())
                 gameObject.SetActive(!e.canSpined);
        }

        private void Start()
        {
            
            AdsManager.Instance.OnRewardedAdsGet += AdsManager_OnRewarded;

            if(currentRenewSpinDate.CheckItTheCurrentDate())
            {
                gameObject.SetActive(false);
                return;
            }
            FortuneWheelManager.Instance.OnAwardWon += FortuneWheelManager_OnAwardWon;
            //gameObject.SetActive(false);
        }

        private void AdsManager_OnRewarded(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
        }

        private void FortuneWheelManager_OnAwardWon(object sender, FortuneWheelManager.OnAwardWonEventArgs e)
        {
            if(!currentRenewSpinDate.CheckItTheCurrentDate())
                gameObject.SetActive(true);
        }

       
        private void OnDestroy()
        {
            SpinTimer.OnCanSpined -= SpinTimer_OnCanSpined;
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdventureOfKnowledge.FortuneWheel
{
    public class RenewSpinButtonUI:MonoBehaviour
    {
 
        private void Awake()
        {
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
            int year = SaveSystem.LoadRenewSpinDataYear();
            int month = SaveSystem.LoadRenewSpinDataMonth();
            int day = SaveSystem.LoadRenewSpinDataDay();


            if(!CheckItTheCurrentDate(year, month, day))
                 gameObject.SetActive(!e.canSpined);
        }

        private void Start()
        {
            
            AdsManager.Instance.OnRewardedAdsGet += AdsManager_OnRewarded;

            int year = SaveSystem.LoadRenewSpinDataYear();
            int month = SaveSystem.LoadRenewSpinDataMonth();
            int day = SaveSystem.LoadRenewSpinDataDay();

            if(CheckItTheCurrentDate(year, month, day))
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

            int year = SaveSystem.LoadRenewSpinDataYear();
            int month = SaveSystem.LoadRenewSpinDataMonth();
            int day = SaveSystem.LoadRenewSpinDataDay();

            if(!CheckItTheCurrentDate(year, month, day))
                gameObject.SetActive(true);
        }

        private bool CheckItTheCurrentDate(int year, int month, int day) => year == System.DateTime.Now.Year && month == System.DateTime.Now.Month && day == System.DateTime.Now.Day;

        private void OnDestroy()
        {
            SpinTimer.OnCanSpined -= SpinTimer_OnCanSpined;
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge.FortuneWheel
{
    public class FortuneWheelManager:MonoBehaviour
    {
        public static FortuneWheelManager Instance { get;private set; }

        public event EventHandler<OnAwardWonEventArgs> OnAwardWon;

        public class OnAwardWonEventArgs : EventArgs {  public int awardAmount; }

        [SerializeField] private List<FortuneWheelElement> fortuneWheelElements;

        private int awardAmount;

        private PlayerDiamond playerDiamond;

        private void Awake() => Instance = this;

        private void Start() 
        {
            playerDiamond = new PlayerDiamond();
            AdsManager.Instance.OnRewardedAdsGet += AdsManager_OnRewarded;
        }

        private void AdsManager_OnRewarded(object sender, EventArgs e)
        {
            SaveRenewSpinTime();
            SaveManager.ResetSpinTime();
        }

        public void SetAward(int awardIndex)
        {
            awardAmount = GetFortuneWheelElementForIndex(awardIndex).AwardValue;

            CurrentDate currentDate = new CurrentDate(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
            SaveManager.SaveSpinTime(currentDate);

     

            OnAwardWon?.Invoke(this,new OnAwardWonEventArgs { awardAmount = awardAmount });
        }

        public void SaveRenewSpinTime()
        {
            CurrentDate currentDate = new CurrentDate(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
            SaveManager.SaveRenewSpinTime(currentDate);
        }

        public int GetNumberOfWheelElement() => fortuneWheelElements.Count;

        public FortuneWheelElement GetFortuneWheelElementForIndex(int index) => fortuneWheelElements[index];

        public void AddAward() 
        {
            playerDiamond.AddDiamond(awardAmount);
        }
    }
}

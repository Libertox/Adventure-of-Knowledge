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
            playerDiamond = new PlayerDiamond(SaveSystem.LoadDiamondAmount());
            AdsManager.Instance.OnRewardedAdsGet += AdsManager_OnRewarded;
        }

        private void AdsManager_OnRewarded(object sender, EventArgs e)
        {
            SaveRenewSpinTime();
            SaveSystem.ResetSpinTime();
        }

        public void SetAward(int awardIndex)
        {
            awardAmount = GetFortuneWheelElementForIndex(awardIndex).AwardValue;

            SaveSystem.SaveSpinDataYear(DateTime.Now.Year);
            SaveSystem.SaveSpinDataMonth(DateTime.Now.Month);
            SaveSystem.SaveSpinDataDay(DateTime.Now.Day);

            OnAwardWon?.Invoke(this,new OnAwardWonEventArgs { awardAmount = awardAmount });
        }

        public void SaveRenewSpinTime()
        {
            SaveSystem.SaveRenewSpinDataYear(DateTime.Now.Year);
            SaveSystem.SaveRenewSpinDataMonth(DateTime.Now.Month);
            SaveSystem.SaveRenewSpinDataDay(DateTime.Now.Day);
        }

        public int GetNumberOfWheelElement() => fortuneWheelElements.Count;

        public FortuneWheelElement GetFortuneWheelElementForIndex(int index) => fortuneWheelElements[index];

        public void AddAward() 
        {
            playerDiamond.AddDiamond(awardAmount);
            SaveSystem.SaveDiamondAmount(playerDiamond.GetDiamondAmount());
        }
    }
}

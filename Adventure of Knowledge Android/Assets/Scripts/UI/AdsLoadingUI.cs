using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge.UI
{
    public class AdsLoadingUI:MonoBehaviour
    {
        private void Start()
        {
            AdsManager.Instance.OnRewardedAdsWaited += AdsManager_OnRewardedAdsWaited;
            AdsManager.Instance.OnRewardedAdsShow += AdsManager_OnRewardedAdsShow;

            Hide();
        }

        private void AdsManager_OnRewardedAdsShow(object sender, EventArgs e) => Hide();
     

        private void AdsManager_OnRewardedAdsWaited(object sender, EventArgs e) => Show();
     

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);
    }
}

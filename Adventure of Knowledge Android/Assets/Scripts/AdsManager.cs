using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AdventureOfKnowledge
{
    public class AdsManager:MonoBehaviour
    {
        public static AdsManager Instance { get;private set; }

        public event EventHandler OnRewardedAdsShow;
        public event EventHandler OnRewardedAdsWaited;
        public event EventHandler OnRewardedAdsGet;

        private const string appKey = "1cdcce035";

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            IronSourceRewardedVideoEvents.onAdOpenedEvent += IronSourceRewardedVideoEvents_onAdOpenedEvent;
            IronSourceRewardedVideoEvents.onAdRewardedEvent += IronSourceRewardedVideoEvents_onAdRewardedEvent;
            IronSourceEvents.onSdkInitializationCompletedEvent += IronSourceEvents_onSdkInitializationCompletedEvent;
        }

        private void IronSourceRewardedVideoEvents_onAdRewardedEvent(IronSourcePlacement arg1, IronSourceAdInfo arg2)
        {
            OnRewardedAdsGet?.Invoke(this, EventArgs.Empty);
        }

        private void IronSourceEvents_onSdkInitializationCompletedEvent()
        {
            LoadRewardAds();
        }

        private void IronSourceRewardedVideoEvents_onAdOpenedEvent(IronSourceAdInfo obj)
        {
            OnRewardedAdsShow?.Invoke(this, EventArgs.Empty);
        }

        private void Start()
        {
            IronSource.Agent.validateIntegration();
            IronSource.Agent.init(appKey);
        }

        public void LoadRewardAds()
        {
            if(!IronSource.Agent.isRewardedVideoAvailable())
                IronSource.Agent.loadRewardedVideo();
        }

        public void ShowRewardAds()
        {
            while (!IronSource.Agent.isRewardedVideoAvailable())
            {
                OnRewardedAdsWaited?.Invoke(this, EventArgs.Empty);
            }
            IronSource.Agent.showRewardedVideo();
         
        }

        private void OnDestroy()
        {
            IronSourceRewardedVideoEvents.onAdOpenedEvent -= IronSourceRewardedVideoEvents_onAdOpenedEvent;
            IronSourceRewardedVideoEvents.onAdRewardedEvent -= IronSourceRewardedVideoEvents_onAdRewardedEvent;
            IronSourceEvents.onSdkInitializationCompletedEvent -= IronSourceEvents_onSdkInitializationCompletedEvent;
        }


        private void OnApplicationPause(bool isPaused)
        {
            IronSource.Agent.onApplicationPause(isPaused);
        }

    }
}

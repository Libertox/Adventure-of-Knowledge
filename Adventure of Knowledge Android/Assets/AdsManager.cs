using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    string appKey = "1cdcce035";

    private void Start()
    {
        IronSource.Agent.validateIntegration();
        IronSource.Agent.init(appKey);
    }

    public void LoadRewardAds()
    {
        IronSource.Agent.loadRewardedVideo();
    }

    public void ShowRewardAds()
    {
        if(IronSource.Agent.isRewardedVideoAvailable())
            IronSource.Agent.showRewardedVideo();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAds
{
    public void ShowBannerAd();
    public void HideBannerAd();
    public void ShowRewardedAd();
    public void ShowInterstitialAd();
    public void ShowMRECAd();
    public void HideMRECAd();
    public bool IsRewardedAdAvailable();
    public bool IsInterstitialAdAvailable();

}
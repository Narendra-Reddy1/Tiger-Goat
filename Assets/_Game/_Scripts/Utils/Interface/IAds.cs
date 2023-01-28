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
    public void LoadAppOpenAd() { }
    public void ShowAppOpenAd();
    public bool IsAppOpenAdAvailable();
    public bool IsRewardedAdAvailable();
    public bool IsInterstitialAdAvailable();

}
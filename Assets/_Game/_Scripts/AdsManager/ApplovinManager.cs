using SovereignStudios;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AdState
{
    LOADED,
    FAILED_TO_LOAD,
    DISPLAYED,
    FAILED_TO_DISPLAY,
    REWARD_RECEIVED,
    REVENUE_PAID,
    AD_CLICKED,
    DISMISSED,
}

public class AdEventData
{
    public AdState adState;
    public double revenue;
    public string networkName;
    public string adFormat;
    public MaxSdkBase.ErrorInfo errorInfo;
    public AdEventData(AdState adState, double revenue = 0, string networkName = "", string adFormat = "", MaxSdkBase.ErrorInfo errorInfo = null)
    {
        this.adState = adState;
        this.revenue = revenue;
        this.networkName = networkName;
        this.adFormat = adFormat;
        this.errorInfo = errorInfo;
    }
}
public class ApplovinManager : IInitializer, IAds
{
    [SerializeField] private AdUnitIds adUnitIds;
    private int retryAttempt;
    private bool isBannerAdLoaded = false;
    private bool isMRECAdLoaded = false;

    public ApplovinManager(AdUnitIds adUnit)
    {
        adUnitIds = adUnit;
        Init();
    }

    public void Init()
    {
        InitializeBannerAds();
        InitializeInterstitialAds();
        InitializeMRecAds();
        InitializeRewardedAds();
    }

    #region BannerAds
    public void InitializeBannerAds()
    {
        // Banners are automatically sized to 320×50 on phones and 728×90 on tablets
        // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
        MaxSdk.CreateBanner(adUnitIds.BannerAdId, MaxSdkBase.BannerPosition.BottomCenter);

        // Set background or background color for banners to be fully functional
        MaxSdk.SetBannerBackgroundColor(adUnitIds.BannerAdId, Color.black);

        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
        MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;
    }

    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        isBannerAdLoaded = true;
    }

    private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) { }

    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }
    #endregion BannerAds

    #region Interstitial Ads

    public void InitializeInterstitialAds()
    {
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

        // Load the first interstitial
        LoadInterstitial();
    }

    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(adUnitIds.InterstitialAdId);
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

        // Reset retry attempt
        retryAttempt = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        retryAttempt++;
        float retryDelay = Mathf.Pow(2, Mathf.Min(6, retryAttempt));
        SovereignUtils.DelayedCallback(retryDelay, LoadInterstitial);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.
        LoadInterstitial();
    }
    #endregion Interstitial Ads

    #region Rewarded Ads

    public void InitializeRewardedAds()
    {
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first rewarded ad
        LoadRewardedAd();
    }

    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(adUnitIds.RewardedAdId);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.

        // Reset retry attempt
        retryAttempt = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        retryAttempt++;
        float retryDelay = Mathf.Pow(2, Mathf.Min(6, retryAttempt));

        SovereignUtils.DelayedCallback(retryDelay, LoadRewardedAd);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        LoadRewardedAd();
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        LoadRewardedAd();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // The rewarded ad displayed and the user should receive the reward.
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
    }
    #endregion Rewarded Ads

    #region MREC Ads
    public void InitializeMRecAds()
    {
        // MRECs are sized to 300x250 on phones and tablets
        MaxSdk.CreateMRec(adUnitIds.MRECAdId, MaxSdkBase.AdViewPosition.Centered);

        MaxSdkCallbacks.MRec.OnAdLoadedEvent += OnMRecAdLoadedEvent;
        MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += OnMRecAdLoadFailedEvent;
        MaxSdkCallbacks.MRec.OnAdClickedEvent += OnMRecAdClickedEvent;
        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnMRecAdRevenuePaidEvent;
        MaxSdkCallbacks.MRec.OnAdExpandedEvent += OnMRecAdExpandedEvent;
        MaxSdkCallbacks.MRec.OnAdCollapsedEvent += OnMRecAdCollapsedEvent;
    }

    public void OnMRecAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        isMRECAdLoaded = true;
    }

    public void OnMRecAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo error) { }

    public void OnMRecAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnMRecAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnMRecAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnMRecAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }
    #endregion MREC Ads
    public void ShowBannerAd()
    {
        if (isBannerAdLoaded)
            MaxSdk.ShowBanner(adUnitIds.BannerAdId);
        else
            MaxSdk.LoadBanner(adUnitIds.BannerAdId);
    }
    public void HideBannerAd()
    {
        MaxSdk.HideBanner(adUnitIds.BannerAdId);
    }
    public void ShowRewardedAd()
    {
        if (MaxSdk.IsRewardedAdReady(adUnitIds.RewardedAdId))
            MaxSdk.ShowRewardedAd(adUnitIds.RewardedAdId);
        else
            MaxSdk.LoadRewardedAd(adUnitIds.RewardedAdId);
    }

    public void ShowInterstitialAd()
    {
        if (MaxSdk.IsInterstitialReady(adUnitIds.InterstitialAdId))
            MaxSdk.ShowInterstitial(adUnitIds.InterstitialAdId);
        else
            MaxSdk.LoadInterstitial(adUnitIds.InterstitialAdId);
    }

    public void ShowMRECAd()
    {
        if (isMRECAdLoaded)
            MaxSdk.ShowMRec(adUnitIds.MRECAdId);
        else
            MaxSdk.LoadMRec(adUnitIds.MRECAdId);
    }
    public void HideMRECAd()
    {
        MaxSdk.HideMRec(adUnitIds.MRECAdId);
    }
    public bool IsRewardedAdAvailable()
    {
        return MaxSdk.IsRewardedAdReady(adUnitIds.RewardedAdId);
    }
    public bool IsInterstitialAdAvailable()
    {
        return MaxSdk.IsInterstitialReady(adUnitIds.InterstitialAdId);
    }
}

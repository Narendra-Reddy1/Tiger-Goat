using SovereignStudios.Utils;
using SovereignStudios.EventSystem;
using UnityEngine;
using System;

public enum AdState
{
    //Banner Ads
    BANNER_AD_LOADED,
    BANNER_AD_FAILED_TO_LOAD,
    BANNER_AD_DISPLAYED,
    BANNER_AD_FAILED_TO_DISPLAY,
    BANNER_AD_REVENUE_PAID,
    BANNER_AD_CLICKED,

    //Interstitial Ads
    INTERSTITIAL_LOADED,
    INTERSTITIAL_FAILED_TO_LOAD,
    INTERSTITIAL_DISPLAYED,
    INTERSTITIAL_FAILED_TO_DISPLAY,
    INTERSTITIAL_REVENUE_PAID,
    INTERSTITIAL_AD_CLICKED,
    INTERSTITIAL_DISMISSED,
    //Rewarded Ads
    REWARDED_LOADED,
    REWARDED_FAILED_TO_LOAD,
    REWARDED_DISPLAYED,
    REWARDED_FAILED_TO_DISPLAY,
    REWARDED_REWARD_RECEIVED,
    REWARDED_REVENUE_PAID,
    REWARDED_AD_CLICKED,
    REWARDED_DISMISSED,
    //Mrec Ads
    MREC_LOADED,
    MREC_FAILED_TO_LOAD,
    MREC_DISPLAYED,
    MREC_FAILED_TO_DISPLAY,
    MREC_REVENUE_PAID,
    MREC_AD_CLICKED,
    //App Open Ads
    APP_OPEN_LOADED,
    APP_OPEN_FAILED_TO_LOAD,
    APP_OPEN_DISPLAYED,
    APP_OPEN_FAILED_TO_DISPLAY,
    APP_OPEN_REVENUE_PAID,
    APP_OPEN_AD_CLICKED,
    APP_OPEN_AD_DISMISSED,

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
        InitializeAppOpenAd();
    }

    #region BannerAds
    public void InitializeBannerAds()
    {
        // Banners are automatically sized to 320×50 on phones and 728×90 on tablets
        // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
        MaxSdk.CreateBanner(adUnitIds.BannerAdId, MaxSdkBase.BannerPosition.BottomCenter);

        // Set background or background color for banners to be fully functional
        MaxSdk.SetBannerBackgroundColor(adUnitIds.BannerAdId, Color.black);
        MaxSdk.LoadBanner(adUnitIds.BannerAdId);
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
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.BANNER_AD_LOADED, networkName: adInfo.NetworkName, adFormat: adInfo.AdFormat));
        SovereignUtils.Log($"On Banner AdLoadeed");
    }

    private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.BANNER_AD_FAILED_TO_LOAD, errorInfo: errorInfo));
        SovereignUtils.Log($"Banner ad load faiuled: {errorInfo}");
    }

    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.BANNER_AD_CLICKED, adInfo.Revenue, adInfo.NetworkName, adInfo.AdFormat)); ;
    }

    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.BANNER_AD_REVENUE_PAID, adInfo.Revenue, adInfo.NetworkName, adInfo.AdFormat));
    }

    private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }
    #endregion BannerAds

    #region App Open
    private void InitializeAppOpenAd()
    {
        MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += OnAppOpenAdLoadedEvent;
        MaxSdkCallbacks.AppOpen.OnAdDisplayedEvent += OnAppOpenAdDisplayedEvent;
        MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent += OnAppOpenAdLoadFailedEvent;
        MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += OnAppOpenAdRevenuePaidEvent;
        MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenAdDismissedEvent;
        LoadAppOpenAd();
    }

    private void OnAppOpenAdDisplayedEvent(string adId, MaxSdkBase.AdInfo adInfo)
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.APP_OPEN_DISPLAYED, adInfo.Revenue, adInfo.NetworkName, adInfo.AdFormat));
    }

    private void OnAppOpenAdLoadedEvent(string adId, MaxSdkBase.AdInfo adInfo)
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.APP_OPEN_LOADED, networkName: adInfo.NetworkName, adFormat: adInfo.AdFormat));
    }
    private void OnAppOpenAdLoadFailedEvent(string adId, MaxSdkBase.ErrorInfo errorInfo)
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.APP_OPEN_LOADED, errorInfo: errorInfo));
    }
    private void OnAppOpenAdRevenuePaidEvent(string adId, MaxSdkBase.AdInfo adInfo)
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.APP_OPEN_REVENUE_PAID, adInfo.Revenue, adInfo.NetworkName, adInfo.AdFormat));
    }
    private void OnAppOpenAdDismissedEvent(string adId, MaxSdkBase.AdInfo adInfo)
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.APP_OPEN_AD_DISMISSED, adInfo.Revenue, adInfo.NetworkName, adInfo.AdFormat));
    }
    #endregion  App Open

    #region Interstitial Ads

    public void InitializeInterstitialAds()
    {
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaidEvent;

        // Load the first interstitial
        LoadInterstitial();
    }


    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(adUnitIds.InterstitialAdId);
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        retryAttempt = 0;
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.INTERSTITIAL_LOADED, networkName: adInfo.NetworkName, adFormat: adInfo.AdFormat));
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        retryAttempt++;
        float retryDelay = Mathf.Pow(2, Mathf.Min(6, retryAttempt));
        SovereignUtils.DelayedCallback(retryDelay, LoadInterstitial);
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.INTERSTITIAL_FAILED_TO_LOAD, errorInfo: errorInfo));
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.INTERSTITIAL_DISPLAYED, adInfo.Revenue, networkName: adInfo.NetworkName, adFormat: adInfo.AdFormat));
    }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.INTERSTITIAL_FAILED_TO_DISPLAY, errorInfo: errorInfo));
        LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.INTERSTITIAL_AD_CLICKED, adInfo.Revenue, networkName: adInfo.NetworkName, adFormat: adInfo.AdFormat));
    }
    private void OnInterstitialRevenuePaidEvent(string adId, MaxSdkBase.AdInfo adInfo)
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.INTERSTITIAL_REVENUE_PAID, adInfo.Revenue, networkName: adInfo.NetworkName, adFormat: adInfo.AdFormat));
    }
    private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.INTERSTITIAL_DISMISSED, adInfo.Revenue, networkName: adInfo.NetworkName, adFormat: adInfo.AdFormat));
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
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.REWARDED_LOADED, networkName: adInfo.NetworkName, adFormat: adInfo.AdFormat));

    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).
        retryAttempt++;
        float retryDelay = Mathf.Pow(2, Mathf.Min(6, retryAttempt));
        SovereignUtils.DelayedCallback(retryDelay, LoadRewardedAd);
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.REWARDED_FAILED_TO_LOAD, errorInfo: errorInfo));
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.REWARDED_DISPLAYED, adInfo.Revenue, networkName: adInfo.NetworkName, adFormat: adInfo.AdFormat));
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        LoadRewardedAd();
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.REWARDED_FAILED_TO_DISPLAY, errorInfo: errorInfo));
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.REWARDED_AD_CLICKED, adInfo.Revenue, networkName: adInfo.NetworkName, adFormat: adInfo.AdFormat));
    }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        LoadRewardedAd();
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.REWARDED_DISMISSED, adInfo.Revenue, adInfo.NetworkName, adInfo.AdFormat));
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // The rewarded ad displayed and the user should receive the reward.
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.REWARDED_REWARD_RECEIVED, adInfo.Revenue, networkName: adInfo.NetworkName, adFormat: adInfo.AdFormat));
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.REWARDED_REVENUE_PAID, adInfo.Revenue, networkName: adInfo.NetworkName, adFormat: adInfo.AdFormat));
    }
    #endregion Rewarded Ads

    #region MREC Ads
    public void InitializeMRecAds()
    {
        // MRECs are sized to 300x250 on phones and tablets
        MaxSdk.CreateMRec(adUnitIds.MRECAdId, MaxSdkBase.AdViewPosition.Centered);
        LoadMRECAd();
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
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.MREC_LOADED, networkName: adInfo.NetworkName, adFormat: adInfo.AdFormat));
    }

    public void OnMRecAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo error)
    {
        isMRECAdLoaded = false;
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.MREC_FAILED_TO_LOAD, errorInfo: error));
    }

    public void OnMRecAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.MREC_AD_CLICKED, adInfo.Revenue, networkName: adInfo.NetworkName, adFormat: adInfo.AdFormat));
    }

    public void OnMRecAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AD_STATE_CHANGED, new AdEventData(AdState.MREC_REVENUE_PAID, adInfo.Revenue, networkName: adInfo.NetworkName, adFormat: adInfo.AdFormat));
    }

    public void OnMRecAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnMRecAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }
    #endregion MREC Ads

    #region IAds 
    public void ShowBannerAd()
    {
        SovereignUtils.Log($"Inside applovin.showbannerad");
        if (isBannerAdLoaded)
        {
            SovereignUtils.Log($"Inside applovin.showbannerad if statement");
            MaxSdk.ShowBanner(adUnitIds.BannerAdId);
        }
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
        {
            SovereignUtils.Log($"Mrec showing...");
            MaxSdk.ShowMRec(adUnitIds.MRECAdId);
        }
        else
        {
            SovereignUtils.Log($"Mrec not loaded reloading...");
            LoadMRECAd();
        }
    }
    public void LoadMRECAd()
    {
        if (isMRECAdLoaded) return;
        MaxSdk.LoadMRec(adUnitIds.MRECAdId);
    }
    public void HideMRECAd()
    {
        MaxSdk.HideMRec(adUnitIds.MRECAdId);
    }
    public void LoadAppOpenAd()
    {
        if (IsAppOpenAdAvailable())
            return;
        MaxSdk.LoadAppOpenAd(adUnitIds.AppOpenId);
    }
    public void ShowAppOpenAd()
    {
        if (IsAppOpenAdAvailable())
            MaxSdk.ShowAppOpenAd(adUnitIds.AppOpenId);
        else
            MaxSdk.LoadAppOpenAd(adUnitIds.AppOpenId);

    }
    public bool IsAppOpenAdAvailable()
    {
        return MaxSdk.IsAppOpenAdReady(adUnitIds.AppOpenId);
    }
    public bool IsRewardedAdAvailable()
    {
        return MaxSdk.IsRewardedAdReady(adUnitIds.RewardedAdId);
    }
    public bool IsInterstitialAdAvailable()
    {
        return MaxSdk.IsInterstitialReady(adUnitIds.InterstitialAdId);
    }
    #endregion IAds
}

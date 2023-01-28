using UnityEngine;
using SovereignStudios.EventSystem;
using SovereignStudios.Utils;

public class GenericAdsManager : MonoBehaviour, IInitializer
{
    #region Variables

    [SerializeField] private AdUnitIds adUnitIds;
    private IAds AdsManager;

    #endregion Variables

    #region Unity Methods
    private void Awake()
    {
        Init();

    }
    private void OnEnable()
    {
        GlobalEventHandler.AddListener(EventID.EVENT_ON_SHOW_BANNER_AD_REQUESTED, Callback_On_ShowBannerAd_Requested);
        GlobalEventHandler.AddListener(EventID.EVENT_ON_HIDE_BANNER_AD_REQUESTED, Callback_On_HideBannerAd_Requested);
        GlobalEventHandler.AddListener(EventID.EVENT_ON_SHOW_MREC_AD_REQUESTED, Callback_On_ShowMRECAd_Requested);
        GlobalEventHandler.AddListener(EventID.EVENT_ON_HIDE_MREC_AD_REQUESTED, Callback_On_HideMRECAd_Requested);
        GlobalEventHandler.AddListener(EventID.EVENT_ON_SHOW_INTERSTITIAL_AD_REQUESTED, Callback_On_ShowInterstitialAd_Requested);
        GlobalEventHandler.AddListener(EventID.EVENT_ON_SHOW_REWARDED_AD_REQUESTED, Callback_On_ShowRewardedAd_Requested);
        GlobalEventHandler.AddListener(EventID.EVENT_ON_LOAD_APP_OPEN_AD_REQUESTED, Callback_On_Load_App_Open_Ad_Requested);
        GlobalEventHandler.AddListener(EventID.EVENT_ON_SHOW_APP_OPEN_AD_REQUESTED, Callback_On_Show_AppOpenAd_Requested);
        GlobalEventHandler.AddListener(EventID.EVENT_ON_APP_OPEN_AD_AVAILABILITTY_REQUESTED, Callback_On_AppOpenAd_Availability_Requested);
        GlobalEventHandler.AddListener(EventID.EVENT_ON_INTERSTITIAL_AD_AVAILABILITY_REQUESTED, Callback_On_InterstitialAd_Availability_Requested);
        GlobalEventHandler.AddListener(EventID.EVENT_ON_REWARDED_AD_AVAILABILITY_REQUESTED, Callback_On_RewardedAd_Availability_Requested);
    }
    private void OnDisable()
    {
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SHOW_BANNER_AD_REQUESTED, Callback_On_ShowBannerAd_Requested);
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_HIDE_BANNER_AD_REQUESTED, Callback_On_HideBannerAd_Requested);
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SHOW_MREC_AD_REQUESTED, Callback_On_ShowMRECAd_Requested);
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_HIDE_MREC_AD_REQUESTED, Callback_On_HideMRECAd_Requested);
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SHOW_INTERSTITIAL_AD_REQUESTED, Callback_On_ShowInterstitialAd_Requested);
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SHOW_REWARDED_AD_REQUESTED, Callback_On_ShowRewardedAd_Requested);
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_LOAD_APP_OPEN_AD_REQUESTED, Callback_On_Load_App_Open_Ad_Requested);
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SHOW_APP_OPEN_AD_REQUESTED, Callback_On_Show_AppOpenAd_Requested);
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_APP_OPEN_AD_AVAILABILITTY_REQUESTED, Callback_On_AppOpenAd_Availability_Requested);
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_INTERSTITIAL_AD_AVAILABILITY_REQUESTED, Callback_On_InterstitialAd_Availability_Requested);
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_REWARDED_AD_AVAILABILITY_REQUESTED, Callback_On_RewardedAd_Availability_Requested);
    }

    #endregion Unity Methods

    public void Init()
    {
        AdsManager = new ApplovinManager(adUnitIds);
    }

    #region Banner Ads
    private void Callback_On_ShowBannerAd_Requested(object args)
    {
        AdsManager.ShowBannerAd();
        SovereignUtils.Log($" Banner ad show callback");
    }
    private void Callback_On_HideBannerAd_Requested(object args)
    {
        AdsManager.HideBannerAd();
    }
    #endregion Banner Ads

    #region Interstitial Ads
    private void Callback_On_ShowInterstitialAd_Requested(object args)
    {
        AdsManager.ShowInterstitialAd();
    }
    private object Callback_On_InterstitialAd_Availability_Requested(object args)
    {
        return AdsManager.IsInterstitialAdAvailable();
    }
    #endregion Interstitial Ads

    #region Rewarded Ads
    private void Callback_On_ShowRewardedAd_Requested(object args)
    {
        AdsManager.ShowRewardedAd();
    }
    private object Callback_On_RewardedAd_Availability_Requested(object args)
    {
        return AdsManager.IsRewardedAdAvailable();
    }
    #endregion Rewarded Ads

    #region MREC Ads
    private void Callback_On_ShowMRECAd_Requested(object args)
    {
        AdsManager.ShowMRECAd();
    }
    private void Callback_On_HideMRECAd_Requested(object args)
    {
        AdsManager.HideMRECAd();
    }
    #endregion MREC Ads

    #region App Open Ads
    private void Callback_On_Load_App_Open_Ad_Requested(object args)
    {
        AdsManager.LoadAppOpenAd();
    }
    private void Callback_On_Show_AppOpenAd_Requested(object args)
    {
        AdsManager.ShowAppOpenAd();
    }
    private object Callback_On_AppOpenAd_Availability_Requested(object args)
    {
        return AdsManager.IsAppOpenAdAvailable();
    }
    #endregion App Open Ads

    #region Debug 
    [DebugButton("ShowBanner")]
    public void DebugShowBannerAd()
    {
        Callback_On_ShowBannerAd_Requested(null);
    }

    [DebugButton("HideBanner")]
    public void DebugHideBannerAd()
    {
        Callback_On_HideBannerAd_Requested(null);
    }

    [DebugButton("ShowMRec")]
    public void DebugShowMrec()
    {
        Callback_On_ShowMRECAd_Requested(null);
    }

    [DebugButton("HideMRec")]
    public void DebugHideMRec()
    {
        Callback_On_HideMRECAd_Requested(null);
    }

    [DebugButton("ShowInter")]
    public void DebugShowInter()
    {
        Callback_On_ShowInterstitialAd_Requested(null);
    }

    [DebugButton("ShowRewarded")]
    public void DebugShowRewarded()
    {
        Callback_On_ShowRewardedAd_Requested(null);
    }
    [DebugButton("LoadAppOpen")]
    public void DebugLoadAppOpenAd()
    {
        Callback_On_Load_App_Open_Ad_Requested(null);
    }
    [DebugButton("ShowAppOpen")]
    public void DebugShowAppOpenAd()
    {
        Callback_On_Show_AppOpenAd_Requested(null);
    }
    #endregion Debug
}
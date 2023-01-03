using SovereignStudios;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAdsManager : MonoBehaviour, IInitializer
{
    [SerializeField] private AdUnitIds adUnitIds;
    private IAds AdsManager;


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

}
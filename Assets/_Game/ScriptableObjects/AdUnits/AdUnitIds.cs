using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new AdUnitIds", menuName = "ScriptableObjects/AdUnits")]
public class AdUnitIds : BaseScriptlableObject
{
    [Space(10)]
    [SerializeField] private string bannerAdId;
    [SerializeField] private string interstitialAdId;
    [SerializeField] private string rewardedAdId;
    [SerializeField] private string mrecAdId;

    public string BannerAdId => bannerAdId;
    public string InterstitialAdId => interstitialAdId;
    public string RewardedAdId => rewardedAdId;
    public string MRECAdId => mrecAdId;
}


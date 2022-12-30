//using Facebook.Unity;
//using Facebook.Unity.Settings;
//using mixpanel;
using System.Xml;
using UnityEngine;
[CreateAssetMenu(fileName = "ThirdPartySdkKeys", menuName = "ScriptableObjects/ThirdPartySdkKeys", order = 1)]
public class ThirdPartySdkKeys : ScriptableObject
{

    //public MixpanelSettings mixpanelSettings;
    //public AppsFlyerObjectScript AppsFlyerObject;
    [Header("Analytics")]
    [Space(2)]
    public MixPanelSetting[] mixPanelSettingsList;
    public string applovinSDKKey;

    //public void SetUpBranchData(ProjectBranch projectBranch)
    //{
    //    switch (projectBranch)
    //    {
    //        case ProjectBranch.DEVELOPMENT_BUILD:
    //            SetupDevelopmentData();
    //            break;
    //        case ProjectBranch.UPLOAD_BUILD:
    //            SetupProductionData();
    //            break;
    //        default:
    //            SetupDevelopmentData();
    //            break;
    //    }
    //}



    //public ref TextAsset GetGoolgeServiceAsset(ProjectBranch projectBranch)
    //{
    //    if (projectBranch ==ProjectBranch.UPLOAD_BUILD)
    //    {
    //        return ref goolgeService_Dev;
    //    }
    //    else
    //    {
    //        return ref goolgeService_Upload;
    //    }
    //}
    //public  string GetIronSourceApiKey()
    //{
    //#if UNITY_ANDROID
    //    return ironSourceMediationSettings.AndroidAppKey;
    //#elif UNITY_IOS
    //        return ironSourceMediationSettings.IOSAppKey;
    //#endif
    //}
    //public void AppsFlyerDebugSettings(ProjectBranch projectBranch)
    //{
    //    if (projectBranch == ProjectBranch.UPLOAD_BUILD)
    //    {
    //        AppsFlyerObject.isDebug = false;
    //    }
    //    else
    //    {
    //        AppsFlyerObject.isDebug = true;
    //    }
    //}

    //public ref MixPanelSetting GetCurrentMixpanelSetting(ProjectBranch projectBranch)
    //{
    //    if (mixPanelSettingsList.Length > (int)projectBranch)
    //    {
    //        return ref mixPanelSettingsList[(int)projectBranch];
    //    }
    //    else
    //    {
    //        return ref mixPanelSettingsList[(int)ProjectBranch.DEVELOPMENT_BUILD];
    //    }

    //}
    //public void UpdateMixpanelSettings(ProjectBranch projectBranch)
    //{

    //    mixpanelSettings.RuntimeToken= GetCurrentMixpanelSetting(projectBranch).RunTimeToken;
    //    mixpanelSettings.DebugToken = GetCurrentMixpanelSetting(projectBranch).RunTimeToken;
    //}
    //public void UpdateIronSourceSettings()
    //{
    //    //#if UNITY_ANDROID
    //    //    ironSourceMediationSettings.AndroidAppKey = IronSourceAndroidApiKey;
    //    //#elif UNITY_IOS
    //    //    ironSourceMediationSettings.IOSAppKey = IronSourceIosApiKey;
    //    //#endif
    //}

    //private void SetupDevelopmentData()
    //{
    //    //branchData.testMode = true;
    //    //branchData.testBranchKey = testBranchKey;
    //    //branchData.testBranchUri = testBranchUri;
    //    //branchData.testAppLinks = new string[2];
    //    //branchData.testAppLinks[0] = testBranchAppLink1;
    //    //branchData.testAppLinks[1] = testBranchAppLink2;
    //}

    //private void SetupProductionData()
    //{
    //    //branchData.testMode = false;
    //    //branchData.liveBranchKey = liveBranchKey;
    //    //branchData.liveBranchUri = liveBranchUri;
    //    //branchData.liveAppLinks = new string[2];
    //    //branchData.liveAppLinks[0] = liveBranchAppLink1;
    //    //branchData.liveAppLinks[1] = liveBranchAppLink2;
    //}

}
[System.Serializable]
public class MixPanelSetting
{
    public string name;
    public string RunTimeToken;
    public string DebugToken;
}

//using Facebook.Unity.Settings;
using SovereignStudios.Utils;
using System;
using UnityEngine;

public class ProjectSetting
{

    #region PROJECT SETTING 
    public static ProjectBranch CurrentProjectBranch = ProjectBranch.DEVELOPMENT_BUILD;
    public static bool enableReporter;
    public static bool enableDebugDefine;
    public static bool enableLogWrite;
    public static int BundleVersionCode;
    private static bool isInitialized;
    private static string IornSourceApiKey;
    private static string FaceBookAppId;
    private static string MixPaneRunTimeToken;
    private static string MixPaneDebugToken;

    #endregion

    #region DO NOT MODIFY THIS REGION
    public static string GetIornSourceApiKey()
    {
        return IornSourceApiKey;
    }
    public static string GetFaceBookAppId()
    {
        return FaceBookAppId;
    }
    public static string GetMixPaneRunTimeToken()
    {
        return MixPaneRunTimeToken;
    }
    public static string GetMixPaneDebugToken()
    {
        return MixPaneDebugToken;
    }

    public static ref bool IsProjectSettingInitialized()
    {
        return ref isInitialized;
    }

    #endregion

    #region All Setup for master Build For Final Build
    public static void EnableCoreProjectSetting(bool status)
    {
#if UPLOAD_BUILD
        Debug.unityLogger.logEnabled = false;
#else
        Debug.unityLogger.logEnabled = status;
#endif
    }



    /// <summary>
    /// This will intialize all project related Setting
    /// </summary>
    internal static void InitializeProjectSetting(ProjectAssetManager projectAssetManager)
    {

        enableReporter = projectAssetManager.projectSettingAssets.ENABLE_REPORTER;
        enableDebugDefine = projectAssetManager.projectSettingAssets.ENABLE_DEBUG_DEFINE;
        enableLogWrite = projectAssetManager.projectSettingAssets.ENABLE_LOG_WRITE;
        CurrentProjectBranch = projectAssetManager.projectSettingAssets.CurrentProjectBranch;
        BundleVersionCode = projectAssetManager.projectSettingAssets.GetBundleVersionCode();
        //  DeftouchUtils.InitUtilityManager(projectAssetManager);
        SovereignUtils.Log("BundleVersionCode" + BundleVersionCode);

        EnableCoreProjectSetting(enableReporter);
        int branchIndex = (int)CurrentProjectBranch;
        if (branchIndex > (int)ProjectBranch.UPLOAD_BUILD)
        {
            branchIndex = (int)ProjectBranch.DEVELOPMENT_BUILD;
        }
        //using new Server configuration
        //GatewayServer = projectAssetManager.serverInformations.GatewayServer[branchIndex];
        //AnalyticsServer = projectAssetManager.serverInformations.AnalyticsServer[branchIndex];
        //InfoServer = projectAssetManager.serverInformations.InfoServer[branchIndex];
        //DeftouchUtils.Log("ProjectSetting.GetgatewayServerInfo().GetAddressName()" + GetgatewayServerInfo().GetAddressName());
        //DeftouchUtils.Log("ProjectSetting.GetgatewayServerInfo().Port()" + GetgatewayServerInfo().port);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        ///THIRD PARTY SKEY KEY SETTINGS
        //FaceBookAppId = FacebookSettings.AppId;
        // MixPaneDebugToken = projectAssetManager.projectSettingAssets.deftThirdPartySdkKeys.mixpanelSettings.DebugToken;
        // MixPaneRunTimeToken = projectAssetManager.projectSettingAssets.deftThirdPartySdkKeys.mixpanelSettings.RuntimeToken;
        //IornSourceApiKey = projectAssetManager.projectSettingAssets.deftThirdPartySdkKeys.GetIronSourceApiKey();
        //SmartLookApi_Key = projectAssetManager.projectSettingAssets.thirdPartySdkKeys.SMART_LOOK_API_KEY;

        SovereignUtils.Log("Device refresh rate: " + Screen.currentResolution.refreshRate);
        Application.targetFrameRate = Math.Min(60, Screen.currentResolution.refreshRate);

        isInitialized = true;
    }

    #endregion

}

//using mixpanel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Project Settings", menuName = "ScriptableObjects/ProjectSettings")]
[System.Serializable]
public class ProjectSettingAssets : ScriptableObject
{

    #region PROJECT SETTING 
    public ProjectBranch CurrentProjectBranch = ProjectBranch.DEVELOPMENT_BUILD;
    public bool ENABLE_REPORTER;
    public bool ENABLE_DEBUG_DEFINE;
    public bool ENABLE_LOG_WRITE;
    public bool ENABLE_GAMEPLAY_TESTING;
    public bool APP_BUNDLE_BUILD;
    public bool EditEnablePlayerSetting;
    public bool ViewThirdPartySdkKeys;

    public bool ViewPlayerSetting;
    public string BundleVersionCode;
    public string Version;
    public string PackageName;
    public ThirdPartySdkKeys thirdPartySdkKeys;
    //public MixpanelSettings mixpanelSettings;

    public string GetVersion()
    {
        return Version;
    }
    public int GetBundleVersionCode()
    {
        return int.Parse(BundleVersionCode);
    }
    public static List<string> predefinedSymbolList = new List<string> { "DEVLOPMENT_BUILD", "STAGING_BUILD",
        "MASTER_BUILD","UPLOAD_BUILD","ENABLE_REPORTER","ENABLE_LOG_WRITE","ENABLE_GAMEPLAY_TESTING", "ENABLE_GOOGLE_PLAY_SIGN_IN_TESTING","DEBUG_DEFINE"};
    public static List<string> GetDefineSymoblList(ref ProjectSettingAssets projectSettingAssets)
    {
        List<string> SymbolList = new List<string>();
        switch (projectSettingAssets.CurrentProjectBranch)
        {
            case ProjectBranch.DEVELOPMENT_BUILD:
                SymbolList.Add("DEVLOPMENT_BUILD");
                break;
            //case ProjectBranch.STAGING_BUILD:
            //    SymbolList.Add("STAGING_BUILD");
            //    break;
            //case ProjectBranch.MASTER_BUILD:
            //    SymbolList.Add("MASTER_BUILD");
            //    break;
            case ProjectBranch.UPLOAD_BUILD:
                SymbolList.Add("UPLOAD_BUILD");
                break;
            default:
                SymbolList.Add("DEVLOPMENT_BUILD");
                break;
        }
        if (projectSettingAssets.CurrentProjectBranch != ProjectBranch.UPLOAD_BUILD)
        {
            if (projectSettingAssets.ENABLE_REPORTER)
            {
                SymbolList.Add("ENABLE_REPORTER");
            }
            if (projectSettingAssets.ENABLE_DEBUG_DEFINE)
            {
                SymbolList.Add("DEBUG_DEFINE");
            }
            if (projectSettingAssets.ENABLE_LOG_WRITE)
            {
                SymbolList.Add("ENABLE_LOG_WRITE");
            }
            if (projectSettingAssets.ENABLE_GAMEPLAY_TESTING)
            {
                SymbolList.Add("ENABLE_GAMEPLAY_TESTING");
            }
            //if (projectSettingAssets.ENABLE_GOOGLE_PLAY_SIGN_IN_TESTING)
            //{
            //    SymbolList.Add("ENABLE_GOOGLE_PLAY_SIGN_IN_TESTING");
            //}
        }
        else
        {
            projectSettingAssets.ENABLE_REPORTER = false;
            projectSettingAssets.ENABLE_DEBUG_DEFINE = false;
            projectSettingAssets.ENABLE_LOG_WRITE = false;
            projectSettingAssets.ENABLE_GAMEPLAY_TESTING = false;
        }
        return SymbolList;
    }



    #endregion
}
public enum ProjectBranch
{
    DEVELOPMENT_BUILD = 0,
    UPLOAD_BUILD = 1,
}

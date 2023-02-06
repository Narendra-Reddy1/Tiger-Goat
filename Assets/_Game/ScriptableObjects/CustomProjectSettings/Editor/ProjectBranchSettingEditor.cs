using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.AddressableAssets.Settings;
//using GooglePlayGames.Editor;

[CustomEditor(typeof(ProjectSettingAssets))]
public class ProjectBranchSettingEditor : Editor
{
    //float thumbnailWidth = 70;
    //float thumbnailHeight = 70;
    float labelWidth = 150f;

    public override void OnInspectorGUI()
    {
        ProjectSettingAssets projectSettingAsset = (ProjectSettingAssets)target;
        GUI.color = Color.yellow;
        projectSettingAsset.CurrentProjectBranch = (ProjectBranch)EditorGUILayout.EnumPopup("CurrentProjectBranch", projectSettingAsset.CurrentProjectBranch);
        GUI.color = Color.white;

        GUI.color = Color.grey;
        EditorGUILayout.HelpBox("Change ProjectBranch for build", MessageType.None);
        GUI.color = Color.white;

        GUI.color = Color.green;
        projectSettingAsset.ENABLE_REPORTER = EditorGUILayout.Toggle("ENABLE_REPORTER", projectSettingAsset.ENABLE_REPORTER);
        GUI.color = Color.white;
        GUI.color = Color.green;
        projectSettingAsset.ENABLE_DEBUG_DEFINE = EditorGUILayout.Toggle("ENABLE_DEBUG_DEFINE", projectSettingAsset.ENABLE_DEBUG_DEFINE);
        GUI.color = Color.white;
        GUI.color = Color.grey;
        EditorGUILayout.HelpBox("If it is UPLOAD_BUILD , Uncheck ENABLE_REPORTER", MessageType.None);
        GUI.color = Color.white;

        GUI.color = Color.green;
        projectSettingAsset.ENABLE_LOG_WRITE = EditorGUILayout.Toggle("ENABLE_LOG_WRITE", projectSettingAsset.ENABLE_LOG_WRITE);
        GUI.color = Color.white;
        GUI.color = Color.grey;
        EditorGUILayout.HelpBox("If it is UPLOAD_BUILD , Uncheck ENABLE_LOG_WRITE", MessageType.None);
        GUI.color = Color.white;

        GUI.color = Color.green;
        projectSettingAsset.ENABLE_GAMEPLAY_TESTING = EditorGUILayout.Toggle("ENABLE_GAMEPLAY_TESTING", projectSettingAsset.ENABLE_GAMEPLAY_TESTING);
        GUI.color = Color.white;
        GUI.color = Color.grey;
        EditorGUILayout.HelpBox("If Gameplay testing needed from Cricketsim scene, Check ENABLE_GAMEPLAY_TESTING  else always uncheck ENABLE_GAMEPLAY_TESTING", MessageType.None);
        GUI.color = Color.white;

        GUI.color = Color.green;
        projectSettingAsset.APP_BUNDLE_BUILD = EditorGUILayout.Toggle("APP_BUNDLE_BUILD", projectSettingAsset.APP_BUNDLE_BUILD);
        GUI.color = Color.white;
        GUI.color = Color.grey;
        EditorGUILayout.HelpBox("If need make aap build  to internal, Check APP_BUNDLE_BUILD  else always uncheck APP_BUNDLE_BUILD", MessageType.None);
        GUI.color = Color.white;


        GUILayout.BeginHorizontal();
        GUILayout.Label("Third Party Sdk Settings", GUILayout.Width(labelWidth));
        projectSettingAsset.thirdPartySdkKeys = (ThirdPartySdkKeys)EditorGUILayout.ObjectField(projectSettingAsset.thirdPartySdkKeys, typeof(ThirdPartySdkKeys), false);
        GUILayout.EndHorizontal();


        if (!projectSettingAsset.EditEnablePlayerSetting)
        {
            UpdateVersionCode(ref projectSettingAsset);
        }
        if (projectSettingAsset.EditEnablePlayerSetting)
        {
            GUI.color = Color.cyan;
        }
        else
        {
            GUI.color = Color.white;

        }
        if (GUILayout.Button("EditPlayerSetting"))
        {
            projectSettingAsset.EditEnablePlayerSetting = !projectSettingAsset.EditEnablePlayerSetting;
        }

        GUI.color = Color.white;

        if (projectSettingAsset.EditEnablePlayerSetting)
        {
#if UNITY_ANDROID
            EditorGUILayout.LabelField("Identification", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label("BundleVersionCode", GUILayout.Width(labelWidth));
            projectSettingAsset.BundleVersionCode = GUILayout.TextField(projectSettingAsset.BundleVersionCode);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Version", GUILayout.Width(labelWidth));
            projectSettingAsset.Version = GUILayout.TextField(GetVersion());
            PlayerSettings.bundleVersion = projectSettingAsset.Version;
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Package Name", GUILayout.Width(labelWidth));
            projectSettingAsset.PackageName = GUILayout.TextField(projectSettingAsset.PackageName);
            //PlayerSettings.applicationIdentifier = projectSettingAsset.PackageName;
            //PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, projectSettingAsset.PackageName);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Publisher Setting", EditorStyles.boldLabel);
            GUILayout.EndHorizontal();
            if (string.IsNullOrWhiteSpace(PlayerSettings.Android.keystoreName) || !PlayerSettings.Android.keystoreName.Contains(".keystore"))
            {
                GUI.color = Color.red;
            }
            else
            {
                GUI.color = Color.green;
            }
            GUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox("KeyStorerPath: " + (PlayerSettings.Android.keystoreName), MessageType.None);
            GUILayout.EndHorizontal();
            GUI.color = Color.white;
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("KeyAliasName: " + (PlayerSettings.Android.keyaliasName));
            GUILayout.EndHorizontal();
            if (!string.IsNullOrEmpty(PlayerSettings.Android.keystoreName)
               && File.Exists(PlayerSettings.Android.keystoreName)
               && !string.IsNullOrEmpty(PlayerSettings.Android.keyaliasName))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("KeystorePass", GUILayout.Width(labelWidth));
                PlayerSettings.keystorePass = GUILayout.PasswordField(PlayerSettings.keystorePass, '*');
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("KeyaliasPass", GUILayout.Width(labelWidth));
                PlayerSettings.keyaliasPass = GUILayout.PasswordField(PlayerSettings.keyaliasPass, '*');
                GUILayout.EndHorizontal();
            }
#endif

        }
        else
        {
            GUI.color = Color.grey;
            EditorGUILayout.HelpBox("Edit Player Setting: after edit try to setup project successfuly", MessageType.None);
            GUI.color = Color.white;
        }


        if (projectSettingAsset.ViewPlayerSetting)
        {
            GUI.color = Color.cyan;
        }
        else
        {
            GUI.color = Color.white;

        }

        if (GUILayout.Button("ViewPlayerSetting"))
        {
            projectSettingAsset.ViewPlayerSetting = !projectSettingAsset.ViewPlayerSetting;
        }

        GUI.color = Color.gray;
        if (projectSettingAsset.ViewPlayerSetting)
        {
            EditorGUILayout.LabelField("View Player Setting", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Platform: " + EditorUserBuildSettings.activeBuildTarget, MessageType.None);
            EditorGUILayout.HelpBox("BundleVersionCode: " + projectSettingAsset.BundleVersionCode.ToString(), MessageType.None);
            EditorGUILayout.HelpBox("Package Name: " + projectSettingAsset.PackageName, MessageType.None);
            EditorGUILayout.HelpBox("Version: " + GetVersion(), MessageType.None);
            EditorGUILayout.HelpBox("ScriptingDefineSymbol: " + PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup), MessageType.None);
            EditorGUILayout.HelpBox("Default Screen Orientation: " + PlayerSettings.defaultInterfaceOrientation.ToString(), MessageType.None);

        }
        else
        {
            GUI.color = Color.grey;
            EditorGUILayout.HelpBox("View Player Setting", MessageType.None);
            GUI.color = Color.white;
        }
        if (projectSettingAsset.ViewThirdPartySdkKeys)
        {
            GUI.color = Color.cyan;
        }
        else
        {
            GUI.color = Color.white;

        }
        if (GUILayout.Button("View SDK Keys"))
        {
            projectSettingAsset.ViewThirdPartySdkKeys = !projectSettingAsset.ViewThirdPartySdkKeys;
        }
        if (projectSettingAsset.ViewThirdPartySdkKeys)
        {
            GUI.color = Color.white;
            // EditorGUILayout.LabelField("-----------MIXPANEL SETTINGS--------", EditorStyles.boldLabel);
            // GUILayout.BeginHorizontal();
            // GUILayout.Label("MixPanel Debug Token", GUILayout.Width(labelWidth));
            // EditorGUILayout.HelpBox("" + projectSettingAsset.deftThirdPartySdkKeys.mixpanelSettings.DebugToken, MessageType.None);
            //GUILayout.EndHorizontal();
            //GUILayout.BeginHorizontal();
            //GUILayout.Label("MixPanel Runtime Token ", GUILayout.Width(labelWidth));
            // EditorGUILayout.HelpBox("" + projectSettingAsset.deftThirdPartySdkKeys.mixpanelSettings.RuntimeToken, MessageType.None);
            //GUILayout.EndHorizontal();
            //EditorGUILayout.LabelField("-----------IRON  SOURCE  SETTINGS--------", EditorStyles.boldLabel);
            //GUILayout.BeginHorizontal();
            //GUILayout.Label("API_KEY", GUILayout.Width(labelWidth));
            //GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("-----------APPLOVIN SETTINGS--------", EditorStyles.boldLabel);
            //GUILayout.BeginHorizontal();
            //GUILayout.Label($"Quality Service Enabled: { projectSettingAsset.thirdPartySdkKeys.applovinSettings.QualityServiceEnabled.ToString()} ");
            //GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label($"Applovin SDK key: {projectSettingAsset.thirdPartySdkKeys.applovinSDKKey} ");
            GUILayout.EndHorizontal();


            //if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android){
            //    EditorGUILayout.HelpBox("" + projectSettingAsset.deftThirdPartySdkKeys.ironSourceMediationSettings.AndroidAppKey, MessageType.None);
            //}
            //else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS){
            //    EditorGUILayout.HelpBox("" + projectSettingAsset.deftThirdPartySdkKeys.ironSourceMediationSettings.IOSAppKey, MessageType.None);
            //}




        }
        else
        {
            GUI.color = Color.grey;
            EditorGUILayout.HelpBox("View Player Settings: after edit try to setup project successfuly", MessageType.None);
            GUI.color = Color.white;
        }
        GUI.color = Color.green;
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(" ", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(" ", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("SETUP", GUILayout.Height(25)))
        {

            UpdateProjectSetting(ref projectSettingAsset);
            projectSettingAsset.ViewPlayerSetting = true;

            PlayerPrefs.DeleteAll();
            Debug.Log("User PlayerPrefs data successfully cleared");
            Debug.Log("Project Branch Setup Sucessful!");
            BuildAddresable(false);
            var setting = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings;
            setting.ActivePlayModeDataBuilderIndex = 0;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }
        GUILayout.EndHorizontal();

        GUI.color = Color.grey;
        EditorGUILayout.HelpBox("Setup project branch", MessageType.None);
        GUI.color = Color.white;
        GUI.color = Color.yellow;

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(" ", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(" ", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("BUILD ANDROID", GUILayout.Height(40)))
        {

            ShowBuildSetup(ref projectSettingAsset);
        }

        GUI.color = Color.grey;
        EditorGUILayout.HelpBox("Build android ", MessageType.None);
        GUI.color = Color.white;
    }
    static void ShowBuildSetup(ref ProjectSettingAssets projectSettingAsset)
    {
        if (EditorUtility.DisplayDialog("Android Build",
            "Do you want to make android build?",
            "Build",
            "Cancel"))
        {
            UpdateProjectSetting(ref projectSettingAsset);
            BuildAddresable(true);
            BuildAndroid(ref projectSettingAsset);
        }
        else
        {
            Debug.Log("Build Canceled!!");
        }


    }
    private static void BuildAndroid(ref ProjectSettingAssets projectSettings)
    {
        HandleAndroidKeystore(ref projectSettings);
        PlayerSettings.Android.bundleVersionCode = projectSettings.GetBundleVersionCode();
        PlayerSettings.bundleVersion = projectSettings.GetVersion();
        //PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        string buildFolderPath = Application.dataPath + "/../" + "build";
        string buildPathName = buildFolderPath + "/" +
            GetAppProductName(projectSettings.CurrentProjectBranch) + "_" + "V" + projectSettings.Version + "_" + projectSettings.GetBundleVersionCode() + "_" +
            GetprojectBranchName(projectSettings.CurrentProjectBranch) +
            GetEnableLogString(ref projectSettings) + "." + (projectSettings.APP_BUNDLE_BUILD ? "aab" : "apk");

        BuildReport buildReport = BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, buildPathName, EditorUserBuildSettings.activeBuildTarget,
        BuildOptions.None);
        if (buildReport.summary.result != BuildResult.Succeeded)
            throw new Exception($"Build ended with {buildReport.summary.result} status");

        else if (buildReport.summary.result == BuildResult.Succeeded)
        {
            ShowExplorer(buildPathName);
        }
        var setting = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings;
        setting.ActivePlayModeDataBuilderIndex = 0;
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void ShowExplorer(string itemPath)
    {
        itemPath = itemPath.Replace(@"/", @"\");   // explorer doesn't like front slashes
        System.Diagnostics.Process.Start("explorer.exe", "/select," + itemPath);
    }

    private static string GetEnableLogString(ref ProjectSettingAssets projectSettings)
    {
        if (projectSettings.ENABLE_REPORTER && projectSettings.CurrentProjectBranch != ProjectBranch.UPLOAD_BUILD)
        {
            return "_WithLog";
        }
        else
        {
            return "";
        }

    }
    private static void HandleAndroidKeystore(ref ProjectSettingAssets projectSettingAsset)
    {
        string currentPath = Application.dataPath;
        string keyStorePath = currentPath + "/Keystore/ss.keystore";
        if (!File.Exists(keyStorePath))
        {
            throw new Exception("not found, skipping setup, using Unity's default keystore " + keyStorePath);
        }
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = keyStorePath;
        PlayerSettings.Android.keyaliasName = "ss";
        PlayerSettings.Android.keystorePass = "naren137";
        PlayerSettings.Android.keyaliasPass = "naren137";
    }
    private static void BuildAddresable(bool shouldCleanContent)
    {
        //AddressableAssetSettings.Create();
        var setting = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.GetSettings(true);
        setting.activeProfileId = setting.profileSettings.GetProfileId("Default");
        setting.ActivePlayModeDataBuilderIndex = 2;
        if (shouldCleanContent)
        {
            AddressableAssetSettings.CleanPlayerContent();
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        AddressableAssetSettings.BuildPlayerContent();
    }

    static void UpdateVersionCode(ref ProjectSettingAssets projectSettingAsset)
    {
#if UNITY_ANDROID
        projectSettingAsset.BundleVersionCode = PlayerSettings.Android.bundleVersionCode.ToString();
#elif UNITY_IOS
            projectSettingAsset.BundleVersionCode = PlayerSettings.iOS.buildNumber;
#endif
    }
    static void SetVersionCode(ref ProjectSettingAssets projectSettingAsset)
    {
#if UNITY_ANDROID
        int.TryParse(projectSettingAsset.BundleVersionCode, out int BundleVersionCode);
        PlayerSettings.Android.bundleVersionCode = BundleVersionCode;
#elif UNITY_IOS
            projectSettingAsset.BundleVersionCode = PlayerSettings.iOS.buildNumber;
#endif
    }
    static string GetVersion()
    {
        return Application.version;
    }
    static void UpdateProjectSetting(ref ProjectSettingAssets projectSettingAsset)
    {
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            UpdateAndroidSetting(ref projectSettingAsset);
        }

        EditorUserBuildSettings.buildAppBundle = projectSettingAsset.APP_BUNDLE_BUILD;

        PlayerSettings.gcIncremental = true;
        //EditorUserBuildSettings.development = projectSettingAsset.CurrentProjectBranch != ProjectBranch.UPLOAD_BUILD;
        AddDefineSymbols(ref projectSettingAsset);
        SetVersionCode(ref projectSettingAsset);
        UpdateBundleIdentifier(ref projectSettingAsset);
        UpdateScreenOrientation(ref projectSettingAsset);
        //projectSettingAsset.deftThirdPartySdkKeys.SetUpBranchData(projectSettingAsset.CurrentProjectBranch);
        //SetupManifestFile(ref projectSettingAsset);
        //projectSettingAsset.deftThirdPartySdkKeys.UpdateIronSourceSettings();
        //  projectSettingAsset.deftThirdPartySdkKeys.UpdateMixpanelSettings(projectSettingAsset.CurrentProjectBranch);
        //projectSettingAsset.deftThirdPartySdkKeys.AppsFlyerDebugSettings(projectSettingAsset.CurrentProjectBranch);

        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            UpdateGooglePlayServicesFile(ref projectSettingAsset);
        }
        //EditorUtility.SetDirty(projectSettingAsset.deftThirdPartySdkKeys.mixpanelSettings);
        //EditorUtility.SetDirty(projectSettingAsset.deftThirdPartySdkKeys.ironSourceMediationSettings);
        //EditorUtility.SetDirty(projectSettingAsset.deftThirdPartySdkKeys.branchData);
        EditorUtility.SetDirty(projectSettingAsset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    static void UpdateScreenOrientation(ref ProjectSettingAssets projectSettingAsset)
    {
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
        PlayerSettings.allowedAutorotateToPortrait = true;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = true;
#if DEVLOPMENT_BUILD
        PlayerSettings.allowedAutorotateToLandscapeRight = true;
        PlayerSettings.allowedAutorotateToLandscapeLeft = true;
#elif UPLOAD_BUILD
        PlayerSettings.allowedAutorotateToLandscapeRight = false;
        PlayerSettings.allowedAutorotateToLandscapeLeft = false;
#endif

    }

    static void UpdateGooglePlayServicesFile(ref ProjectSettingAssets projectSettingAsset)
    {
        //var thirdPartySdk = projectSettingAsset.deftThirdPartySdkKeys;
        //if (projectSettingAsset.CurrentProjectBranch == ProjectBranch.UPLOAD_BUILD)
        //{
        //    File.WriteAllText(AssetDatabase.GetAssetPath(thirdPartySdk.goolgeService), thirdPartySdk.goolgeService_Upload.text);
        //    EditorUtility.SetDirty(thirdPartySdk.goolgeService);
        //}
        //else
        //{           
        //File.WriteAllText(AssetDatabase.GetAssetPath(thirdPartySdk.goolgeService), thirdPartySdk.goolgeService_Dev.text);
        //EditorUtility.SetDirty(thirdPartySdk.goolgeService);
        //}
    }
    static void UpdateBundleIdentifier(ref ProjectSettingAssets projectSettingAsset)
    {
        // PlayerSettings.SetApplicationIdentifier(EditorUserBuildSettings.selectedBuildTargetGroup, ProjectSettingAssets.GetBundleId(ref projectSettingAsset));
    }

    //static void UpdateGSUploadFileNameForBuild(ref ProjectSettingAssets projectSettingAsset)
    //{
    //    string assetPath = AssetDatabase.GetAssetPath(projectSettingAsset.thirdPartySdkKeys.goolgeService_Upload.GetInstanceID());
    //    AssetDatabase.RenameAsset(assetPath, "google-services");
    //}

    //static void UpdateGSTestFileNameForBuild(ref ProjectSettingAssets projectSettingAsset)
    //{
    //    string assetPath = AssetDatabase.GetAssetPath(projectSettingAsset.thirdPartySdkKeys.goolgeService_Dev.GetInstanceID());
    //    AssetDatabase.RenameAsset(assetPath, "google-services");
    //}

    //static void ResetGSUploadFileName(ref ProjectSettingAssets projectSettingAsset)
    //{
    //    string assetPath = AssetDatabase.GetAssetPath(projectSettingAsset.thirdPartySdkKeys.goolgeService_Upload.GetInstanceID());
    //    AssetDatabase.RenameAsset(assetPath, "google-services-prod");
    //}

    //static void ResetGSTestFileName(ref ProjectSettingAssets projectSettingAsset)
    //{
    //    string assetPath = AssetDatabase.GetAssetPath(projectSettingAsset.thirdPartySdkKeys.goolgeService_Dev.GetInstanceID());
    //    AssetDatabase.RenameAsset(assetPath, "google-services-dev");
    //}

    /// <summary>
    /// Add define symbols as soon as Unity gets done compiling.
    /// </summary>
    static void AddDefineSymbols(ref ProjectSettingAssets projectSettingAsset)
    {
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> allDefines = new List<string>();
        var defineStringAray = definesString.Split(';');
        for (int i = 0; i < defineStringAray.Length; i++)
        {
            if (!ProjectSettingAssets.predefinedSymbolList.Contains(defineStringAray[i]))
            {
                allDefines.Add(defineStringAray[i]);
            }
        }
        allDefines.AddRange(ProjectSettingAssets.GetDefineSymoblList(ref projectSettingAsset));
        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            string.Join(";", allDefines.ToArray()));

    }

    static void UpdateAndroidSetting(ref ProjectSettingAssets projectSettingAsset)
    {
        //PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
        if (projectSettingAsset.APP_BUNDLE_BUILD)
        {
            //PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel30;
        }
        else
        {
            //PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
        }
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);
        PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.Android, Il2CppCompilerConfiguration.Release);
        AndroidArchitecture aac = AndroidArchitecture.ARM64;
        aac |= AndroidArchitecture.ARMv7;
        PlayerSettings.Android.targetArchitectures = aac;
        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Android, ManagedStrippingLevel.Low);
        PlayerSettings.companyName = "Sovereign Studios";
        PlayerSettings.productName = GetProductName(projectSettingAsset.CurrentProjectBranch);
        PlayerSettings.fullScreenMode = FullScreenMode.FullScreenWindow;
        //PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, GetPackageName(projectSettingAsset.CurrentProjectBranch));
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, projectSettingAsset.PackageName);
        PlayerSettings.Android.useCustomKeystore = true;

        //Orentation
        //if (projectSettingAsset.CurrentProjectBranch == ProjectBranch.DEVELOPMENT_BUILD ||
        //    projectSettingAsset.CurrentProjectBranch == ProjectBranch.STAGING_BUILD ||
        //    projectSettingAsset.CurrentProjectBranch == ProjectBranch.MASTER_BUILD ||
        //     projectSettingAsset.CurrentProjectBranch == ProjectBranch.UPLOAD_BUILD)
        //{
        //    PlayerSettings.Android.useCustomKeystore = true;
        //}
        //else
        //{
        //    PlayerSettings.Android.useCustomKeystore = false;
        //}

        PlayerSettings.use32BitDisplayBuffer = true;
        PlayerSettings.Android.disableDepthAndStencilBuffers = false;
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android,
            new UnityEngine.Rendering.GraphicsDeviceType[] { UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3 });
        PlayerSettings.muteOtherAudioSources = false;
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();


    }
    private static string GetPackageName(ProjectBranch projectBranch)
    {
        switch (projectBranch)
        {
            case ProjectBranch.DEVELOPMENT_BUILD:
            case ProjectBranch.UPLOAD_BUILD:
                return "com.sovereign.studios";
            default:
                return "com.sovereign.studios";
        }
    }
    private static string GetprojectBranchName(ProjectBranch projectBranch)
    {
        switch (projectBranch)
        {
            case ProjectBranch.DEVELOPMENT_BUILD:
                return "DEV";
            case ProjectBranch.UPLOAD_BUILD:
                return "UPLOAD";
            default:
                return "DEV";
        }
    }
    private static string GetAppProductName(ProjectBranch projectBranch)
    {
        switch (projectBranch)
        {
            case ProjectBranch.DEVELOPMENT_BUILD:
            case ProjectBranch.UPLOAD_BUILD:
                return "Tiger_and_Goat";
            default:
                return "Tiger_and_Goat";
        }
    }
    private static string GetProductName(ProjectBranch projectBranch)
    {
        switch (projectBranch)
        {
            case ProjectBranch.DEVELOPMENT_BUILD:
            case ProjectBranch.UPLOAD_BUILD:
                return "Tiger and Goat";
            default:
                return "Tiger and Goat";
        }
    }

    private static void SetupManifestFile(ref ProjectSettingAssets projectSettingAsset)
    {
        //var branchData = projectSettingAsset.deftThirdPartySdkKeys.branchData;
        //if (projectSettingAsset.CurrentProjectBranch == ProjectBranch.UPLOAD_BUILD)
        //{
        //    WriteManifest(ref projectSettingAsset, branchData.testMode, "io.branch.sdk.BranchKey", branchData.liveBranchKey, branchData.liveBranchUri, branchData.liveAppLinks[0], branchData.liveAppLinks[1]);
        //}
        //else
        //{
        //    WriteManifest(ref projectSettingAsset, branchData.testMode, "io.branch.sdk.BranchKey.test", branchData.testBranchKey, branchData.testBranchUri, branchData.testAppLinks[0], branchData.testAppLinks[1]);
        //}
    }

    private static void WriteManifest(ref ProjectSettingAssets projectSettingAsset, bool mode, string branchKeyName, string branchKey, string branchUri, string branchAppLink1, string branchAppLink2)
    {
        //XmlDocument xmlDoc = new XmlDocument();
        //var manifestPath = AssetDatabase.GetAssetPath(projectSettingAsset.deftThirdPartySdkKeys.androidManifestFile);
        //xmlDoc.Load(manifestPath);

        //XmlNodeList aNodes = xmlDoc.SelectNodes("/manifest/application/activity/intent-filter");
        //XmlNode intentFilterUriNode = null;
        //XmlNode intentFilterAppLink1Node = null;
        //XmlNode intentFilterAppLink2Node = null;

        //foreach (XmlNode node in aNodes)
        //{

        //    foreach (XmlNode childNode in node.ChildNodes)
        //    {
        //        foreach (XmlAttribute attr in childNode.Attributes)
        //        {

        //            if (attr.Name.Contains("host") && attr.Value == "open")
        //            {
        //                intentFilterUriNode = childNode;
        //                break;
        //            }
        //            else if (attr.Name.Contains("host") && !attr.Value.Contains("alternate"))
        //            {
        //                intentFilterAppLink1Node = childNode;
        //                break;
        //            }
        //            else if (attr.Name.Contains("host") && attr.Value.Contains("alternate"))
        //            {
        //                intentFilterAppLink2Node = childNode;
        //                break;
        //            }
        //        }
        //    }
        //}

        //foreach (XmlAttribute attr in intentFilterUriNode.Attributes)
        //{
        //    if (attr.Name.Contains("scheme"))
        //    {
        //        attr.Value = branchUri;
        //    }
        //}

        //foreach (XmlAttribute attr in intentFilterAppLink1Node.Attributes)
        //{
        //    if (attr.Name.Contains("host"))
        //    {
        //        attr.Value = branchAppLink1;
        //    }
        //}

        //foreach (XmlAttribute attr in intentFilterAppLink2Node.Attributes)
        //{
        //    if (attr.Name.Contains("host"))
        //    {
        //        attr.Value = branchAppLink2;
        //    }
        //}

        //XmlNodeList metaDataNodes = xmlDoc.SelectNodes("/manifest/application/meta-data");
        //XmlNode intentFilterMetaTestModeNode = null;
        //XmlNode intentFilterMetaKeyNode = null;
        //foreach (XmlNode node in metaDataNodes)
        //{
        //    foreach (XmlAttribute attr in node.Attributes)
        //    {
        //        if (attr.Value.Contains("io.branch.sdk.TestMode"))
        //        {
        //            intentFilterMetaTestModeNode = node;
        //            break;
        //        }
        //        if (attr.Value.Contains("io.branch.sdk.BranchKey"))
        //        {
        //            intentFilterMetaKeyNode = node;
        //            attr.Value = branchKeyName;
        //            break;
        //        }
        //    }
        //}

        //foreach (XmlAttribute attr in intentFilterMetaTestModeNode.Attributes)
        //{
        //    if (attr.Name.Contains("value"))
        //    {
        //        attr.Value = mode ? "true" : "false";
        //    }
        //}

        //foreach (XmlAttribute attr in intentFilterMetaKeyNode.Attributes)
        //{
        //    if (attr.Name.Contains("value"))
        //    {
        //        attr.Value = branchKey;
        //    }
        //}

        //xmlDoc.Save(manifestPath);

        //TextReader manifestReader = new StreamReader(manifestPath);
        //string content = manifestReader.ReadToEnd();
        //manifestReader.Close();

        //TextWriter manifestWriter = new StreamWriter(manifestPath);
        //manifestWriter.Write(content);
        //manifestWriter.Close();
    }
}



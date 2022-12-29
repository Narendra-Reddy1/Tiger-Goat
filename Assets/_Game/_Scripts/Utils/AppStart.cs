using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppStart : MonoBehaviour
{
    [SerializeField] private ProjectAssetManager assetManager;
    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        Initialize();
    }
    private void Initialize()
    {
        if (!ProjectSetting.IsProjectSettingInitialized())
            ProjectSetting.InitializeProjectSetting(assetManager);
        MaxSdk.SetSdkKey(assetManager.projectSettingAssets.thirdPartySdkKeys.applovinSettings.SdkKey);
        MaxSdk.InitializeSdk();
    }
}

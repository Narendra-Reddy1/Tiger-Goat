using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppStart : MonoBehaviour
{
    [SerializeField] private ProjectAssetManager assetManager;
    [SerializeField] private GameObject reporter;

    private void Awake()
    {
        MaxSdk.SetSdkKey(assetManager.projectSettingAssets.thirdPartySdkKeys.applovinSDKKey);
        MaxSdk.InitializeSdk();
    }
    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        Initialize();
    }
    private void Initialize()
    {
#if ENABLE_REPORTER && !UPLOAD_BUILD
        SovereignStudios.SovereignUtils.Log($"Reporter created");
        DontDestroyOnLoad(Instantiate(reporter));
#endif
        if (!ProjectSetting.IsProjectSettingInitialized())
            ProjectSetting.InitializeProjectSetting(assetManager);
      

    }
}

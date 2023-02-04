using SovereignStudios.Utils;
using System.Collections;
using UnityEngine;

public class AppStart : MonoBehaviour
{
    [SerializeField] private ProjectAssetManager assetManager;
    [SerializeField] private GameObject reporter;

    private void Awake()
    {
        MaxSdk.SetSdkKey(assetManager.projectSettingAssets.thirdPartySdkKeys.applovinSDKKey);
        MaxSdk.InitializeSdk();
        MaxSdkCallbacks.OnSdkInitializedEvent += (config) =>
        {
            SovereignUtils.Log($"Initialized Max Sdk: {JsonUtility.ToJson(config)}");
        };
    }
    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        Initialize();
    }
    private void Initialize()
    {
#if ENABLE_REPORTER && !UPLOAD_BUILD
        SovereignUtils.Log($"Reporter created");
        DontDestroyOnLoad(Instantiate(reporter));
#endif
        if (!ProjectSetting.IsProjectSettingInitialized())
            ProjectSetting.InitializeProjectSetting(assetManager);
    }
}

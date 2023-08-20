using DG.Tweening;
using SovereignStudios;
using SovereignStudios.EventSystem;
using SovereignStudios.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitSceneInitializer : MonoBehaviour
{
    [SerializeField] private AssetReference initScene;
    [SerializeField] private AssetReference persistentScene;
    [SerializeField] private AssetReference mainScene;
    WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    private UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle mainSceneHandle;
    private SceneInstance mainSceneInstance;
    private IEnumerator Start()
    {
        yield return WaitForEndOfFrame;

        ShowLoadingScreen();
        Addressables.LoadSceneAsync(persistentScene, LoadSceneMode.Additive).Completed += (handle) =>
        {
            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                mainScene.LoadSceneAsync(LoadSceneMode.Additive, false).Completed += (mainSceneHandle) =>
                {
                    this.mainSceneHandle = mainSceneHandle;
                    mainSceneInstance = mainSceneHandle.Result;
                    GlobalEventHandler.TriggerEvent(EventID.REQUEST_TO_PLAY_BGM, AudioID.GameplayBGM);
                };
        };
    }

    //to shift logic to another script easily if needed.☺
    #region LoadinScreen Logic

    [Space(25)]
    [Header("Loading Screen")]
    [SerializeField] private Transform loadingBarPanel;
    [SerializeField] private CanvasGroup tapToStartTxtCanvasGroup;
    [SerializeField] private CanvasGroup loadingScreenCanvasGroup;
    [SerializeField] private Image fillBar;
    [SerializeField] private float fakeDuration = 2f;
    [SerializeField] private Button offScreenTabBtn;
    [SerializeField] private Image screenFader;

    private void ShowLoadingScreen()
    {
        loadingScreenCanvasGroup.DOFade(1, 0);
        fillBar.DOFillAmount(1, fakeDuration).onComplete += () =>
        {
            //HideLoadingScreen();
            offScreenTabBtn.interactable = true;
            tapToStartTxtCanvasGroup.DOFade(1, 0.35f).onComplete += () =>
            {

                tapToStartTxtCanvasGroup.DOFade(.75f, 1.25f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
            };
            SovereignUtils.Log($"Done with loading");
        };
    }
    private void HideLoadingScreen()
    {
        loadingScreenCanvasGroup.DOFade(0, 0.35f).onComplete += () =>
          {
              fillBar.fillAmount = 0;
          };
    }


    public void OnClickOffScreenTab()
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_REQEST_FADE_SCREEN_IN, new System.Tuple<System.Action>(Oncomplete));

        SovereignUtils.Log($"NAME: {Constants.INIT_SCENE} {mainSceneHandle.IsDone}");
        void Oncomplete()
        {
            if (!mainSceneHandle.IsDone) return;
            mainSceneInstance.ActivateAsync().completed += (handle) =>
            {
                SceneManager.SetActiveScene(mainSceneInstance.Scene);
                GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_CHANGE_SCREEN_REQUESTED, new ScreenChangeProperties(Window.MainMenu, onComplete: () =>
                  {
                      GlobalEventHandler.TriggerEvent(EventID.EVENT_REQEST_FADE_SCREEN_OUT);
                      SovereignUtils.UnloadSceneAsync(Constants.INIT_SCENE);

                  }));
            };
            SovereignUtils.Log($"Done with Fading In");
        }
    }

    #endregion LoadingScreen Logic
}

//[System.Serializable]
//public class AssetReferenceScene : AssetReferenceT<Object>
//{
//    public AssetReferenceScene(string guid) : base(guid)
//    {
//    }
//}
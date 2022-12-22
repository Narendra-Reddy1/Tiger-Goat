using SovereignStudios;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitSceneInitializer : MonoBehaviour
{
    WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();

    private IEnumerator Start()
    {
        yield return WaitForEndOfFrame;
        SovereignUtils.LoadSceneAsync(Constants.PERSISTENT_SCENE, onComplete: () =>
         {
             SovereignUtils.LoadSceneAsync(Constants.MAIN_SCENE, true, () =>
             {
                 GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_CHANGE_SCREEN_REQUESTED, new System.Tuple<Window, ScreenType, bool, System.Action>(Window.MainMenu, ScreenType.Replace, false, () =>
                 {
                     SovereignUtils.UnloadSceneAsync(Constants.INIT_SCENE);
                 }));
             });
         });
    }
}

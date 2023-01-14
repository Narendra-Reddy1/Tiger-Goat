using UnityEngine;
using SovereignStudios;
using SovereignStudios.EventSystem;
using SovereignStudios.Utils;
using SovereignStudios.Enums;

public class WinOrDefeatHandler : MonoBehaviour
{
    #region Variables

    public static GameResult gameResult;

    #endregion Variables

    #region Unity Methods
    private void OnEnable()
    {
        GlobalEventHandler.AddListener(EventID.EVEN_ON_LEVEL_FINISHED, Callback_On_Level_Finished);
        GlobalEventHandler.AddListener(EventID.EVENT_RESTART_LEVEL_REQUESTED, Callback_On_Level_Restart_Requested);
    }
    private void OnDisable()
    {
        GlobalEventHandler.RemoveListener(EventID.EVEN_ON_LEVEL_FINISHED, Callback_On_Level_Finished);
        GlobalEventHandler.RemoveListener(EventID.EVENT_RESTART_LEVEL_REQUESTED, Callback_On_Level_Restart_Requested);
    }

    #endregion Unity Methods

    #region Public Methods

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #region Callbacks

    //private void Update()
    //{
    //    if (Input.GetKeyUp(KeyCode.L))
    //    {
    //        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_CHANGE_SCREEN_REQUESTED, new ScreenChangeProperties(Window.GameOverScreen, ScreenType.Additive, true, null));
    //        SovereignUtils.Log($"@@@@ CurrentScreen: {((Window)GlobalEventHandler.TriggerEventForReturnType(EventID.EVENT_REQUEST_GET_CURRENT_SCREEN)).ToString()}");
    //        SovereignUtils.Log($"@@@@ PreviousScreen: {((Window)GlobalEventHandler.TriggerEventForReturnType(EventID.EVENT_REQUEST_GE_PREVIOUS_SCREEN)).ToString()}");

    //    }

    //}
    private void Callback_On_Level_Finished(object args)
    {
        gameResult = (GameResult)args;
        SovereignUtils.DelayedCallback(1f, () =>
        {

            GameplayManager.SetGameState(GameState.Ended);
            GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_CHANGE_SCREEN_REQUESTED, new ScreenChangeProperties(Window.GameOverScreen, ScreenType.Additive, true, null));
        });
    }


    private void Callback_On_Level_Restart_Requested(object arg)
    {
        gameResult = GameResult.None;
    }
    #endregion Callbacks


}

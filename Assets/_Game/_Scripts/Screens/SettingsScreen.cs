using SovereignStudios;
using SovereignStudios.EventSystem;
using SovereignStudios.Utils;
using UnityEngine;

public class SettingsScreen : PopupBase
{
    #region Variables
    #endregion Variables

    #region Unity Methods
    public override void OnEnable()
    {
        base.OnEnable();
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SHOW_MREC_AD_REQUESTED);
        GlobalEventHandler.AddListener(EventID.EVENT_ON_AD_STATE_CHANGED, Callback_On_Ad_State_Changed);
    }
    public override void OnDisable()
    {
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_AD_STATE_CHANGED, Callback_On_Ad_State_Changed);
    }
    #endregion Unity Methods

    #region Private Methods
    private void ToggleAudio()
    {
    }
    private void QuitToTheMainMenu()
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_HIDE_MREC_AD_REQUESTED);
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SHOW_INTERSTITIAL_AD_REQUESTED);
    }
    #endregion Private  Methods

    #region Public Methods
    public void OnClickToggleAudioButton()
    {
        ToggleAudio();
    }
    public void OnClickQuitButton()
    {
        QuitToTheMainMenu();
    }
    public override void OnCloseClick()
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_HIDE_MREC_AD_REQUESTED);
        base.OnCloseClick();
    }
    #endregion Public Methods

    #region Callbacks

    private void Callback_On_Ad_State_Changed(object args)
    {
        AdEventData eventData = args as AdEventData;
        switch (eventData.adState)
        {
            case AdState.INTERSTITIAL_DISMISSED:
                SovereignUtils.Log($"Inter dismissed: ");
                GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_CHANGE_SCREEN_REQUESTED, new ScreenChangeProperties(Window.MainMenu, enableDelay: true));
                break;
            default:
                break;
        }


    }
    #endregion Callbacks

}

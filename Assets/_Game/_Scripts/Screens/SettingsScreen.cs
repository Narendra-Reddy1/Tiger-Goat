using SovereignStudios;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsScreen : ScreenBase
{
    #region Variables
    #endregion Variables

    #region Unity Methods
    private void OnEnable()
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SHOW_MREC_AD_REQUESTED);
    }
    #endregion Unity Methods

    #region Private Methods
    private void ToggleAudio()
    {

    }
    private void QuitToTheMainMenu()
    {

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
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_CLOSE_LAST_ADDITIVE_SCREEN);
    }
    #endregion Public Methods

    #region Callbacks
    #endregion Callbacks

}

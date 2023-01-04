using SovereignStudios.EventSystem;
using SovereignStudios.Utils;
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
        if (GameObject.Find("MRec") != null)
            SovereignUtils.Log($"Found mrec!!");
        else if (GameObject.Find("MRec(clone)") != null)
            SovereignUtils.Log($"mrec clone!!!");
        else if (GameObject.Find("MRecCenter(clone)") != null)
            SovereignUtils.Log($"MRecCenter clone!!!");
        else if (GameObject.Find("CenteredMRec(clone)") != null)
            SovereignUtils.Log($"CenteredMRec clone!!!");
        else if (GameObject.Find("CenterMRec(clone)") != null)
            SovereignUtils.Log($"CenterMRec clone!!!");
        else if (GameObject.Find("MRec") != null)
            SovereignUtils.Log($"mrec");
        else if (GameObject.Find("MRecCenter") != null)
            SovereignUtils.Log($"MRecCenter");
        else if (GameObject.Find("CenteredMRec") != null)
            SovereignUtils.Log($"CenteredMRec");
        else if (GameObject.Find("CenterMRec") != null)
            SovereignUtils.Log($"CenterMRec");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SovereignStudios
{
    public class MainMenuScreen : ScreenBase
    {
        #region Variables

        #endregion Variables

        #region Unity Methods

        #endregion Unity Methods

        #region Public Methods
        public void OnClickPlayButton()
        {
            GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SCREEN_TRANSITION_REQUESTED, true);

            //GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_CHANGE_SCREEN_REQUESTED, new System.Tuple<Window, ScreenType, bool, System.Action>(Window.GenericPopup, ScreenType.Additive, true, () =>
            //{
            //    GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SETUP_GENERIC_POPUP_REQUESTED, new System.Tuple<string, string, UnityEngine.Events.UnityAction>("PLAY", "Testing.. " +
            //        "You clicked on play button.", () => { GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_CLOSE_LAST_ADDITIVE_SCREEN); }));
            //}));
            //ScreenManager.Instance.ChangeScreen(Window.GenericPopup, onComplete: () =>
            //{
            //    GenericPopup.SetupPopup("PLAY", "You clicked on the play on button to play!", () => { ScreenManager.Instance.CloseLastAdditiveScreen(); });
            //});
        }
        #endregion Public Methods

        #region Private Methods

        #endregion Private Methods

        #region Callbacks

        #endregion Callbacks
    }
}
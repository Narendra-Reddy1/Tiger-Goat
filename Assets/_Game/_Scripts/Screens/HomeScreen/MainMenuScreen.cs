using SovereignStudios.EventSystem;
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
            GlobalEventHandler.TriggerEvent(EventID.EVENT_REQUEST_TO_CHANGE_SCREEN_WITH_TRANSITION, Window.GameplayScreen);//ChangeScreenWithTransistion

            //GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_CHANGE_SCREEN_REQUESTED, new System.Tuple<Window, ScreenType, bool, System.Action>(Window.GameplayScreen, ScreenType.Replace, true, () =>
            //{
            //  //  GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SCREEN_TRANSITION_REQUESTED, false);//false uncovers the screen.
            //}));
        }
        #endregion Public Methods

        #region Private Methods

        #endregion Private Methods

        #region Callbacks

        #endregion Callbacks
    }
}
using SovereignStudios;
using SovereignStudios.Enums;
using SovereignStudios.EventSystem;
using SovereignStudios.Utils;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : PopupBase
{
    #region Variables
    [SerializeField] private Toggle m_audioToggle;
    [SerializeField] private Image m_audioToggleImage;
    [SerializeField] private SpriteManager m_spriteManager;
    private bool isRewardedAdWatchedCompletely = false;
    #endregion Variables

    #region Unity Methods
    public override void OnEnable()
    {
        base.OnEnable();
        m_audioToggle.onValueChanged.AddListener(OnClickToggleAudioButton);
        ToggleAudio(PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.audio_toggle, true));
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SHOW_MREC_AD_REQUESTED);
        GlobalEventHandler.AddListener(EventID.EVENT_ON_AD_STATE_CHANGED, Callback_On_Ad_State_Changed);
    }
    public override void OnDisable()
    {
        m_audioToggle.onValueChanged.RemoveListener(OnClickToggleAudioButton);
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_AD_STATE_CHANGED, Callback_On_Ad_State_Changed);
    }
    #endregion Unity Methods

    #region Private Methods
    private void ToggleAudio(bool value)
    {
        PlayerPrefsWrapper.SetPlayerPrefsBool(PlayerPrefKeys.audio_toggle, value);
        m_audioToggleImage.sprite = value ? m_spriteManager.resourcesDictionary[ResourceType.AudioON] : m_spriteManager.resourcesDictionary[ResourceType.AudioOFF];
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_AUDIO_TOGGLE_UPDATED);
    }
    private void QuitToTheMainMenu()
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_HIDE_MREC_AD_REQUESTED);
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_HIDE_BANNER_AD_REQUESTED);
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_CHANGE_SCREEN_REQUESTED, new ScreenChangeProperties(Window.MainMenu, enableDelay: true));
    }
    private void RestartLevel()
    {
        isRewardedAdWatchedCompletely = false;
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_CLOSE_LAST_ADDITIVE_SCREEN, new System.Tuple<System.Action>(() =>
        {
            GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_HIDE_MREC_AD_REQUESTED);
            GlobalEventHandler.TriggerEvent(EventID.EVENT_RESTART_LEVEL_REQUESTED);
            GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_LEVEL_STARTED);
        }));
    }
    #endregion Private  Methods

    #region Public Methods
    public void OnClickToggleAudioButton(bool value)
    {
        ToggleAudio(value);
    }
    public void OnClickQuitButton()
    {
        if (!(bool)GlobalEventHandler.TriggerEventForReturnType(EventID.EVENT_ON_INTERSTITIAL_AD_AVAILABILITY_REQUESTED))
        {
            QuitToTheMainMenu();
            return;
        }
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SHOW_INTERSTITIAL_AD_REQUESTED);
    }
    public void OnClickRestartButton()
    {
        if (!(bool)GlobalEventHandler.TriggerEventForReturnType(EventID.EVENT_ON_REWARDED_AD_AVAILABILITY_REQUESTED))
        {
            RestartLevel();
            return;
        }
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SHOW_REWARDED_AD_REQUESTED);
    }
    public void OnClickPrivacyPolicy()
    {
        Application.OpenURL("https://sites.google.com/view/benstudiosprivacypolicy/home?authuser=1");
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
                QuitToTheMainMenu();
                break;
            case AdState.REWARDED_REWARD_RECEIVED:
                isRewardedAdWatchedCompletely = true;
                break;
            case AdState.REWARDED_DISMISSED:
                if (isRewardedAdWatchedCompletely)
                    RestartLevel();
                break;
            default:
                break;
        }


    }
    #endregion Callbacks

}

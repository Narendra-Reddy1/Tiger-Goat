using SovereignStudios.EventSystem;
using SovereignStudios.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// It Manages and tracks the game sessions.
/// </summary>
public class SessionManager : MonoBehaviour
{
    #region Variables

    private int sessionCounter = -1;
    private bool isAppOpenAdDisplaying = false;
    private const string RECORDED_SESSIONS = "session_counter";
    private const byte WAITING_TIME_TO_SHOW_APP_OPEN_AD = 100;
    private Timer appColdStartTimer;

    #endregion Variables

    #region Unity Methods
    private void Awake()
    {
        RecordSession();
    }
    private void OnEnable()
    {
        GlobalEventHandler.AddListener(EventID.EVENT_ON_AD_STATE_CHANGED, Callback_On_Ad_State_Changed);
    }
    private void OnDisable()
    {
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_AD_STATE_CHANGED, Callback_On_Ad_State_Changed);
    }
    private void Start()
    {
        appColdStartTimer = new Timer();
        appColdStartTimer.StartTimer();
    }
    private void OnApplicationPause(bool pause)
    {
        ShowAppOpenAdOnApplicationPause(pause);
    }
    #endregion Unity Methods

    #region Public Methods

    #endregion Public Methods

    #region Private Methods

    private void RecordSession()
    {
        sessionCounter = PlayerPrefs.GetInt(RECORDED_SESSIONS, 0);
        sessionCounter++;
        Debug.Log($"Session: {sessionCounter}");
        PlayerPrefs.SetInt(RECORDED_SESSIONS, sessionCounter);
    }
    private void ShowAppOpenAdOnApplicationPause(bool pause)
    {

        if (appColdStartTimer.GetElapsedTimeInSeconds() <= WAITING_TIME_TO_SHOW_APP_OPEN_AD) return;
        if (pause)
        {
            if (!(bool)GlobalEventHandler.TriggerEventForReturnType(EventID.EVENT_ON_APP_OPEN_AD_AVAILABILITTY_REQUESTED))
                GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_LOAD_APP_OPEN_AD_REQUESTED);
        }
        else
        {
            if (!isAppOpenAdDisplaying)
            {
                GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SHOW_APP_OPEN_AD_REQUESTED);
            }
        }
    }
    private void OnDestroy()
    {
        appColdStartTimer = null;
    }
    #endregion Private Methods

    #region Callbacks
    private void Callback_On_Ad_State_Changed(object args)
    {
        AdEventData adEventData = args as AdEventData;
        switch (adEventData.adState)
        {
            case AdState.APP_OPEN_DISPLAYED:
                isAppOpenAdDisplaying = true;
                break;
            case AdState.APP_OPEN_AD_DISMISSED:
                isAppOpenAdDisplaying = false;
                appColdStartTimer.RestartTimer();
                break;
            default:
                break;
        }
    }
    #endregion Callbacks
}

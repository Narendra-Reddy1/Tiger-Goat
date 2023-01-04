using Coffee.UIEffects;
using SovereignStudios.EventSystem;
using SovereignStudios.Enums;
using SovereignStudios;
using UnityEngine;
using DG.Tweening;
using TMPro;
using SovereignStudios.Utils;

public class InGameUIManager : MonoBehaviour
{
    #region Variables

    [Header("Tiger")]
    [SerializeField] private UIEffect tigerUiEffect;
    [SerializeField] private UnityEngine.UI.Image tigerTimerBar;
    [SerializeField] private TextMeshProUGUI goatsKilledCountTxt;
    [Space(5)]
    [Header("Goat")]
    [SerializeField] private UIEffect goatUiEffect;
    [SerializeField] private UnityEngine.UI.Image goatTimerBar;
    [SerializeField] private TextMeshProUGUI goatsPlacedCountTxt;

    #endregion Variables

    #region Unity Methods
    private void OnEnable()
    {
        GlobalEventHandler.AddListener(EventID.EVENT_ON_TIGER_TURN, Callback_On_Tiger_Turn);
        GlobalEventHandler.AddListener(EventID.EVENT_REQUEST_TO_KILL_TURN_TIMER_TWEENING, Callback_On_Kill_Timer_Tweening_Requested);
        GlobalEventHandler.AddListener(EventID.EVENT_ON_GOAT_TURN, Callback_On_Goat_Turn);
        GlobalEventHandler.AddListener(EventID.EVENT_ON_LEVEL_STARTED, Callback_On_Level_Started);
    }
    private void OnDisable()
    {
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_TIGER_TURN, Callback_On_Tiger_Turn);
        GlobalEventHandler.RemoveListener(EventID.EVENT_REQUEST_TO_KILL_TURN_TIMER_TWEENING, Callback_On_Kill_Timer_Tweening_Requested); ;
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_GOAT_TURN, Callback_On_Goat_Turn);
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_LEVEL_STARTED, Callback_On_Level_Started);
    }
    private void Start()
    {
        UpdateGoatsKilledByTigerCountText();
        UpdateGoatsPlacedOnTheScreenCountText();
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SHOW_BANNER_AD_REQUESTED);
    }
    #endregion Unity Methods

    #region Public Methods 
    public void OnClickSettingsBtn()
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_CHANGE_SCREEN_REQUESTED, new ScreenChangeProperties(Window.SettingsScreen, ScreenType.Additive));
    }
    #endregion Public Methods 

    #region Private Methods 
    private void SetupTigerTurn()
    {
        if (GameplayManager.GetCurrentGameState().Equals(GameState.Ended)) return;
        goatTimerBar.fillAmount = 1;
        goatUiEffect.effectMode = EffectMode.Grayscale;
        tigerUiEffect.effectMode = EffectMode.None;
        tigerTimerBar.DOFillAmount(0, Constants.TIMER_FOR_TURN_CHANGE).SetRecyclable(true).onComplete += () =>
        {
            GlobalEventHandler.TriggerEvent(EventID.EVENT_REQUEST_TO_CHANGE_PLAYER_TURN);
        };
    }
    private void SetupGoatTurn()
    {
        if (GameplayManager.GetCurrentGameState().Equals(GameState.Ended)) return;
        tigerTimerBar.fillAmount = 1;
        tigerUiEffect.effectMode = EffectMode.Grayscale;
        goatUiEffect.effectMode = EffectMode.None;
        goatTimerBar.DOFillAmount(0, Constants.TIMER_FOR_TURN_CHANGE).SetRecyclable(true).onComplete += () =>
        {
            GlobalEventHandler.TriggerEvent(EventID.EVENT_REQUEST_TO_CHANGE_PLAYER_TURN);
        };
    }

    private void UpdateGoatsPlacedOnTheScreenCountText()
    {
        goatsPlacedCountTxt.text = (Constants.NUMBER_OF_GOATS_IN_THE_GAME - GameplayManager.GetGoatsOnTheBoard()).ToString();
    }
    private void UpdateGoatsKilledByTigerCountText()
    {
        goatsKilledCountTxt.text = $"{Mathf.Clamp(GameplayManager.GetDeadGoatsCount(), 0, Constants.MINIMUM_NUMBER_OF_GOATS_SHOULD_KILL_FOR_TIGERS_WIN)} / {Constants.MINIMUM_NUMBER_OF_GOATS_SHOULD_KILL_FOR_TIGERS_WIN}";
    }
    #endregion Private Methods 

    #region Callbacks
    private void Callback_On_Goat_Turn(object args)
    {
        SetupGoatTurn();
        UpdateGoatsKilledByTigerCountText();
    }
    private void Callback_On_Tiger_Turn(object args)
    {
        SetupTigerTurn();
        UpdateGoatsPlacedOnTheScreenCountText();
    }
    private void Callback_On_Kill_Timer_Tweening_Requested(object args)
    {
        goatTimerBar.DOKill();
        tigerTimerBar.DOKill();
        SovereignUtils.Log($"### KILLED Tweens");
    }
    private void Callback_On_Level_Started(object args)
    {
        UpdateGoatsPlacedOnTheScreenCountText();
        UpdateGoatsKilledByTigerCountText();
    }
    #endregion Callbacks
}

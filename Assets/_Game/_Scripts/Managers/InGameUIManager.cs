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

    [SerializeField] private SpriteManager spritesManager;
    [Space(10)]
    [Header("Player 1")]
    [SerializeField] private UnityEngine.UI.Image player1AnimalHolder;
    [SerializeField] private UIEffect player1UiEffect;
    [SerializeField] private UnityEngine.UI.Image player1TimerBar;
    [SerializeField] private TextMeshProUGUI player1CountText;
    [Space(5)]
    [Header("Player 2")]
    [SerializeField] private UnityEngine.UI.Image player2AnimalHolder;
    [SerializeField] private UIEffect player2UiEffect;
    [SerializeField] private UnityEngine.UI.Image player2TimerBar;
    [SerializeField] private TextMeshProUGUI player2CountTxt;

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
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_CHANGE_SCREEN_REQUESTED, new ScreenChangeProperties(Window.SettingsScreen, ScreenType.Additive, false));
    }
    #endregion Public Methods 

    #region Private Methods 
    private void SetupPlayer1Turn()
    {
        if (GameplayManager.GetCurrentGameState().Equals(GameState.Ended)) return;
        player2TimerBar.fillAmount = 1;
        player2UiEffect.effectMode = EffectMode.Grayscale;
        player1UiEffect.effectMode = EffectMode.None;
        player1TimerBar.DOFillAmount(0, Constants.TIMER_FOR_TURN_CHANGE).SetRecyclable(true).onComplete += () =>
        {
            GlobalEventHandler.TriggerEvent(EventID.EVENT_REQUEST_TO_CHANGE_PLAYER_TURN);
        };
    }
    private void SetupPlayer2Turn()
    {
        if (GameplayManager.GetCurrentGameState().Equals(GameState.Ended)) return;
        player1TimerBar.fillAmount = 1;
        player1UiEffect.effectMode = EffectMode.Grayscale;
        player2UiEffect.effectMode = EffectMode.None;
        player2TimerBar.DOFillAmount(0, Constants.TIMER_FOR_TURN_CHANGE).SetRecyclable(true).onComplete += () =>
        {
            GlobalEventHandler.TriggerEvent(EventID.EVENT_REQUEST_TO_CHANGE_PLAYER_TURN);
        };
    }

    private void UpdateGoatsPlacedOnTheScreenCountText()
    {
        player2CountTxt.text = (Constants.NUMBER_OF_GOATS_IN_THE_GAME - GameplayManager.GetGoatsOnTheBoard()).ToString();
    }
    private void UpdateGoatsKilledByTigerCountText()
    {
        player1CountText.text = $"{Mathf.Clamp(GameplayManager.GetDeadGoatsCount(), 0, Constants.MINIMUM_NUMBER_OF_GOATS_SHOULD_KILL_FOR_TIGERS_WIN)} / {Constants.MINIMUM_NUMBER_OF_GOATS_SHOULD_KILL_FOR_TIGERS_WIN}";
    }
    #endregion Private Methods 

    #region Callbacks
    private void Callback_On_Goat_Turn(object args)
    {
        SetupPlayer2Turn();
        UpdateGoatsKilledByTigerCountText();
    }
    private void Callback_On_Tiger_Turn(object args)
    {
        SetupPlayer1Turn();
        UpdateGoatsPlacedOnTheScreenCountText();
    }
    private void Callback_On_Kill_Timer_Tweening_Requested(object args)
    {
        player2TimerBar.DOKill();
        player1TimerBar.DOKill();
        SovereignUtils.Log($"### KILLED Tweens");
    }
    private void Callback_On_Level_Started(object args)
    {
        UpdateGoatsPlacedOnTheScreenCountText();
        UpdateGoatsKilledByTigerCountText();
    }
    #endregion Callbacks
}

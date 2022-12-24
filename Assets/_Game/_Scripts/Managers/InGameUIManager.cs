using Coffee.UIEffects;
using SovereignStudios;
using UnityEngine;
using DG.Tweening;
using TMPro;

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
    }
    private void OnDisable()
    {
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_TIGER_TURN, Callback_On_Tiger_Turn);
        GlobalEventHandler.RemoveListener(EventID.EVENT_REQUEST_TO_KILL_TURN_TIMER_TWEENING, Callback_On_Kill_Timer_Tweening_Requested); ;
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_GOAT_TURN, Callback_On_Goat_Turn);
    }
    private void Start()
    {
        UpdateGoatsKilledByTigerCountText();
        UpdateGoatsPlacedOnTheScreenCountText();
    }
    #endregion Unity Methods

    #region Public Methods 

    #endregion Public Methods 

    #region Private Methods 
    private void SetupTigerTurn()
    {
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
        goatsKilledCountTxt.text = $"{Mathf.Clamp(GameplayManager.GetDeadGoatsCount(), 0, Constants.MINIMUM_NUMBER_OF_GOATS_SHOULD_FOR_TIGERS_WIN)} / {Constants.MINIMUM_NUMBER_OF_GOATS_SHOULD_FOR_TIGERS_WIN}";
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

    #endregion Callbacks
}

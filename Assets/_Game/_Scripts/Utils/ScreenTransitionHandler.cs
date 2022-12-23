using DG.Tweening;
using SovereignStudios;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransitionHandler : MonoBehaviour
{
    #region Variables
    [SerializeField] private UnityEngine.UI.Image screenTransitionBg;
    [SerializeField] private CanvasGroup leavesCanvasGroup;
    [SerializeField] private List<DOTweenAnimation> leavesAnimatorList;
    private bool isAnimating = false;
    private Window window = default;
    #endregion Variables

    #region Unity Methods
    private void OnEnable()
    {
        GlobalEventHandler.AddListener(EventID.EVENT_ON_SCREEN_TRANSITION_REQUESTED, Callback_On_Screen_Transition_Effect_Requested);
        GlobalEventHandler.AddListener(EventID.EVENT_REQUEST_TO_CHANGE_SCREEN_WITH_TRANSITION, Callback_On_Change_Screen_With_Transition_Requested);
    }
    private void OnDisable()
    {
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SCREEN_TRANSITION_REQUESTED, Callback_On_Screen_Transition_Effect_Requested);
        GlobalEventHandler.RemoveListener(EventID.EVENT_REQUEST_TO_CHANGE_SCREEN_WITH_TRANSITION, Callback_On_Change_Screen_With_Transition_Requested);
    }
    #endregion Unity Methods

    #region Private Methods

    private void ChangeScreenWithTransition()
    {
        isAnimating = true;
        leavesCanvasGroup.DOFade(1, 0.25f);
        screenTransitionBg.DOFade(1, 0.75f);
        for (int i = 0, count = leavesAnimatorList.Count; i < count; i++)
        {
            leavesAnimatorList[i].DOPlay();
            if (i == count - 1)
            {
                Invoke(nameof(ChangeScreen), 1.1f);
                Invoke(nameof(UncoverTheScreenWithEffect), 1.45f);
            }
        }


    }
    private void CoverTheScreenWithEffect()
    {
        Kill();
        isAnimating = true;
        leavesCanvasGroup.DOFade(1, 0.25f);
        screenTransitionBg.DOFade(1, 0.75f);
        for (int i = 0, count = leavesAnimatorList.Count; i < count; i++)
        {
            leavesAnimatorList[i].DOPlayForward();
        }
    }
    private void ChangeScreen()
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_CHANGE_SCREEN_REQUESTED, new System.Tuple<Window, ScreenType, bool, System.Action>(window, ScreenType.Replace, true, () =>
        {
            window = Window.None;
        }));
    }
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.O))
    //        CoverTheScreenWithEffect();
    //    if (Input.GetKeyUp(KeyCode.O))
    //        UncoverTheScreenWithEffect();
    //}
    private void UncoverTheScreenWithEffect()
    {
        Kill();
        screenTransitionBg.DOFade(0, 0.25f);
        isAnimating = false;
        for (int i = 0, count = leavesAnimatorList.Count; i < count; i++)
        {
            leavesAnimatorList[i].DOPlayBackwards();
        }
        SovereignUtils.Log($"!!! After for loop");
        leavesCanvasGroup.DOFade(0, 1f).SetDelay(1.1f);
    }
    private void Kill()
    {
        for (int i = 0, count = leavesAnimatorList.Count; i < count; i++)
            DOTween.Kill(leavesAnimatorList[i]);
        DOTween.Kill(leavesCanvasGroup);
        DOTween.Kill(screenTransitionBg);
    }
    #endregion Private Methods

    #region Callbacks

    private void Callback_On_Screen_Transition_Effect_Requested(object args)
    {
        bool canCoverScreen = (bool)args;
        if (canCoverScreen) CoverTheScreenWithEffect();
        else UncoverTheScreenWithEffect();
    }
    private void Callback_On_Change_Screen_With_Transition_Requested(object args)
    {
        SovereignUtils.Log($"!!Changing screen with transittoe ");
        window = (Window)args;
        if (isAnimating || window == Window.None) return;
        ChangeScreenWithTransition();
    }

    #endregion Callbacks
}

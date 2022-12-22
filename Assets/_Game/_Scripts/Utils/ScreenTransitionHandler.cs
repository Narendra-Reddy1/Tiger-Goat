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

    #endregion Variables

    #region Unity Methods
    private void OnEnable()
    {
        GlobalEventHandler.AddListener(EventID.EVENT_ON_SCREEN_TRANSITION_REQUESTED, Callback_On_Screen_Transition_Effect_Requested);
    }
    private void OnDisable()
    {
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SCREEN_TRANSITION_REQUESTED, Callback_On_Screen_Transition_Effect_Requested);
    }
    #endregion Unity Methods

    #region Private Methods
    private void CoverTheScreenWithEffect()
    {
        leavesCanvasGroup.DOFade(1, 0.2f);
        screenTransitionBg.DOFade(1, 0.75f);
        for (int i = 0, count = leavesAnimatorList.Count; i < count; i++)
            leavesAnimatorList[i].DOPlay();
    }
    private void UncoverTheScreenWithEffect()
    {
        screenTransitionBg.DOFade(0, 0.45f);
        for (int i = 0, count = leavesAnimatorList.Count; i < count; i++)
            leavesAnimatorList[i].DOPlayBackwards();
        leavesCanvasGroup.DOFade(1, 3f);
    }
    #endregion Private Methods

    #region Callbacks

    private void Callback_On_Screen_Transition_Effect_Requested(object args)
    {
        bool show = (bool)args;
        if (show)
            CoverTheScreenWithEffect();
        else
            UncoverTheScreenWithEffect();
    }

    #endregion Callbacks
}

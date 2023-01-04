using UnityEngine;
using DG.Tweening;
using SovereignStudios.EventSystem;

public class PopupBase : WindowBase
{

    [SerializeField] private Transform popupPanel;
    [SerializeField] private CanvasGroup transparentBGCanvasGroup;
    private CanvasGroup popupPanelCanvasGroup;



    #region UnityMethods
    public virtual void OnEnable()
    {
        if (popupPanel.GetComponent<CanvasGroup>() != null)
        {
            popupPanelCanvasGroup = popupPanel.GetComponent<CanvasGroup>();
            StartAnimation();
        }
        else
        {
            Debug.LogError("ComponentMissing : CanvasGroup component missing on popupPanel" + this.gameObject.name);
        }
    }
    public virtual void OnDisable()
    {
    }

    #endregion UnityMethods

    #region PublicMethods
    public override void EndAnimation(System.Action OnAnimationComplete)
    {
        var sequence = DOTween.Sequence();
        //sequence.Join(popupPanel.DOLocalMoveY(250, 0.25f).SetEase(Ease.InBack));
        sequence.Join(popupPanel.DOScale(0, 0.25f).SetEase(Ease.InBack));
        sequence.Join(popupPanelCanvasGroup.DOFade(0, 0.25f).SetEase(Ease.InBack));
        sequence.Join(transparentBGCanvasGroup?.DOFade(0, 0.25f).SetEase(Ease.InBack));
        sequence.SetUpdate(true);
        sequence.OnComplete(() =>
        {
            OnAnimationComplete();
        });
    }

    public override void StartAnimation()
    {
        //popupPanel.localPosition = new Vector3(0, 250, 0);
        popupPanel.localScale = Vector3.zero;
        popupPanelCanvasGroup.alpha = 0;
        transparentBGCanvasGroup.alpha = 0;

        var sequence = DOTween.Sequence();
        //sequence.Join(popupPanel.DOLocalMoveY(0, 0.4f).SetEase(Ease.OutBack));
        sequence.Join(popupPanel.DOScale(1, 0.4f).SetEase(Ease.OutBack));
        sequence.Join(popupPanelCanvasGroup.DOFade(1, 0.4f).SetEase(Ease.OutBack));
        sequence.Join(transparentBGCanvasGroup?.DOFade(1, 0.3f).SetEase(Ease.OutBack));
        sequence.SetUpdate(true);
    }

    public override void OnCloseClick()
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_CLOSE_LAST_ADDITIVE_SCREEN);
    }

    #endregion PublicMethods

}

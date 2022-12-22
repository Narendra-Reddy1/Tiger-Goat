using SovereignStudios;
using TMPro;
using UnityEngine;

public class GenericPopup : PopupBase
{

    #region Variables
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private TextMeshProUGUI messageTxt;
    [SerializeField] private CustomButton actionBtn;

    #endregion Variables

    #region Unity Methods
    public override void OnEnable()
    {
        base.OnEnable();
        GlobalEventHandler.AddListener(EventID.EVENT_ON_SETUP_GENERIC_POPUP_REQUESTED, Callback_On_Setup_Generic_Popup_Requested);
    }
    public override void OnDisable()
    {
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SETUP_GENERIC_POPUP_REQUESTED, Callback_On_Setup_Generic_Popup_Requested);
    }

    #endregion Unity Methods

    #region Public Methods
    public void SetupPopup(string title, string message, UnityEngine.Events.UnityAction onActionBtnClicked)
    {
        titleTxt.SetText(title);
        messageTxt.SetText(message);
        actionBtn.onClick.AddListener(onActionBtnClicked);
    }
    public override void OnCloseClick()
    {
        base.OnCloseClick();
    }

    #endregion Public Methods

    #region Callbacks

    private void Callback_On_Setup_Generic_Popup_Requested(object args)
    {
        System.Tuple<string, string, UnityEngine.Events.UnityAction> tuple = (System.Tuple<string, string, UnityEngine.Events.UnityAction>)args;
        SetupPopup(tuple.Item1, tuple.Item2, tuple.Item3);
    }
    #endregion Callbacks
}

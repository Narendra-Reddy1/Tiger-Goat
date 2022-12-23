using SovereignStudios;
using UnityEngine;
using DG.Tweening;


public class ScreenFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup screenFader;

    private bool isScreenFaderAvailable = true;

    private void OnEnable()
    {
        GlobalEventHandler.AddListener(EventID.EVENT_REQEST_FADE_SCREEN_IN, Callback_On_Fade_Screen_In_Requested);
        GlobalEventHandler.AddListener(EventID.EVENT_REQEST_FADE_SCREEN_OUT, Callback_On_Fade_Screen_Out_Requested);
        GlobalEventHandler.AddListener(EventID.EVENT_REQUEST_SCREEN_BLINK, Callback_On_Fade_Screen_Blink_Requested);
    }

    private void OnDisable()
    {
        GlobalEventHandler.RemoveListener(EventID.EVENT_REQEST_FADE_SCREEN_IN, Callback_On_Fade_Screen_In_Requested);
        GlobalEventHandler.RemoveListener(EventID.EVENT_REQEST_FADE_SCREEN_OUT, Callback_On_Fade_Screen_Out_Requested);
        GlobalEventHandler.RemoveListener(EventID.EVENT_REQUEST_SCREEN_BLINK, Callback_On_Fade_Screen_Blink_Requested);

    }

    private void FadeTheScreeenIn(System.Action onComplete = null)
    {
        if (!isScreenFaderAvailable)
        {
            SovereignUtils.Log($"Screen Fader Unavailable!!", SovereignStudios.LogType.Error);
            return;
        }
        screenFader.blocksRaycasts = true;
        screenFader.DOFade(1, 0.85f).onComplete += () =>
        {
            isScreenFaderAvailable = false;
            onComplete?.Invoke();
        };

    }
    private void FadeTheScreenOut(System.Action onComplete = null)
    {
        screenFader.DOFade(0, 1f).onComplete += () =>
        {
            screenFader.blocksRaycasts = false;
            onComplete?.Invoke();
        };
    }
    private void ShowBlinkScreenEffect()
    {
        if (!isScreenFaderAvailable)
        {
            SovereignUtils.Log($"Screen Fader Unavailable!!", SovereignStudios.LogType.Error);
            return;
        }
        screenFader.blocksRaycasts = true;
        screenFader.DOFade(1, 0.45f).onComplete += () =>
        {
            isScreenFaderAvailable = false;
            screenFader.DOFade(0, 0.35f).onComplete += () =>
            {
                isScreenFaderAvailable = true;
                screenFader.blocksRaycasts = false;
            };
        };

    }

    private void Callback_On_Fade_Screen_Blink_Requested(object args)
    {
        ShowBlinkScreenEffect();
    }
    private void Callback_On_Fade_Screen_Out_Requested(object args)
    {
        System.Tuple<System.Action> tuple = args as System.Tuple<System.Action>;
        FadeTheScreenOut(tuple?.Item1);
    }

    private void Callback_On_Fade_Screen_In_Requested(object args)
    {
        System.Tuple<System.Action> tuple = args as System.Tuple<System.Action>;
        FadeTheScreeenIn(tuple.Item1);
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using SovereignStudios;
using SovereignStudios.EventSystem;
using SovereignStudios.Utils;
using System;

public class DebugPanelHandler : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private GameObject debugPanel;
    [SerializeField] private byte noOfTapsToOpenPanel = 2;
    private byte taps;
    private TimeSpan time;
    private void Update()
    {
        if (Input.touchCount <= 0) return;
        if (Input.GetTouch(0).tapCount == noOfTapsToOpenPanel)
        {
            GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_CHANGE_SCREEN_REQUESTED, new ScreenChangeProperties(Window.ConsoleScreen, ScreenType.Additive));
        }

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        SovereignUtils.Log($"OnPointerClick: {eventData.clickCount}");
        if (Input.GetMouseButtonDown(0))
        {
            if ((DateTime.Now.TimeOfDay.TotalSeconds - time.TotalSeconds) > 1)
            {
                taps = 0;
                time = default;
            }
            time = DateTime.Now.TimeOfDay;
            taps++;
            if (taps >= noOfTapsToOpenPanel)
            {
                SovereignUtils.Log($"showing Console");
                GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_CHANGE_SCREEN_REQUESTED, new ScreenChangeProperties(Window.ConsoleScreen, ScreenType.Additive));
            }
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using SovereignStudios;
using SovereignStudios.EventSystem;
using SovereignStudios.Utils;
using static System.DateTime;

public class DebugPanelHandler : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private GameObject debugPanel;
    [SerializeField] private byte noOfTapsToOpenPanel = 2;
    private byte taps;
    private System.TimeSpan time;
    public void OnPointerClick(PointerEventData eventData)
    {
        SovereignUtils.Log($"OnPointerClick: {eventData.clickCount}");

        if ((Now.TimeOfDay.TotalSeconds - time.TotalSeconds) > 1)
        {
            taps = 0;
            time = default;
        }
        time = Now.TimeOfDay;
        taps++;
        if (taps >= noOfTapsToOpenPanel)
        {
            SovereignUtils.Log($"showing Console");
            GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_CHANGE_SCREEN_REQUESTED, new ScreenChangeProperties(Window.ConsoleScreen, ScreenType.Additive));
        }

    }
}

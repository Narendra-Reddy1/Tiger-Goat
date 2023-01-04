using UnityEngine;
using UnityEngine.EventSystems;
using SovereignStudios.Utils;
using UnityEngine.Events;

public class InputPointerHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public UnityEvent PointerClickCallback = default;
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        SovereignUtils.Log($"Pointer down: {this.name}");
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        SovereignUtils.Log($"Pointer up: {this.name}");
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        SovereignUtils.Log($"Pointer click: {this.name}");
        //        var sp = eventData.pointerClick.GetComponent<SpotPoint>();
        //Dictionary<DirectionFace, SpotPoint> spd = new Dictionary<DirectionFace, SpotPoint>();
        //var spd = sp.neighborsDictionary.Values.ToList();
        PointerClickCallback?.Invoke();
    }
}

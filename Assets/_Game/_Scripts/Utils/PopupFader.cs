using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopupFader : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CanvasGroup[] canvasGroups;
    [SerializeField] private float duration;

    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (CanvasGroup canvasGroup in canvasGroups)
            canvasGroup.DOFade(0, duration);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (CanvasGroup canvasGroup in canvasGroups)
            canvasGroup.DOFade(1, duration);
    }
}

using SovereignStudios.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisableOnEnable : MonoBehaviour
{
    [SerializeField] private float delay = 1f;
    private void OnEnable()
    {
        SovereignUtils.DelayedCallback(delay, Disable);
    }
    private void Disable()
    {
        gameObject.SetActive(false);
    }
}

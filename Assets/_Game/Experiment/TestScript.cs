using SovereignStudios;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestScript : MonoBehaviour
{
    public RectTransform tiger;
    public RectTransform targetPos;
    public GameObject tigerAtTargetPos;
    public Vector3 myPrevPos;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            myPrevPos = tiger.position;
            tiger.DOMove(targetPos.position, 1f, true).onComplete += () =>
            {
                tigerAtTargetPos.SetActive(true);
                tiger.DOMove(myPrevPos, 0);
                tiger.gameObject.SetActive(false);
            };
        }
    }
}
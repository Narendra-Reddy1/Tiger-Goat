using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class LineRendererWrapper : MonoBehaviour
{
    [SerializeField] private List<Transform> pointsList;
    [SerializeField] private UILineRenderer uiLinerenderer;
    [SerializeField] private Canvas mainCanvas;

    private Vector2 rectPos;
    private List<Vector2> pointsPositions = new List<Vector2>();

    private void Start()
    {
        if (mainCanvas == null) mainCanvas = GameObject.FindGameObjectWithTag("Main Canvas").GetComponent<Canvas>();
        rectPos = GetComponent<RectTransform>().position;
        foreach (Transform t in pointsList)
        {
            pointsPositions.Add(new Vector2(t.localPosition.x, t.localPosition.y));
        }
        uiLinerenderer.Points = pointsPositions.ToArray();
        uiLinerenderer.SetAllDirty();
    }

    private void OnDrawGizmos()
    {
        if (mainCanvas)
            Gizmos.matrix = mainCanvas.transform.localToWorldMatrix;
        for (int i = 0; i < pointsList.Count - 1; i++)
        {
            Gizmos.DrawLine(pointsList[i].localPosition, pointsList[i + 1].localPosition);
        }
    }
}

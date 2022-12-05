using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class ExperimentClass : MonoBehaviour
{
    public List<Transform> pointsList;
    public List<Vector2> pointsPositions;
    public UILineRenderer uiLinerenderer;
    public Canvas mainCanvas;
    public Vector2 rectPos;


    private void Start()
    {
        rectPos = GetComponent<RectTransform>().position;
        foreach (Transform t in pointsList)
        {
            //  var rt = t.GetComponent<RectTransform>();
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
            //Vector2 p1 = new Vector2(pointsList[i].position.x - mainCanvas.GetComponent<RectTransform>().rect.position.x,
            //    pointsList[i].position.y - mainCanvas.GetComponent<RectTransform>().rect.position.y);
            //Vector2 p2 = new Vector2(pointsList[i + 1].position.x - mainCanvas.GetComponent<RectTransform>().rect.position.x,
            //    pointsList[i + 1].position.y - mainCanvas.GetComponent<RectTransform>().rect.position.y);

            Gizmos.DrawLine(pointsList[i].localPosition, pointsList[i + 1].localPosition);
            // Gizmos.DrawLine(p1, p2);
        }
    }
}

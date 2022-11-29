using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentClass : MonoBehaviour
{
    public Transform lineStartTransform;
    public Transform lineEndTransform;
    public LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer.enabled = true;
        lineRenderer.startColor = Color.red;
        lineRenderer.SetPosition(0, lineStartTransform.position);
        lineRenderer.SetPosition(1, lineEndTransform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(lineStartTransform.position, lineEndTransform.position);
    }
}

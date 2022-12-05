using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiLineRenderer : Graphic
{

    #region Variables

    public Vector2Int gridSize = new Vector2Int(10, 9);
    public List<Vector2> points;

    public float width;
    public float height;
    public float unitHeight;
    public float unitWidth;

    public float thickness = 10f;


    #endregion Variables

    #region Unity Methods

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        unitWidth = width / (float)gridSize.x;
        unitHeight = height / (float)gridSize.y;
        if (points.Count < 2) return;
        for (int i = 0; i < points.Count; i++)
        {
            DrawVerticesForPoint(points[i], vh);
        }
        for (int i = 0; i < points.Count - 1; i++)
        {
            int index = i * 2;
            vh.AddTriangle(index + 0, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index + 0);
        }
    }
    #endregion UnityMethods

    #region Public Methods

    #endregion Public Methods
    public void DrawVerticesForPoint(Vector2 point, VertexHelper vh)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;
        vertex.position = new Vector3(-thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);
        vertex.position = new Vector3(thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);
    }

    #region Private Methods

    #endregion Private Methods

    #region Callbacks

    #endregion Callbacks

}

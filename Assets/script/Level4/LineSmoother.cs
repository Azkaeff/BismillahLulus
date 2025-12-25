using UnityEngine;
using System.Collections.Generic;

public class LineSmoother : MonoBehaviour
{
    public float minDistance = 0.05f;

    private LineRenderer lr;
    private List<Vector3> points = new();

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void AddPoint(Vector3 point)
    {
        if (points.Count > 0 &&
            Vector3.Distance(points[^1], point) < minDistance)
            return;

        points.Add(point);
        lr.positionCount = points.Count;
        lr.SetPositions(points.ToArray());
    }

    public void Clear()
    {
        points.Clear();
        lr.positionCount = 0;
    }
}

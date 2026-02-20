using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Validates that the pointer stays within tolerance of the target path.
/// Prevents easy disconnection by tracking continuous path following.
/// </summary>
public class PathValidator : MonoBehaviour
{
    [Header("Path Validation Settings")]
    [SerializeField] private float pathFollowTolerance = 0.8f;  // Distance from path centerline
    [SerializeField] private float minPathProgressRequired = 0.3f;  // Minimum % of path to trace
    [SerializeField] private float recoveryTime = 0.5f;  // Time to recover after leaving path
    
    private EdgeCollider2D pathCollider;
    private float pathProgress = 0f;  // 0-1 progress along path
    private float timeOffPath = 0f;
    private bool canRecover = true;
    private List<Vector2> pathPoints = new List<Vector2>();
    private int lastValidSegment = 0;

    void Start()
    {
        pathCollider = GetComponent<EdgeCollider2D>();
        if (pathCollider != null && pathCollider.points.Length > 0)
        {
            pathPoints = new List<Vector2>(pathCollider.points);
        }
    }

    void Update()
    {
        if (!canRecover)
        {
            timeOffPath += Time.deltaTime;
            if (timeOffPath >= recoveryTime)
            {
                canRecover = true;
                timeOffPath = 0f;
            }
        }
    }

    /// <summary>
    /// Check if pointer position is following the path correctly
    /// </summary>
    public bool IsFollowingPath(Vector2 pointerPos)
    {
        if (pathPoints.Count < 2)
            return true;

        // Find closest point on path
        float closestDistance = float.MaxValue;
        int closestIndex = 0;

        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            float distance = DistanceToLineSegment(pointerPos, pathPoints[i], pathPoints[i + 1]);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        // Check if within tolerance and progressing forward
        bool withinTolerance = closestDistance <= pathFollowTolerance;
        bool isProgressing = closestIndex >= lastValidSegment;

        if (withinTolerance && isProgressing)
        {
            lastValidSegment = closestIndex;
            pathProgress = (float)closestIndex / (pathPoints.Count - 1);
            timeOffPath = 0f;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Check if player has completed enough of the path
    /// </summary>
    public bool HasSufficientProgress()
    {
        return pathProgress >= minPathProgressRequired;
    }

    /// <summary>
    /// Distance from point to line segment
    /// </summary>
    private float DistanceToLineSegment(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
    {
        Vector2 lineVec = lineEnd - lineStart;
        Vector2 pointVec = point - lineStart;
        
        float lineLen = lineVec.magnitude;
        if (lineLen == 0)
            return pointVec.magnitude;

        float t = Mathf.Clamp01(Vector2.Dot(pointVec, lineVec) / (lineLen * lineLen));
        Vector2 closest = lineStart + lineVec * t;
        
        return Vector2.Distance(point, closest);
    }

    public float GetPathProgress() => pathProgress;
    public bool CanRecover() => canRecover;
    public void ResetPathTracking()
    {
        pathProgress = 0f;
        lastValidSegment = 0;
        timeOffPath = 0f;
        canRecover = true;
    }
}

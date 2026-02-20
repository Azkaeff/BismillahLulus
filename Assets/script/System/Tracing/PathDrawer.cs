using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PathDrawer : MonoBehaviour
{
    public Path path;
    private LineRenderer myLineRenderer;
    public int MyCurrentNumber;
    
    [Header("Tolerance Settings")]
    [SerializeField] private float colliderEdgeRadius = 0.3f;  // Tolerance for collision

    void Start()
    {
        // PathValidator can be added separately if needed
    }

    public void CreatePath()
    {
        path = new Path(transform.position);

        // Get or create LineRenderer
        myLineRenderer = this.GetComponent<LineRenderer>();
        if (myLineRenderer == null)
        {
            myLineRenderer = this.AddComponent<LineRenderer>();
        }
        
        if (myLineRenderer != null)
        {
            myLineRenderer.widthMultiplier = 0.2f;
        }
    }

    public void DrawPath(List<Vector2> points)
    {
        if(this.GetComponent<EdgeCollider2D>() == null)
        {
            EdgeCollider2D collider = this.gameObject.AddComponent<EdgeCollider2D>();
            collider.edgeRadius = colliderEdgeRadius;  // Apply tolerance
        }
        else
        {
            this.GetComponent<EdgeCollider2D>().edgeRadius = colliderEdgeRadius;
        }
        
        this.GetComponent<EdgeCollider2D>().offset = new Vector2(0f, 0f);
        this.GetComponent<EdgeCollider2D>().points = points.ToArray();

        this.GetComponent<LineRenderer>().positionCount = points.Count;

        for (int i = 0; i < points.Count; i++)
        {
            this.GetComponent<LineRenderer>().SetPosition(i, points[i]);
            this.GetComponent<EdgeCollider2D>().points[i] = new Vector2(points[i].x - 90f, points[i].y - 90f);
        }
        
        // Collider edge radius applied for tolerance
    }

    public void ClearEdgeCollider()
    {
        EdgeCollider2D collider = this.GetComponent<EdgeCollider2D>();
        if(collider != null)
        {
            DestroyImmediate(collider);
        }
    }
}
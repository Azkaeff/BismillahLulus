using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerAlignmentChecker : MonoBehaviour
{
    public GameObject myMask;
    public float spawnThreshold = 0.1f;
    public float alignmentCheckRadius = 0.15f;

    private Vector3 lastMaskPos = Vector3.zero;
    private bool hasSpawnedMask = false;
    private Collider2D pathCollider;

    void Start()
    {
        // Cache collider reference
        pathCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        Vector3 currentPos = TouchMovementHandler.Instance.GetCurrentPointerPosition();
        
        // Gunakan distance-based check (lebih stabil dari trigger)
        bool isInPath = CheckIfInPath(currentPos);
        TouchMovementHandler.Instance.isAligned = isInPath;

        if(TouchMovementHandler.Instance.isAligned && TouchMovementHandler.Instance.PointerGO != null)
        {
            Vector3 lastPos = TouchMovementHandler.Instance.GetLastPointerPosition();
            
            float distance = Vector3.Distance(lastPos, currentPos);
            
            if(distance > spawnThreshold)
            {
                int steps = Mathf.Max(1, (int)(distance / spawnThreshold));
                
                for(int i = 0; i <= steps; i++)
                {
                    float t = steps > 0 ? (float)i / steps : 0;
                    Vector3 spawnPos = Vector3.Lerp(lastPos, currentPos, t);
                    
                    GameObject go = Instantiate(myMask, spawnPos, Quaternion.identity);
                    go.transform.SetParent(GameObject.Find("Masks").transform);
                }
                
                hasSpawnedMask = true;
                lastMaskPos = currentPos;
            }
        }

        if(Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            ClearPointer();
        }
    }

    private bool CheckIfInPath(Vector3 position)
    {
        // Method 1: Gunakan Physics2D.OverlapPoint (lebih akurat)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, alignmentCheckRadius);
        
        foreach(Collider2D col in colliders)
        {
            if(col.CompareTag("myPath"))
            {
                return true;
            }
        }
        
        return false;
    }

    void ClearPointer()
    {
        if(TouchMovementHandler.Instance.PointerGO != null)
        {
            Destroy(TouchMovementHandler.Instance.PointerGO);
            TouchMovementHandler.Instance.PointerGO = null;
        }
        
        hasSpawnedMask = false;
    }
}
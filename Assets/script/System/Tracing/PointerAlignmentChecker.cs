using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerAlignmentChecker : MonoBehaviour
{
    public GameObject myMask;
    
    [Header("Tolerance Settings")]
    [SerializeField] private float alignmentTolerance = 0.5f;
    [SerializeField] private float maxDisconnectDistance = 1.5f;
    
    private EdgeCollider2D pathCollider;
    private Vector2 lastValidPosition;
    private bool hasStartedTracing = false;
    private float timeDisconnected = 0f;
    private const float DISCONNECT_TIME_THRESHOLD = 0.3f;

    void Start()
    {
        pathCollider = GetComponent<EdgeCollider2D>();
        if (pathCollider != null)
        {
            pathCollider.edgeRadius = alignmentTolerance;
        }
    }

    void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

        // Track path continuity
        if(TouchMovementHandler.Instance.isAligned)
        {
            lastValidPosition = pos;
            timeDisconnected = 0f;
            hasStartedTracing = true;
            
            GameObject go = Instantiate(myMask, pos, Quaternion.identity);
            go.transform.SetParent(GameObject.Find("Masks").transform);
        }
        else if(hasStartedTracing)
        {
            // Allow small temporary disconnects with tolerance
            timeDisconnected += Time.deltaTime;
            
            if(timeDisconnected > DISCONNECT_TIME_THRESHOLD ||
               Vector2.Distance(pos, lastValidPosition) > maxDisconnectDistance)
            {
                // Path broken - player drifted too far
                hasStartedTracing = false;
                timeDisconnected = 0f;
            }
        }

        if(Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            DestroyPointer();
            hasStartedTracing = false;
            timeDisconnected = 0f;
        }
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if(collision.gameObject.tag == "myPath")
        {
           TouchMovementHandler.Instance.isAligned = true;
        }
    }

    private void OnTriggerExit2D (Collider2D collision)
    {
        if(collision.gameObject.tag == "myPath")
        {
            if(TouchMovementHandler.Instance.isAligned)
            {
                TouchMovementHandler.Instance.isAligned = false;
            }
        }
    }

    void DestroyPointer()
    {
        if(TouchMovementHandler.Instance.PointerGO != null)
        {
            if(GameObject.Find("Masks").transform.childCount > 0)
            {
                foreach(Transform child in GameObject.Find("Masks").transform)
                {
                    Destroy(child.gameObject);
                }
            }
                
            Destroy(TouchMovementHandler.Instance.PointerGO);
        }
    }
    
    public bool IsContinuouslyTracing => hasStartedTracing;
    public float GetDisconnectTime => timeDisconnected;
}
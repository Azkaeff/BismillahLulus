using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMovementHandler : MonoBehaviour
{
    public static TouchMovementHandler Instance;

    [HideInInspector] public GameObject PointerGO;
    public GameObject PointerPrefab;
    private Vector3 PointerPosition;
    private Plane newPlane;
    private float CalcRayDistance;
    public bool isAligned = false;
    
    [Header("Tolerance Settings")]
    [SerializeField] private float pointerDetectionRadius = 0.3f;
    private Vector3 lastValidPointerPos;
    private bool isPointerMoving = false;

    private void Awake()
    {
        Instance = this;   
    }

    void Start()
    {
        newPlane = new Plane(Camera.main.transform.forward * 0.1f, transform.position);
    }
    private void Update()
    {
        PointerHandle();
    }
    
    void PointerHandle()
    {
        if((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || (Input.GetMouseButtonDown(0)))
        {
            Ray newRay = Camera.main.ScreenPointToRay(Input.mousePosition);
     
            if(newPlane.Raycast(newRay, out CalcRayDistance))
            {
                PointerPosition = newRay.GetPoint(CalcRayDistance);
                lastValidPointerPos = PointerPosition;
                isPointerMoving = true;
                PointerGO = Instantiate(PointerPrefab, PointerPosition, Quaternion.identity);
            }
        }
        else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0))
        {
            Ray newRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if(newPlane.Raycast(newRay, out CalcRayDistance))
            {
                Vector3 newPos = newRay.GetPoint(CalcRayDistance);
                
                if(PointerGO != null)
                {
                    // Smooth pointer movement with detection radius
                    Vector3 direction = (newPos - lastValidPointerPos).normalized;
                    float distance = Vector3.Distance(newPos, lastValidPointerPos);
                    
                    // Allow smooth movement within tolerance
                    if(distance <= pointerDetectionRadius * 2f)
                    {
                        PointerGO.transform.position = newPos;
                        lastValidPointerPos = newPos;
                    }
                    else
                    {
                        // Move incrementally if distance is large
                        PointerGO.transform.position = lastValidPointerPos + direction * (pointerDetectionRadius * 2f);
                        lastValidPointerPos = PointerGO.transform.position;
                    }
                }
            }
        }
    }
}

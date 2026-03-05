using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMovementHandler : MonoBehaviour
{
    public static TouchMovementHandler Instance;

    [HideInInspector] public GameObject PointerGO;
    public GameObject PointerPrefab;
    private Vector3 PointerPosition;
    private Vector3 LastPointerPosition;
    public bool isAligned = false;

    private void Awake()
    {
        Instance = this;   
    }

    void Start()
    {
        LastPointerPosition = Vector3.zero;
    }
    private void Update()
    {
        PointerHandle();
    }
    
    void PointerHandle()
    {
        Vector3 screenPos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;

        if((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || (Input.GetMouseButtonDown(0)))
        {
            PointerPosition = worldPos;
            LastPointerPosition = worldPos;
            PointerGO = Instantiate(PointerPrefab, PointerPosition, Quaternion.identity);
        }
        else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0))
        {
            if(PointerGO != null)
            {
                LastPointerPosition = PointerPosition;
                PointerPosition = worldPos;
                PointerGO.transform.position = PointerPosition;
            }
        }
    }

    public Vector3 GetLastPointerPosition()
    {
        return LastPointerPosition;
    }

    public Vector3 GetCurrentPointerPosition()
    {
        return PointerPosition;
    }
}

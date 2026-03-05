using UnityEngine;

/// <summary>
/// Handle semua input (mouse/touch) dengan consistent 2D world space
/// Responsibility: Input capture dan stroke recording
/// </summary>
public class InputHandler : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private Vector3 currentPointerPosition;
    private Vector3 lastPointerPosition;
    private bool isPointerDown = false;
    
    private void Start()
    {
        if(mainCamera == null)
            mainCamera = Camera.main;
    }
    
    private void Update()
    {
        UpdatePointerPosition();
        UpdateInputState();
    }
    
    private void UpdatePointerPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;
        
        lastPointerPosition = currentPointerPosition;
        currentPointerPosition = worldPos;
    }
    
    private void UpdateInputState()
    {
        // Detect mouse/touch input
        if(Input.GetMouseButtonDown(0) || 
           (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            isPointerDown = true;
        }
        else if(Input.GetMouseButtonUp(0) || 
                (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            isPointerDown = false;
        }
    }
    
    public Vector3 GetCurrentPosition() => currentPointerPosition;
    public Vector3 GetLastPosition() => lastPointerPosition;
    public bool IsPointerDown() => isPointerDown;
    public float GetPointerDelta() => Vector3.Distance(lastPointerPosition, currentPointerPosition);
}

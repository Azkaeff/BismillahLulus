using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 startPosition;
    private Transform startParent;
    private DropPlaceholder currentPlaceholder;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        startPosition = transform.position;
        startParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        startPosition = transform.position;
        startParent = transform.parent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (currentPlaceholder != null)
        {
            // Snap to the placeholder
            transform.position = currentPlaceholder.transform.position;
            transform.SetParent(currentPlaceholder.transform);
        }
        else
        {
            // Return to start position
            transform.position = startPosition;
            transform.SetParent(startParent);
        }

        
        Debug.Log("Dropped item, checking level state...");
        // per level logic
        if (GameManager_LEVEL2.Instance != null)
        {
            Debug.Log("Updating numbers and checking answers for Level 2...");
            GameManager_LEVEL2.Instance.UpdateNumbers();
            GameManager_LEVEL2.Instance.CheckNumbers();
        }
    }

    public void SetPlaceholder(DropPlaceholder placeholder)
    {
        currentPlaceholder = placeholder;
    }

    public void ClearPlaceholder(DropPlaceholder placeholder)
    {
        if (currentPlaceholder == placeholder)
        {
            currentPlaceholder = null;
        }
    }
}

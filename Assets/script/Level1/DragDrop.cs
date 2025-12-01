using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;
    public Vector2 originalPos;
    public Transform dragParent;

    private Canvas canvas;
    private RectTransform rect;
    private CanvasGroup canvasGroup;
    private DropZone nearbySlot;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvas = GetComponentInParent<Canvas>();

        if (dragParent == null && canvas != null)
            dragParent = canvas.transform;

        originalParent = transform.parent;
        originalPos = rect.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;

        originalParent = transform.parent;
        originalPos = rect.anchoredPosition;

        canvasGroup.alpha = 0.6f;
        transform.SetParent(dragParent, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        if (nearbySlot != null)
            nearbySlot.Place(this);
        else
            ReturnToStartImmediate();

        nearbySlot = null;
    }

    public void SetNearbySlot(DropZone slot) => nearbySlot = slot;
    public void ClearNearbySlot() => nearbySlot = null;

    public void ReturnToStartImmediate()
    {
        transform.SetParent(originalParent, false);
        rect.anchoredPosition = originalPos;
    }
}

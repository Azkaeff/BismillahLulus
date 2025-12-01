using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropLevel2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;
    public Vector2 originalPos;
    public Transform dragParent;

    private DropZoneLevel2 nearbySlot;
    private Canvas canvas;
    private RectTransform rect;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvas = GetComponentInParent<Canvas>();
        if (dragParent == null) dragParent = canvas.transform;

        originalParent = transform.parent;
        originalPos = rect.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag dipanggil!");
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;

        originalParent = transform.parent;
        originalPos = rect.anchoredPosition;

        transform.SetParent(dragParent, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        if (nearbySlot != null)
        {
            PlaceToSlot(nearbySlot);
        }
        else
        {
            ReturnToStart();
        }
    }

    public void PlaceToSlot(DropZoneLevel2 slot)
    {
        if (slot.TryPlaceNumber(GetComponent<NumberObject>()))
        {
            FindObjectOfType<GameManager_Level2>().CheckResult();
        }
        else
        {
            ReturnToStart();
        }
        ClearNearbySlot();
    }

    private readonly Vector2 desiredSize = new Vector2(100, 100);
    public void ReturnToStart()
    {
        transform.SetParent(originalParent, false);
        GetComponent<RectTransform>().anchoredPosition = originalPos;

        rect.sizeDelta = desiredSize;
        transform.localScale = Vector3.one;
    }
    public void SetNearbySlot(DropZoneLevel2 slot) => nearbySlot = slot;
    public void ClearNearbySlot() => nearbySlot = null;
}

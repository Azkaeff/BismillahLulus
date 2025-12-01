using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public int correctNumber = -1;
    public bool allowSwap = true;


    private readonly Vector2 desiredSize = new Vector2(120, 120);

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        var drag = eventData.pointerDrag.GetComponent<DragDrop>();
        if (drag != null) drag.SetNearbySlot(this);
    }

   
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        var drag = eventData.pointerDrag.GetComponent<DragDrop>();
        if (drag != null) drag.ClearNearbySlot();
    }

    public void OnDrop(PointerEventData eventData)
    {

    }

    public bool IsOccupied()
    {
        return transform.childCount > 0;
    }

    public DragDrop GetOccupyingDrag()
    {
        if (!IsOccupied()) return null;
        return transform.GetChild(0).GetComponent<DragDrop>();
    }

    public void Place(DragDrop drag)
    {
        if (drag == null) return;

        RectTransform rt = drag.GetComponent<RectTransform>(); // Gunakan rt
        Vector2 desiredSize = new Vector2(100, 100);

        if (!IsOccupied())
        {
            drag.transform.SetParent(transform, false); 
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = desiredSize; 
            rt.localScale = Vector3.one;

            // Update posisi awal baru
            drag.originalParent = transform;
            drag.originalPos = Vector2.zero;
            return;
        }

        if (allowSwap)
        {
            DragDrop existing = GetOccupyingDrag();

            if (existing != null)
            {
                existing.transform.SetParent(drag.originalParent, false);
                existing.GetComponent<RectTransform>().anchoredPosition = existing.originalPos;
                existing.GetComponent<RectTransform>().sizeDelta = desiredSize;
                existing.GetComponent<RectTransform>().localScale = Vector3.one;


                drag.transform.SetParent(transform, false);
                rt.anchoredPosition = Vector2.zero;
                rt.sizeDelta = desiredSize;
                rt.localScale = Vector3.one;

                drag.originalParent = transform;
                drag.originalPos = Vector2.zero;
            }
        }
        else
        {
            drag.ReturnToStartImmediate();
        }
    }

}
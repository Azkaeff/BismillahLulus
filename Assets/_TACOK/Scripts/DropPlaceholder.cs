using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlaceholder : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private DraggableItem draggableHere;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        DraggableItem item = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (item != null)
        {
            draggableHere = item;
            item.SetPlaceholder(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (draggableHere != null)
        {
            draggableHere.ClearPlaceholder(this);
            draggableHere = null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Actual snapping is handled in DraggableItem.OnEndDrag
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

// HANYA IPointerEnterHandler dan IPointerExitHandler yang diperlukan
public class DropZoneLevel2 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int correctNumber;
    public bool allowSwap = true;

    // Sesuaikan nilai ini agar sama dengan Cell Size di komponen Grid Layout Group Anda!
    private readonly Vector2 desiredSize = new Vector2(100, 100);

    // --- FUNGSI POINTER UNTUK MEMBERI TAHU DRAG OBJECT ---

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Pastikan objek yang di-drag memiliki script DragDropLevel2
        if (eventData.pointerDrag == null) return;
        var drag = eventData.pointerDrag.GetComponent<DragDropLevel2>();
        if (drag != null) drag.SetNearbySlot(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Pastikan objek yang di-drag memiliki script DragDropLevel2
        if (eventData.pointerDrag == null) return;
        var drag = eventData.pointerDrag.GetComponent<DragDropLevel2>();
        if (drag != null) drag.ClearNearbySlot();
    }

    // --- LOGIKA PENEMPATAN ---

    public bool TryPlaceNumber(NumberObject num)
    {
        RectTransform numRect = num.GetComponent<RectTransform>();

        // Kasus 1: Slot kosong
        if (transform.childCount == 0)
        {
            num.transform.SetParent(transform, false);
            numRect.anchoredPosition = Vector2.zero;
            numRect.sizeDelta = desiredSize;
            numRect.localScale = Vector3.one; // Pastikan skala normal

            // PENTING: Update info posisi awal di NumberObject (melalui script drag)
            num.GetComponent<DragDropLevel2>().originalParent = transform;
            num.GetComponent<DragDropLevel2>().originalPos = Vector2.zero;
            return true;
        }

        // Kasus 2: Slot terisi, tapi swap tidak diizinkan
        if (!allowSwap) return false;

        // Kasus 3: Slot terisi, dan swap diizinkan
        NumberObject old = transform.GetChild(0).GetComponent<NumberObject>();
        if (old != null)
        {
            // Pindahkan objek lama kembali ke tempat awal
            old.GetComponent<DragDropLevel2>().ReturnToStart();
        }

        // Taruh objek baru
        num.transform.SetParent(transform, false);
        numRect.anchoredPosition = Vector2.zero;
        numRect.sizeDelta = desiredSize;
        numRect.localScale = Vector3.one; // Pastikan skala normal

        // PENTING: Update info posisi awal di NumberObject (melalui script drag)
        num.GetComponent<DragDropLevel2>().originalParent = transform;
        num.GetComponent<DragDropLevel2>().originalPos = Vector2.zero;
        return true;
    }

    // --- LOGIKA CEK KOREKSI ---

    public bool IsCorrect()
    {
        if (transform.childCount == 0) return false;

        // Ambil objek anak secara langsung
        NumberObject placedObject = transform.GetChild(0).GetComponent<NumberObject>();

        if (placedObject == null) return false;

        return placedObject.number == correctNumber;
    }
}
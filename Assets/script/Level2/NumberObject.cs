using UnityEngine;
using TMPro;

public class NumberObject : MonoBehaviour
{
    private Vector2 startAnchoredPosition;
    private Transform parentBeforeDrag;

    public int number;
    public TMP_Text numberText;

    private RectTransform rect;

    public Transform originalParent;
    public bool isInDropZone = false;


    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        parentBeforeDrag = transform.parent;
        originalParent = transform.parent;

        startAnchoredPosition = rect.anchoredPosition;

        if (numberText != null)
            numberText.text = number.ToString();
    }

    public void UpdateStartInfo(Transform newParent, Vector2 newPos)
    {
        parentBeforeDrag = newParent;
        startAnchoredPosition = newPos;
    }

    public void SetNumber(int num)
    {
        number = num;

        if (numberText != null)
            numberText.text = num.ToString();
    }

    public void ReturnToStart()
    {
        transform.SetParent(parentBeforeDrag, false); // FALSE: posisi lokal
        rect.anchoredPosition = startAnchoredPosition;
        isInDropZone = false;
    }

    public Vector2 GetOriginalPosition()
    {
        return startAnchoredPosition;
    }

}

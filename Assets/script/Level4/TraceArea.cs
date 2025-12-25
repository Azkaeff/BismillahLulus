using UnityEngine;
using UnityEngine.UI;
using System;

public class TraceArea : MonoBehaviour
{
    public SimpleDraw simpleDraw;

    [Tooltip("Image used to display the outline/guide for the selected letter/number.")]
    public Image outlineLetter;

    [Tooltip("Parent transform where stroke prefabs will be spawned.")]
    public Transform strokeContainer;

    private StrokeData currentStroke;
    private Action onStrokeComplete;
    private bool started;

    public void Setup(LetterData letter)
    {
        if (outlineLetter != null)
        {
            // Use outlineSprite if provided, otherwise fall back to previewSprite
            outlineLetter.sprite = letter.outlineSprite != null ? letter.outlineSprite : letter.previewSprite;
            outlineLetter.enabled = outlineLetter.sprite != null;
        }

        // Clear existing stroke visual children
        if (strokeContainer != null)
        {
            foreach (Transform child in strokeContainer)
                Destroy(child.gameObject);
        }

        // Spawn stroke prefabs if provided on each StrokeData
        if (letter.strokes != null && strokeContainer != null)
        {
            foreach (StrokeData stroke in letter.strokes)
            {
                if (stroke != null && stroke.prefab != null)
                {
                    GameObject go = Instantiate(stroke.prefab, strokeContainer);
                    go.transform.localPosition = Vector3.zero;
                }
            }
        }
    }

    public void StartStroke(StrokeData stroke, Action onComplete)
    {
        currentStroke = stroke;
        onStrokeComplete = onComplete;
        started = false;
        if (simpleDraw != null)
            simpleDraw.Clear();
    }

    void Update()
    {
        if (currentStroke == null) return;

        if (simpleDraw == null) return;

        Vector2 pos = simpleDraw.CurrentDrawPosition;

        if (!started)
        {
            if (currentStroke.startPoint != null && Vector2.Distance(pos, currentStroke.startPoint.position) <= currentStroke.allowedRadius)
            {
                started = true;
            }
        }
        else
        {
            if (currentStroke.endPoint != null && Vector2.Distance(pos, currentStroke.endPoint.position) <= currentStroke.allowedRadius)
            {
                currentStroke = null;
                onStrokeComplete?.Invoke();
            }
        }
    }
}

using UnityEngine;
using System;

public class TraceManager : MonoBehaviour
{
    public TraceArea traceArea;
    public CompleteLetterPanel completeLetterPanel;

    private LetterData currentLetter;
    private int strokeIndex;
    private Action onLetterComplete;

    public void StartLetter(LetterData letter, Action onComplete)
    {
        currentLetter = letter;
        onLetterComplete = onComplete;
        strokeIndex = 0;
        // Prepare the tracing area with the selected letter (outline and stroke prefabs)
        if (traceArea != null)
            traceArea.Setup(currentLetter);

        StartStroke();
    }

    void StartStroke()
    {
        traceArea.StartStroke(
            currentLetter.strokes[strokeIndex],
            OnStrokeFinished
        );
    }

    void OnStrokeFinished()
    {
        strokeIndex++;

        if (strokeIndex >= currentLetter.strokes.Count)
        {
            // CompleteLetterPanel.Show expects (Sprite, Action, string)
            completeLetterPanel.Show(
                currentLetter.previewSprite,
                FinishLetter,
                currentLetter.letterName
            );
        }
        else
        {
            StartStroke();
        }
    }

    void FinishLetter()
    {
        completeLetterPanel.Hide();
        onLetterComplete?.Invoke();
    }

    public void ResetLetter()
    {
        strokeIndex = 0;
        traceArea.simpleDraw.Clear();
    }
}

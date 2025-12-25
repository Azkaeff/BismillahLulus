using UnityEngine;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    public StageInfo stageInfo;
    public TraceManager traceManager;
    public CompleteStagePanel completeStagePanel;
    public HelpPanel helpPanel;
    public CompleteLetterPanel completeLetterPanel;
    public LetterButton[] letterButtons;


    private Dictionary<LetterData, LetterButton> letterMap = new();
    private HashSet<LetterData> completedLetters = new();
    private LetterData currentLetter;

    void Start()
    {
        SetupStage();
        if (helpPanel != null) helpPanel.Show(); // optional initial help
    }

    void SetupStage()
    {
        if (stageInfo == null)
        {
            Debug.LogError("StageManager: stageInfo is not assigned.");
            return;
        }

        // If explicit letterButtons assigned in inspector, use them; otherwise find in scene
        if (letterButtons == null || letterButtons.Length == 0)
        {
            letterButtons = FindObjectsOfType<LetterButton>();
        }

        int count = Mathf.Min(letterButtons.Length, stageInfo.letters.Count);
        for (int i = 0; i < count; i++)
        {
            var ld = stageInfo.letters[i];
            var btn = letterButtons[i];
            // wire the button to select this letter
            btn.Setup(ld, () => SelectLetter(ld, btn));
            letterMap[ld] = btn;
        }
    }

    public void SelectLetter(LetterData letter, LetterButton button)
    {
        if (letter == null || traceManager == null) return;
        currentLetter = letter;
        traceManager.StartLetter(letter, () =>
        {
            completedLetters.Add(letter);
            button.SetCompleted(true);

            if (completedLetters.Count >= stageInfo.letters.Count)
                CompleteStage();
        });
    }
    


    void CompleteStage()
    {
        if (completeStagePanel != null)
        {
            int stars = stageInfo != null ? stageInfo.letters.Count : 0;
            completeStagePanel.Show(stars, ReplayStage, GoToNextStage);
        }
    }

    // Called by TraceManager when a letter finishes (alternative path)
    public void OnLetterCompleted()
    {
        if (currentLetter != null)
        {
            completedLetters.Add(currentLetter);
            if (letterMap.TryGetValue(currentLetter, out var btn))
            {
                btn.SetCompleted(true);
            }

            if (completedLetters.Count >= stageInfo.letters.Count)
            {
                CompleteStage();
            }
        }
    }
    void ReplayStage()
    {
        // Clear runtime completion state
        completedLetters.Clear();

        // Reset UI
        foreach (var btn in letterButtons)
            btn.SetCompleted(false);

        // Reset trace manager state
        traceManager.ResetLetter();
    }

    void GoToNextStage()
    {
        Debug.Log("LOAD STAGE BERIKUTNYA");
        // nanti: load data stage berikut
    }

}

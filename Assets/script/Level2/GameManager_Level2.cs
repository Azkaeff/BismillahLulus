using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_LEVEL2 : MonoBehaviour
{
    public static GameManager_LEVEL2 Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<int> answerNumbers = new List<int> { 1, 2, 3, 4, 5 };
    public List<int> userNumbers = new List<int> { 1, -1, 3, -1, 5 };

    public GameObject numberQuestions;
    // Reference to the level completion manager (handles showing the popup and buttons)
    public LevelCompleteManager levelCompleteManager;

    // get all children under numberQuestions
    // get their Number component, if there's no Number component
    // get it from their children
    public void UpdateNumbers()
    {
        if (numberQuestions == null) return;

        int index = 0;
        foreach (Transform child in numberQuestions.transform)
        {
            Number number = child.GetComponentInChildren<Number>(); // recursive on this child's subtree
            if (number != null)
            {
                // Use index of child, so userNumbers maps by child position
                if (index < userNumbers.Count)
                {
                    userNumbers[index] = number.value;
                    Debug.Log($"Updated userNumbers[{index}] to {number.value}");
                }
                else
                {
                    Debug.LogWarning($"More Number components than entries in userNumbers: index {index}");
                }
            }
            else
            {
                Debug.Log($"No Number found under child {child.name} (index {index})");
            }
            index++;
        }
    }

    public void CheckNumbers()
    {
        // Ensure we have the latest user input values
        UpdateNumbers();
        bool anyEmpty = false;
        bool anyWrong = false;
        var wrongIndices = new System.Collections.Generic.List<int>();

        int count = Mathf.Min(userNumbers.Count, answerNumbers.Count);
        for (int i = 0; i < count; i++)
        {
            int userVal = userNumbers[i];
            int answerVal = answerNumbers[i];

            if (userVal == -1)
            {
                anyEmpty = true;
                continue;
            }

            if (userVal != answerVal)
            {
                anyWrong = true;
                wrongIndices.Add(i);
            }
        }

        // If there are empty slots, hide all outlines and prompt
        if (anyEmpty)
        {
            Debug.Log("Level 2: some slots empty");
            HideAllOutlines();
            return;
        }

        // If there are wrong slots, show outlines on those slots
        if (anyWrong)
        {
            Debug.Log("Level 2: some answers wrong, showing outlines");
            ShowOutlinesForIndices(wrongIndices);
            return;
        }

        // All correct
        Debug.Log("Level 2 Completed!");
        HideAllOutlines();
        if (levelCompleteManager != null)
        {
            levelCompleteManager.ShowLevelComplete();
        }
        else
        {
            LevelCompleteManager manager = FindObjectOfType<LevelCompleteManager>();
            if (manager != null) manager.ShowLevelComplete();
            else Debug.LogWarning("LevelCompleteManager not found. Create/assign one to show the level complete popup.");
        }
    }

    private void HideAllOutlines()
    {
        if (numberQuestions == null) return;
        int idx = 0;
        foreach (Transform child in numberQuestions.transform)
        {
            // Prefer DropPlaceholder/Draggable pattern: look for a child named "Outline"
            var placeholder = child.GetComponent<DropPlaceholder>() ?? child.GetComponentInChildren<DropPlaceholder>();
            GameObject outlineObj = null;
            if (placeholder != null)
            {
                var t = placeholder.transform.Find("Outline");
                if (t != null) outlineObj = t.gameObject;
            }

            // Fallback: look directly under the slot for an "Outline" child
            if (outlineObj == null)
            {
                var t2 = child.Find("Outline");
                if (t2 != null) outlineObj = t2.gameObject;
            }

            if (outlineObj != null)
                outlineObj.SetActive(false);
            idx++;
        }
    }

    private void ShowOutlinesForIndices(System.Collections.Generic.List<int> indices)
    {
        if (numberQuestions == null) return;
        int idx = 0;
        foreach (Transform child in numberQuestions.transform)
        {
            bool shouldOutline = indices.Contains(idx);

            // Try DropPlaceholder/Draggable pattern: look for child named "Outline"
            var placeholder = child.GetComponent<DropPlaceholder>() ?? child.GetComponentInChildren<DropPlaceholder>();
            GameObject outlineObj = null;
            if (placeholder != null)
            {
                var t = placeholder.transform.Find("Outline");
                if (t != null) outlineObj = t.gameObject;
            }

            // Fallback: look directly under the slot for an "Outline" child
            if (outlineObj == null)
            {
                var t2 = child.Find("Outline");
                if (t2 != null) outlineObj = t2.gameObject;
            }

            if (outlineObj != null)
                outlineObj.SetActive(shouldOutline);
            idx++;
        }
    }
}
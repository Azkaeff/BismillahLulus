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
        bool allCorrect = true;
        for (int i = 0; i < userNumbers.Count; i++)
        {
            if (userNumbers[i] != answerNumbers[i])
            {
                allCorrect = false;
                break;
            }
        }

        if (allCorrect)
        {
            Debug.Log("Level 2 Completed!");
            // Show the level complete popup if a manager is assigned
            if (levelCompleteManager != null)
            {
                levelCompleteManager.ShowLevelComplete();
            }
            else
            {
                // fallback: try to find one in the scene
                LevelCompleteManager manager = FindObjectOfType<LevelCompleteManager>();
                if (manager != null)
                {
                    manager.ShowLevelComplete();
                }
                else
                {
                    Debug.LogWarning("LevelCompleteManager not found. Create/assign one to show the level complete popup.");
                }
            }
            
        }
    }
}
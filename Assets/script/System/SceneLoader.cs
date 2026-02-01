using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public enum SelectionMode { ByName, ByBuildIndex, NextInBuild }

    [Header("Scene Settings")]
    [Tooltip("How the Next button chooses the scene to load")]
    public SelectionMode selectionMode = SelectionMode.NextInBuild;

    [Tooltip("Target scene name (exactly as in Build Settings). Used when SelectionMode = ByName.")]
    public string targetSceneName;

    [Tooltip("Target build index. Used when SelectionMode = ByBuildIndex.")]
    public int targetBuildIndex = -1;

    [Tooltip("If true and NextInBuild reaches the end, wraps to scene 0. Otherwise it logs an error.")]
    public bool wrapWhenLast = false;

    // Loads a specific scene by name (callable from UI)
    public void LoadScene()
    {
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("Target Scene Name belum diisi!");
            return;
        }

        SceneManager.LoadScene(targetSceneName);
    }

    // Loads the configured "next" scene based on SelectionMode. Attach this to your Next button.
    public void LoadNextScene()
    {
        switch (selectionMode)
        {
            case SelectionMode.ByName:
                if (string.IsNullOrEmpty(targetSceneName))
                {
                    Debug.LogError("SelectionMode is ByName but Target Scene Name belum diisi!");
                    return;
                }
                SceneManager.LoadScene(targetSceneName);
                break;

            case SelectionMode.ByBuildIndex:
                if (targetBuildIndex < 0 || targetBuildIndex >= SceneManager.sceneCountInBuildSettings)
                {
                    Debug.LogError("Target build index tidak valid: " + targetBuildIndex);
                    return;
                }
                SceneManager.LoadScene(targetBuildIndex);
                break;

            case SelectionMode.NextInBuild:
                int current = SceneManager.GetActiveScene().buildIndex;
                int next = current + 1;
                if (next >= SceneManager.sceneCountInBuildSettings)
                {
                    if (wrapWhenLast)
                        next = 0;
                    else
                    {
                        Debug.LogError("Sudah di scene terakhir dan wrapWhenLast = false.");
                        return;
                    }
                }
                SceneManager.LoadScene(next);
                break;
        }
    }
}

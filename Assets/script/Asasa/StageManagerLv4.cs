using UnityEngine;

public class StageManagerLv4 : MonoBehaviour
{
    public static StageManagerLv4  Instance { get; private set; }
    public GameObject stageCompleteUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CompleteStage()
    {
        if (stageCompleteUI != null) stageCompleteUI.SetActive(true);
        Time.timeScale = 0f; // pause game

        foreach (GameObject rootGo in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            Transform found = FindDeepChild(rootGo.transform, "ThisIsLevelCompleteUI");
            if (found != null)
            {
                found.gameObject.SetActive(true);
                break;
            }
        }
    }

    private Transform FindDeepChild(Transform parent, string name)
    {
        if (parent.name == name) return parent;
        foreach (Transform child in parent)
        {
            Transform result = FindDeepChild(child, name);
            if (result != null) return result;
        }
        return null;
    }
}
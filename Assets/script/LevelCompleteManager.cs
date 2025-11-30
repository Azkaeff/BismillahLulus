using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteManager : MonoBehaviour


{
    public GameObject taskPanel;           // drag TaskPanel
    public GameObject levelCompletePanel;  // drag LevelCompletePanel
    public UIManager uiManager;

    private void Start()
    {
        levelCompletePanel.SetActive(false);
    }

    // Dipanggil oleh gameplay saat level selesai
    public void ShowLevelComplete()
    {
        levelCompletePanel.SetActive(true);
        taskPanel.SetActive(false);

        if (uiManager != null)
            uiManager.ShowLevelComplete();
    }

    // ============== BUTTON EVENTS ==============
    public void RetryLevel()
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }

    public void NextLevel()
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex + 1);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

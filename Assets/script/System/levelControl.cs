using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControl : MonoBehaviour
{
    public void Ulangi()
    {
        // Reset time scale in case the game was paused
        Time.timeScale = 1f;
        
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log($"Ulangi: Reloading scene '{sceneName}'");
        SceneManager.LoadScene(sceneName);
    }

    public void KembaliKeMenu()
    {
        // Reset time scale in case the game was paused
        Time.timeScale = 1f;
        
        Debug.Log("KembaliKeMenu: Loading Menu scene");
        SceneManager.LoadScene("Menu");
    }
}

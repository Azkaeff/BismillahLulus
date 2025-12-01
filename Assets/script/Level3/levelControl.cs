using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControl : MonoBehaviour
{
    public void Ulangi()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void KembaliKeMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}

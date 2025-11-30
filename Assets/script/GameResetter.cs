using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResetter : MonoBehaviour
{
    public void ResetLevel2()
    {
        SceneManager.LoadScene("Level2");
    }
}

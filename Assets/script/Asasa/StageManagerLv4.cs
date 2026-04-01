using UnityEngine;

public class StageManagerLv4 : MonoBehaviour
{
    public GameObject stageCompleteUI;

    public void CompleteStage()
    {
        stageCompleteUI.SetActive(true);
        Time.timeScale = 0f; // pause game
    }
}
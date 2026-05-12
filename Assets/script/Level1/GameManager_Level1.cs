using UnityEngine;
using TMPro;

public class GameManager_Level1 : MonoBehaviour
{
    public static GameManager_Level1 Instance { get; private set; }

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

    [Header("Level 1 – Reorder Settings")]
    public Transform dropZonePanel;

    [Header("UI")]
    public TMP_Text resultText;
    public UIManager ui;
    public GameObject ThisIsLevelCompleteUI;

    public void CheckResult()
    {
        int totalSlots = dropZonePanel.childCount;

        for (int i = 0; i < totalSlots; i++)
        {
            Transform slot = dropZonePanel.GetChild(i);
            int expected = i + 1;

            Number numObj = slot.GetComponentInChildren<Number>();

            if (numObj == null)
            {
                if (resultText != null) resultText.text = "Masih salah: ada slot kosong.";
                return;
            }

            if (numObj.value != expected)
            {
                if (resultText != null) resultText.text = "Masih salah, coba lagi!";
                return;
            }
        }

        if (resultText != null) resultText.text = "Selamat! Semua benar!";
        if (ui != null) ui.ShowLevelComplete();
        if (ThisIsLevelCompleteUI != null) ThisIsLevelCompleteUI.SetActive(true);
    }
}

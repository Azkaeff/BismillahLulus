using UnityEngine;
using TMPro;

public class GameManager_Level1 : MonoBehaviour
{
    [Header("Level 1 – Reorder Settings")]
    public Transform dropZonePanel;

    [Header("UI")]
    public TMP_Text resultText;
    public UIManager ui;

    public void CheckResult()
    {
        int totalSlots = dropZonePanel.childCount;

        for (int i = 0; i < totalSlots; i++)
        {
            Transform slot = dropZonePanel.GetChild(i);
            int expected = i + 1;

            if (slot.childCount == 0)
            {
                resultText.text = "Masih salah: ada slot kosong.";
                return;
            }

            NumberObject no = slot.GetChild(0).GetComponent<NumberObject>();

            if (no == null || no.number != expected)
            {
                resultText.text = "Masih salah, coba lagi!";
                return;
            }
        }

        resultText.text = "Selamat! Semua benar!";
        ui.ShowLevelComplete();
    }
}

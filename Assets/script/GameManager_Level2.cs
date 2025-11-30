using UnityEngine;
using TMPro;

public class GameManager_Level2 : MonoBehaviour
{
    [Header("Level 2 – Fill Slot Settings")]
    public Transform fillSlotPanel;

    [Header("UI")]
    public TMP_Text resultText;
    public UIManager ui;

    public void CheckResult()
    {
        foreach (Transform child in fillSlotPanel)
        {
            DropZoneLevel2 slot = child.GetComponent<DropZoneLevel2>();

            if (slot == null)
                continue;

            if (slot.transform.childCount == 0)
            {
                resultText.text = "Masih ada slot kosong!";
                return;
            }

            NumberObject numberObj = slot.transform.GetChild(0).GetComponent<NumberObject>();

            if (numberObj == null)
            {
                resultText.text = "Objek angka tidak valid!";
                return;
            }

            if (numberObj.number != slot.correctNumber)
            {
                resultText.text = "Masih salah, coba lagi!";
                return;
            }
        }

        resultText.text = "Selamat! Semua benar!";
        ui.ShowLevelComplete();
    }
    public void ResetSceneState()
    {
        if (fillSlotPanel != null)
        {
            foreach (Transform child in fillSlotPanel)
            {
                DropZoneLevel2 slot = child.GetComponent<DropZoneLevel2>();
                if (slot != null && slot.transform.childCount > 0)
                {
                    Destroy(slot.transform.GetChild(0).gameObject);
                }
            }
        }
        ui.HideLevelComplete();
        resultText.text = "";
    }
}

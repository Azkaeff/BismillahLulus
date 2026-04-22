using UnityEngine;

public class CloseLvlCompleteUI : MonoBehaviour
{
    [Tooltip("Masukkan GameObject LvlCompleteUIBackground atau UI yang ingin ditutup ke sini")]
    public GameObject lvlCompleteUI;

    // Fungsi ini bisa dipanggil dari event OnClick() pada Button
    public void CloseUI()
    {
        if (lvlCompleteUI != null)
        {
            lvlCompleteUI.SetActive(false);
        }
        
        // Mengembalikan waktu berjalan normal jika sebelumnya di-pause (Time.timeScale = 0)
        Time.timeScale = 1f;
    }
}

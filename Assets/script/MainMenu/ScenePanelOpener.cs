using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePanelOpener : MonoBehaviour
{
    [Header("Pengaturan Scene dan Panel")]
    [Tooltip("Nama scene yang ingin dibuka")]
    public string targetSceneName;
    
    [Tooltip("Sesuai dengan nama di box My Panel Name pada script PanelHandler")]
    public string targetPanelName;

    // Static variable untuk menyimpan nama panel saat pindah scene
    private static string panelToOpen;

    public void OpenSceneAndPanel()
    {
        // Jika scene yang dituju berbeda dengan scene saat ini
        if (!string.IsNullOrEmpty(targetSceneName) && targetSceneName != SceneManager.GetActiveScene().name)
        {
            panelToOpen = targetPanelName;
            
            // Daftarkan event listener yang akan dipanggil setelah scene baru selesai dimuat
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            // Jika sudah berada di scene yang sama, langsung buka panel
            OpenPanelDirectly(targetPanelName);
        }
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Hapus listener agar tidak dipanggil berulang kali
        SceneManager.sceneLoaded -= OnSceneLoaded;
        
        if (!string.IsNullOrEmpty(panelToOpen))
        {
            OpenPanelDirectly(panelToOpen);
            panelToOpen = null;
        }
    }

    private static void OpenPanelDirectly(string panelName)
    {
        if (MainMenuHandler.Instance != null)
        {
            MainMenuHandler.Instance.panelName = panelName;
        }
        else
        {
            Debug.LogWarning("MainMenuHandler.Instance tidak ditemukan di scene saat ini!");
        }
    }
}

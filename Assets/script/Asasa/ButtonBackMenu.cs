using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBackMenu : MonoBehaviour
{
    [Header("Scene Target")]
    [Tooltip("Nama scene target yang akan dipanggil (sesuaikan dengan nama scene di Build Settings)")]
    public string targetSceneName;

    // Method ini akan dipanggil ketika tombol diklik (tanpa parameter, menggunakan string dari Inspector)
    public void LoadTargetScene()
    {
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("Nama target scene belum diisi di Inspector!");
            return;
        }

        Debug.Log("Berpindah ke scene: " + targetSceneName);
        SceneManager.LoadScene(targetSceneName);
    }

    // Method alternatif jika ingin memasukkan nama scene langsung dari event OnClick di Inspector
    public void LoadSceneByName(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Parameter nama scene belum diisi!");
            return;
        }

        Debug.Log("Berpindah ke scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}

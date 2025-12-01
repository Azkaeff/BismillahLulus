using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject taskPanel;
    public GameObject helpPanel;
    public GameObject levelCompletePanel;

    public TMPro.TextMeshProUGUI taskText;
    public TMPro.TextMeshProUGUI helpText;
    public TMPro.TextMeshProUGUI completeTitle;

    [Header("Help Navigation")]
    public Image tutorialImage;
    public Sprite[] tutorialSprites;
    public Button nextButton;
    public Button previousButton;

    private int currentIndex = 0;


    private void Start()
    {
        ShowTask("Susunlah bilangan dari yang paling kecil ke paling besar.");
        HideHelp();
        HideLevelComplete();
    }

    // ---------------- TASK PANEL ----------------
    public void ShowTask(string msg)
    {
        taskPanel.SetActive(true);
        taskText.text = msg;
    }

    // ---------------- HELP PANEL ----------------
    public void ShowHelp()
    {
        helpPanel.SetActive(true);

        currentIndex = 0;
        UpdateHelpContent();

    }

    public void HideHelp()
    {
        helpPanel.SetActive(false);
    }

    // --- FUNGSI NAVIGASI ---
    public void NextImage()
    {
        // Pindah ke indeks selanjutnya, loop ke awal jika sudah di akhir
        currentIndex = (currentIndex + 1) % tutorialSprites.Length;
        UpdateHelpContent();
    }

    public void PreviousImage()
    {
        // Pindah ke indeks sebelumnya
        currentIndex--;
        if (currentIndex < 0)
        {
            // Loop ke akhir jika sudah di awal
            currentIndex = tutorialSprites.Length - 1;
        }
        UpdateHelpContent();
    }

    private void UpdateHelpContent()
    {
        if (tutorialSprites == null || tutorialSprites.Length == 0)
        {
            Debug.LogWarning("Array tutorialSprites kosong!");
            if (tutorialImage != null) tutorialImage.sprite = null;
            if (helpText != null) helpText.text = "Tidak ada petunjuk.";
            return;
        }

        tutorialImage.sprite = tutorialSprites[currentIndex];

        if (helpText != null)
        {
            // Menghitung nomor halaman saat ini (dimulai dari 1) dan total halaman
            string pageNumber = $"Petunjuk {currentIndex + 1} dari {tutorialSprites.Length}";

            // Gabungkan nomor halaman dengan teks petunjuk statis (jika diperlukan)
            string staticInstruction =
                "1. Seret kotak angka ke tempat kosong.\n" +
                "2. Susun angka mulai dari yang paling kecil.\n" +
                "3. Jika salah, coba lagi ya!\n\n" +
                "Semangat, kamu pasti bisa 😊";

            helpText.text = $"{pageNumber}\n\n{staticInstruction}";
        }

        bool canNavigate = tutorialSprites.Length > 1;
        if (nextButton != null) nextButton.interactable = canNavigate;
        if (previousButton != null) previousButton.interactable = canNavigate;
    }

    // ---------------- LEVEL COMPLETE PANEL ----------------
    public void ShowLevelComplete()
    {
        levelCompletePanel.SetActive(true);
        completeTitle.text = "Bagus sekali! Kamu berhasil! 🎉";
    }

    public void HideLevelComplete()
    {
        levelCompletePanel.SetActive(false);
    }
}

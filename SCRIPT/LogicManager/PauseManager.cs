using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject pauseMenuPanel;   // Panel Utama Pause
    public GameObject settingsPanel;    // Panel Settings
    public GameObject controlPanel;     // Panel Control
    public GameObject volumePanel;      // Panel Volume

    [Header("Audio Settings")]
    public Slider volumeSlider;         // Slider untuk mengatur suara

    public static bool isPaused = false;

    void Awake()
    {
        // 1. Load volume yang tersimpan atau set ke 1 (Full) jika belum ada data
        // Menggunakan Awake agar suara langsung ada sebelum frame pertama muncul
        float savedVolume = PlayerPrefs.GetFloat("SavedVolume", 1f);
        AudioListener.volume = savedVolume;
        AudioListener.pause = false;
    }

    void Start()
    {
        // 2. Sinkronisasi Slider dengan volume yang dimuat
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // Pastikan semua panel tertutup saat mulai
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (controlPanel != null) controlPanel.SetActive(false);
        if (volumePanel != null) volumePanel.SetActive(false);
    }

    void Update()
    {
        // Menekan Esc untuk Pause atau navigasi Back
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                if (controlPanel != null && controlPanel.activeSelf) CloseControl();
                else if (volumePanel != null && volumePanel.activeSelf) CloseVolume();
                else if (settingsPanel != null && settingsPanel.activeSelf) CloseSettings();
                else ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // --- LOGIKA UTAMA ---

    public void PauseGame()
    {
        isPaused = true;
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (controlPanel != null) controlPanel.SetActive(false);
        if (volumePanel != null) volumePanel.SetActive(false);

        Time.timeScale = 1f;
        AudioListener.pause = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // --- NAVIGASI PANEL ---

    public void OpenSettings() { pauseMenuPanel.SetActive(false); settingsPanel.SetActive(true); }
    public void CloseSettings() { settingsPanel.SetActive(false); pauseMenuPanel.SetActive(true); }

    public void OpenControl() { settingsPanel.SetActive(false); controlPanel.SetActive(true); }
    public void CloseControl() { controlPanel.SetActive(false); settingsPanel.SetActive(true); }

    public void OpenVolume() { settingsPanel.SetActive(false); volumePanel.SetActive(true); }
    public void CloseVolume() { volumePanel.SetActive(false); settingsPanel.SetActive(true); }

    // --- LOGIKA VOLUME ---

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("SavedVolume", value);
        PlayerPrefs.Save(); // Memastikan data tersimpan di penyimpanan lokal
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene("MainMenu");
    }
}
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Penting untuk pindah Scene dan Restart

public class TimeManager : MonoBehaviour
{
    [Header("Timer Settings")]
    public float startingTime = 240f;
    private float currentTime;
    private bool isTimerRunning = true;

    [Header("UI References")]
    public TMP_Text timeText;
    public GameObject gameOverPanel;
    public GameObject gameWinPanel; // Tambahkan referensi untuk panel menang jika ada

    void Start()
    {
        // Inisialisasi waktu awal
        currentTime = startingTime;

        // Pastikan panel-panel UI mati saat mulai
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (gameWinPanel != null) gameWinPanel.SetActive(false);

        UpdateTimerDisplay();
    }

    void Update()
    {
        // Waktu hanya berjalan jika:
        // 1. Game tidak sedang Pause (Time.timeScale > 0.1f)
        // 2. Timer aktif/belum dihentikan oleh StopTimer (isTimerRunning)
        if (Time.timeScale > 0.1f && isTimerRunning)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();

            if (currentTime <= 0)
            {
                currentTime = 0;
                GameOver();
            }
        }
    }

    // Mengubah angka detik menjadi format menit:detik (00:00)
    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        if (timeText != null)
        {
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // --- FUNGSI UNTUK INVENTORY MANAGER ---

    public void ResetTimer()
    {
        currentTime = startingTime;
        UpdateTimerDisplay();
    }

    public void StopTimer()
    {
        isTimerRunning = false;
        Debug.Log("Timer Berhenti: Misi pengumpulan selesai.");
    }

    // --- FUNGSI NAVIGASI & UI ---

    void GameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Menghentikan game

        // Munculkan kursor untuk klik tombol
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Dipanggil oleh tombol 'Next' pada Win Panel
    public void GoToStoryAfter()
    {
        Time.timeScale = 1f; // Reset waktu agar scene berikutnya normal
        SceneManager.LoadScene("StoryAfter"); // Pastikan nama scene persis di Build Settings
    }

    // Dipanggil oleh tombol 'Restart' pada Game Over Panel
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Dipanggil oleh tombol 'Main Menu' pada Game Over atau Pause Panel
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
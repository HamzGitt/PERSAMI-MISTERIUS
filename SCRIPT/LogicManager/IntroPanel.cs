using UnityEngine;

public class OpeningManager : MonoBehaviour
{
    public GameObject introPanel; // Tarik objek Image/Panel cerita Anda ke sini

    void Start()
    {
        // Munculkan gambar saat game mulai
        if (introPanel != null)
        {
            introPanel.SetActive(true);

            // Hentikan waktu agar NPC & Player tidak bergerak di background
            Time.timeScale = 0f;

            // Pastikan kursor muncul untuk klik tombol
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // Fungsi untuk dihubungkan ke Button OnClick
    public void StartGame()
    {
        introPanel.SetActive(false); // Sembunyikan gambar

        Time.timeScale = 1f; // Jalankan waktu kembali

        // Sembunyikan kursor jika perlu (untuk kontrol karakter)
        // Cursor.visible = false;
    }
}
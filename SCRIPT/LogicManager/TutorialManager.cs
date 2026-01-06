using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel; // Masukkan objek TutorialPanel di sini

    void Start()
    {
        // Pastikan panel muncul di awal
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);

            // Opsional: Pause game agar pemain bisa membaca kontrol dulu
            Time.timeScale = 0f;

            // Pastikan kursor terlihat jika diperlukan untuk UI
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Update()
    {
        // Mengecek jika tombol Spasi ATAU Enter (Return) ditekan
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            CloseTutorial();
        }
    }

    void CloseTutorial()
    {
        if (tutorialPanel != null)
        {
            // Menghilangkan panel tutorial
            tutorialPanel.SetActive(false);

            // Lanjutkan waktu game
            Time.timeScale = 1f;

            // Kunci kembali kursor untuk gameplay (khusus First Person/Third Person)
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalAudio : MonoBehaviour
{
    public static GlobalAudio instance;
    private AudioSource audioSource;
    public AudioClip clickSound;

    void Awake()
    {
        // Logika Singleton: Memastikan hanya ada 1 GlobalAudio di game
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Objek tidak hancur saat pindah scene
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Mendeteksi klik mouse kiri di scene mana pun
        if (Input.GetMouseButtonDown(0))
        {
            PlayClick();
        }
    }

    public void PlayClick()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
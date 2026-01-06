using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour
{
    [Header("Settings")]
    public string sceneToLoad = "StoryGame";

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject creditPanel;

    void Start()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (creditPanel != null) creditPanel.SetActive(false);
    }

    // --- FUNGSI NAVIGASI ---

    public void PlayGame()
    {
        Debug.Log("<color=cyan>Sistem: Tombol PLAY ditekan.</color>");
        LoadTargetScene();
    }

    public void OpenCredit()
    {
        Debug.Log("<color=yellow>Sistem: Membuka Panel Credit.</color>");
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (creditPanel != null) creditPanel.SetActive(true);
    }

    public void CloseCredit()
    {
        Debug.Log("<color=yellow>Sistem: Menutup Panel Credit.</color>");
        if (creditPanel != null) creditPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    // --- FUNGSI QUIT DENGAN DEBUG ---
    public void QuitGame()
    {
        // Debug ini akan muncul di Console Unity saat tombol ditekan
        Debug.Log("<color=red><b>SISTEM: Tombol QUIT Berhasil Ditekan! Game akan menutup.</b></color>");

        // Perintah keluar untuk aplikasi hasil build (.exe / .apk)
        Application.Quit();

        // Tambahan agar tombol Quit juga menghentikan mode Play di dalam Editor Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void LoadTargetScene()
    {
        if (Application.CanStreamedLevelBeLoaded(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Gagal: Scene '" + sceneToLoad + "' tidak ada di Build Settings!");
        }
    }
}
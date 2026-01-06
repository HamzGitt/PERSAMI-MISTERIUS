using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static Dictionary<string, int> items = new Dictionary<string, int>();

    [Header("UI References")]
    public TMP_Text logCountText;
    public TMP_Text keroseneCountText;
    public TMP_Text lighterCountText;

    [Header("Hint Panels")]
    public GameObject keroseneHintPanel;
    public GameObject lighterHintPanel;
    public GameObject finalHintPanel;        // Muncul saat barang lengkap
    public GameObject instructionHintPanel;  // Muncul setelah finalHintPanel ditutup

    [Header("Item Groups to Spawn")]
    public GameObject keroseneGroup;
    public GameObject lighterGroup;

    [Header("Logic References")]
    public TimeManager timeManager;

    void Awake() => items.Clear();

    void Start()
    {
        // Inisialisasi UI Teks
        if (logCountText != null) logCountText.gameObject.SetActive(true);
        if (keroseneCountText != null) keroseneCountText.gameObject.SetActive(false);
        if (lighterCountText != null) lighterCountText.gameObject.SetActive(false);

        // Inisialisasi Panel Hint (Semua Nonaktif)
        if (keroseneHintPanel != null) keroseneHintPanel.SetActive(false);
        if (lighterHintPanel != null) lighterHintPanel.SetActive(false);
        if (finalHintPanel != null) finalHintPanel.SetActive(false);
        if (instructionHintPanel != null) instructionHintPanel.SetActive(false);

        // Inisialisasi Group Item
        if (keroseneGroup != null) keroseneGroup.SetActive(false);
        if (lighterGroup != null) lighterGroup.SetActive(false);

        UpdateAllUI();
    }

    public void AddItem(string itemName)
    {
        string name = itemName.Trim();

        // Proteksi agar tidak mengambil item melebihi batas quest
        if (name == "Log" && GetCount("Log") >= 5) return;
        if (name == "Minyak Tanah" && GetCount("Minyak Tanah") >= 2) return;
        if (name == "Korek" && GetCount("Korek") >= 1) return;

        if (items.ContainsKey(name)) items[name]++;
        else items.Add(name, 1);

        UpdateAllUI();
        CheckProgression();
    }

    private void CheckProgression()
    {
        int logs = GetCount("Log");
        int kero = GetCount("Minyak Tanah");
        int lighter = GetCount("Korek");

        // 1. Log 5/5 -> Hint Minyak
        if (logs >= 5 && logCountText != null && logCountText.gameObject.activeSelf)
        {
            ShowHint(keroseneHintPanel);
            if (timeManager != null) timeManager.ResetTimer();
            logCountText.gameObject.SetActive(false);
            if (keroseneCountText != null) keroseneCountText.gameObject.SetActive(true);
            if (keroseneGroup != null) keroseneGroup.SetActive(true);
        }

        // 2. Minyak 2/2 -> Hint Korek
        if (kero >= 2 && keroseneCountText != null && keroseneCountText.gameObject.activeSelf)
        {
            ShowHint(lighterHintPanel);
            if (timeManager != null) timeManager.ResetTimer();
            keroseneCountText.gameObject.SetActive(false);
            if (lighterCountText != null) lighterCountText.gameObject.SetActive(true);
            if (lighterGroup != null) lighterGroup.SetActive(true);
        }

        // 3. Korek 1/1 -> Final Hint
        if (lighter >= 1 && lighterCountText != null && lighterCountText.gameObject.activeSelf)
        {
            ShowHint(finalHintPanel);
            lighterCountText.gameObject.SetActive(false);

            // Memanggil fungsi StopTimer di TimeManager agar hitung mundur berhenti permanen
            if (timeManager != null) timeManager.StopTimer();
        }
    }

    private void ShowHint(GameObject panel)
    {
        if (panel != null)
        {
            panel.SetActive(true);
            Time.timeScale = 0f; // Pause waktu saat hint muncul
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // Gunakan fungsi ini untuk tombol OK pada Hint Minyak dan Korek
    public void CloseHint(GameObject panel)
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
        // Mengembalikan kursor ke mode gameplay (terkunci)
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Gunakan fungsi ini KHUSUS untuk tombol 'Next' di Final Hint
    public void CloseFinalHint()
    {
        if (finalHintPanel != null) finalHintPanel.SetActive(false);

        if (instructionHintPanel != null)
        {
            instructionHintPanel.SetActive(true);
            // Time.timeScale TETAP 0 agar pemain tenang membaca instruksi terakhir
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Gunakan fungsi ini KHUSUS untuk tombol 'OK' di Instruction Hint
    public void CloseInstructionHint()
    {
        if (instructionHintPanel != null) instructionHintPanel.SetActive(false);
        Time.timeScale = 1f; // Waktu jalan kembali untuk misi terakhir menyalakan api
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UpdateAllUI()
    {
        if (logCountText != null) logCountText.text = "LOG: " + GetCount("Log") + "/5";
        if (keroseneCountText != null) keroseneCountText.text = "MINYAK: " + GetCount("Minyak Tanah") + "/2";
        if (lighterCountText != null) lighterCountText.text = "KOREK: " + GetCount("Korek") + "/1";
    }

    private int GetCount(string name) => items.ContainsKey(name) ? items[name] : 0;

    public bool CanLightFire() => GetCount("Log") >= 5 && GetCount("Minyak Tanah") >= 2 && GetCount("Korek") >= 1;
    public void CheckInventory() { }
    public bool TryConsumeLogsForFire() => true;
}
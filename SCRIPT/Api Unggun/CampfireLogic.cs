using UnityEngine;

public class CampfireLogic : MonoBehaviour
{
    [Header("Settings")]
    public GameObject fireEffect;
    public GameObject winPanel;
    public GameObject interactionUI;

    private bool isPlayerNearby = false;
    private InventoryManager inv;

    void Start()
    {
        inv = FindObjectOfType<InventoryManager>();
        if (fireEffect != null) fireEffect.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (interactionUI != null) interactionUI.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (inv != null && inv.CanLightFire())
            {
                ActivateVictory();
            }
            else
            {
                Debug.Log("Item belum lengkap! Butuh 5 Log, 2 Minyak, 1 Korek.");
            }
        }
    }

    void ActivateVictory()
    {
        if (fireEffect != null) fireEffect.SetActive(true);
        if (winPanel != null) winPanel.SetActive(true);
        if (interactionUI != null) interactionUI.SetActive(false);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (interactionUI != null) interactionUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (interactionUI != null) interactionUI.SetActive(false);
        }
    }
}
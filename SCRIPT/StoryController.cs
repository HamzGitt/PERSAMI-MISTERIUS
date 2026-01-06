using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement; // Dibutuhkan untuk perpindahan scene

public class StoryController : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    [TextArea(3, 10)]
    public string storyText;
    public float typingSpeed = 0.05f;

    private bool isTyping = false;
    private bool cancelTyping = false;

    void Start()
    {
        StartCoroutine(TypeText());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                cancelTyping = true;
            }
        }
    }

    IEnumerator TypeText()
    {
        textDisplay.text = "";
        isTyping = true;
        cancelTyping = false;

        foreach (char letter in storyText.ToCharArray())
        {
            if (cancelTyping)
            {
                textDisplay.text = storyText;
                break;
            }

            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    // Fungsi untuk pindah ke scene InGame
    public void LoadInGameScene()
    {
        SceneManager.LoadScene("InGame");
    }

    // Fungsi untuk kembali ke MainMenu
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
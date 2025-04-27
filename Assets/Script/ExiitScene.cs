using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitScene : MonoBehaviour
{
    public Button yesButton;
    public Button noButton;

    public AudioSource buttonAudioSource; // AudioSource untuk efek klik

    void Start()
    {
        if (yesButton != null)
        {
            yesButton.onClick.AddListener(OnYesButtonClicked);
        }

        if (noButton != null)
        {
            noButton.onClick.AddListener(OnNoButtonClicked);
        }
    }

    void OnYesButtonClicked()
    {
        PlayClickSound();
        Invoke("QuitGame", 0.3f); // Delay sebelum quit
    }

    void OnNoButtonClicked()
    {
        PlayClickSound();
        Invoke("BackToHome", 0.3f); // Delay sebelum balik ke Home
    }

    void PlayClickSound()
    {
        if (buttonAudioSource != null)
        {
            buttonAudioSource.Play();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToHome()
    {
        SceneManager.LoadScene("Home");
    }
}

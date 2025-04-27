using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeControllerFix : MonoBehaviour
{
    public Button playButton;
    public Button exitButton;

    public AudioSource buttonAudioSource; // hanya AudioSource saja

    void Start()
    {
        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }
    }

    void OnPlayButtonClicked()
    {
        PlayClickSound();
        Invoke("LoadMainScene", 0.3f); // Delay 0.3 detik
    }

    void OnExitButtonClicked()
    {
        PlayClickSound();
        Invoke("LoadExitScene", 0.3f); // Delay 0.3 detik
    }

    void PlayClickSound()
    {
        if (buttonAudioSource != null)
        {
            buttonAudioSource.Play();
        }
    }

    void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    void LoadExitScene()
    {
        SceneManager.LoadScene("Exit");
    }
}

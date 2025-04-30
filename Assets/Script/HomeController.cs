using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeControllerFix : MonoBehaviour
{
    public Button playButton;
    public Button exitButton;
    public AudioSource buttonAudioSource;

    [Header("Button Colors")]
    public Color normalColor = Color.white;
    public Color highlightedColor = Color.yellow;
    public Color pressedColor = Color.gray;
    public Color disabledColor = Color.black;

    void Start()
    {
        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
            SetButtonColors(playButton);
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
            SetButtonColors(exitButton);
        }
    }

    void OnPlayButtonClicked()
    {
        PlayClickSound();
        Invoke("LoadMainScene", 0.3f);
    }

    void OnExitButtonClicked()
    {
        PlayClickSound();
        Invoke("LoadExitScene", 0.3f);
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

    void SetButtonColors(Button button)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = normalColor;
        colors.highlightedColor = highlightedColor;
        colors.pressedColor = pressedColor;
        colors.disabledColor = disabledColor;
        button.colors = colors;
    }
}

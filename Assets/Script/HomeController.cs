using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeControllerFix : MonoBehaviour
{
    public Button playButton;
    public Button exitButton;

    void Start()
    {
        // Pastikan button tidak null
        if (playButton != null)
        {
            playButton.onClick.AddListener(PlayGame);
        }
        
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitGame);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Exit");
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {
    public GameObject pauseMenu;
    private bool isPaused = false;

    void Start() {
        pauseMenu.SetActive(false); // Pastikan pause menu mati saat start
        Time.timeScale = 1; // Normal speed game saat start
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                SceneManager.LoadScene("Exit"); // ESC lagi langsung exit
            } else {
                PauseGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            ResumeGame();
        }
    }

    public void PauseGame() {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0; // Freeze game
    }

    public void ResumeGame() {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1; // Lanjut game
    }

    public void ExitGame() {
        SceneManager.LoadScene("Exit");
    }
}

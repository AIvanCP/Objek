﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections; // << Tetap diperlukan untuk coroutine

public class GameManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button continueButton;
    public Button exitButton; 

    [Header("UI Elements")]
    public GameObject pauseMenu;

    [Header("Audio")]
    public AudioSource buttonAudioSource;

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        Time.timeScale = 1f; // Normal speed game saat start

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueButtonClicked);

        if (exitButton != null)
            exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
            PlayClickSound();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isPaused)
            {
                ResumeGame();
                PlayClickSound();
            }
        }
    }

    void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        if (pauseMenu != null)
        {
            isPaused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        if (pauseMenu != null)
        {
            isPaused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void ExitToExitScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Exit");
    }

    void PlayClickSound()
    {
        if (buttonAudioSource != null)
            buttonAudioSource.Play();
    }

    // Button click handlers
    void OnContinueButtonClicked()
    {
        ResumeGame();
        PlayClickSound();
    }

    void OnExitButtonClicked()
    {
        PlayClickSound();
        StartCoroutine(ExitToExitSceneDelayed());
    }

    IEnumerator ExitToExitSceneDelayed()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        ExitToExitScene();
    }
}

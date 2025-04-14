using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GlobalAudioManager : MonoBehaviour
{
    private static GlobalAudioManager instance;
    public static GlobalAudioManager Instance
    {
        get { return instance; }
    }
    
    public AudioSource bgmSource;
    
    public float normalVolume = 0.4f;
    public float pausedVolume = 0.2f;
    private bool isPaused = false;
    
    // Event yang bisa disubscribe oleh script lain
    public static event Action<bool> OnGamePauseStateChanged;

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Pastikan volume awal sesuai
            if (bgmSource != null)
            {
                bgmSource.volume = normalVolume;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // Subscribe ke event scene loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset state pause ketika masuk main scene
        if (scene.name == "Main")
        {
            isPaused = false;
            if (bgmSource != null)
            {
                bgmSource.volume = normalVolume;
            }
        }
    }
    
    public void SetPauseState(bool paused)
    {
        isPaused = paused;
        
        // Sesuaikan volume berdasarkan state pause
        if (bgmSource != null)
        {
            bgmSource.volume = isPaused ? pausedVolume : normalVolume;
        }
        
        // Trigger event untuk script lain
        OnGamePauseStateChanged?.Invoke(isPaused);
    }
    
    public bool IsPaused()
    {
        return isPaused;
    }
    
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource bgmSource; 

    private float normalVolume = 0.4f;
    private float pausedVolume = 0.2f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (bgmSource == null)
        {
            Debug.LogError("❌ AudioSource tidak diassign di Inspector!");
            return;
        }

        bgmSource.volume = normalVolume;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void SetPauseState(bool isPaused)
    {
        if (bgmSource == null) return;
        
        bgmSource.volume = isPaused ? pausedVolume : normalVolume;
    }

    public void ResetVolume()
    {
        if (bgmSource == null) return;

        bgmSource.volume = normalVolume;
    }
}

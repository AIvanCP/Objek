using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class InstrumentManager : MonoBehaviour
{
    public TextMeshProUGUI alatMusikNames;
    public TextMeshProUGUI descriptionText;
    public Image instrumentImage;
    public GameObject InfoPanel;
    public GameObject bigImagePanel;
    public Image bigInstrumentImage;
    public CanvasGroup uiCanvas;
    public GameObject informationPanelRoot;
    public Animator mapAnimator;

    public Sprite[] instrumentSprites;
    public string[] instrumentNames;
    public string[] instrumentDescriptions;
    public AudioClip[] instrumentSounds;
    public Button[] instrumentButtons;

    private int currentIndex = 0;
    private bool isPaused = false;

    // Simpan referensi lokal untuk bgmSource
    public AudioSource bgmSource;
    public AudioSource instrumentSource;
    public float normalVolume = 0.4f;
    public float pausedVolume = 0.2f;

    void Start()
    {
        // if (InfoPanel != null) InfoPanel.SetActive(false);
        // if (bigImagePanel != null) bigImagePanel.SetActive(false);

        if (uiCanvas != null)
        {
            uiCanvas.blocksRaycasts = true;
        }

        if (instrumentSource == null)
        {
            instrumentSource = gameObject.AddComponent<AudioSource>();
            instrumentSource.playOnAwake = false;
        }

        if (instrumentButtons == null || instrumentButtons.Length == 0)
        {
            return;
        }

        for (int i = 0; i < instrumentButtons.Length; i++)
        {
            if (instrumentButtons[i] == null)
            {
                continue;
            }

            int index = i;
            instrumentButtons[i].onClick.AddListener(() => ShowInstrumentInfo(index));
        }

        UpdateInstrument(false);

        if (informationPanelRoot.activeSelf)
        {
            informationPanelRoot.SetActive(false);
        }

        // Subscribe ke event pause dari GlobalAudioManager
        GlobalAudioManager.OnGamePauseStateChanged += HandlePauseStateChanged;
    }

    void OnDestroy()
    {
        // Unsubscribe untuk mencegah memory leak
        GlobalAudioManager.OnGamePauseStateChanged -= HandlePauseStateChanged;
    }

    void HandlePauseStateChanged(bool paused)
    {
        isPaused = paused;

        // Handle local audio based on pause state
        if (isPaused)
        {
            // Ubah volume BGM lokal jika ada
            if (bgmSource != null) bgmSource.volume = pausedVolume;

            // Pause instrumen yang sedang diplay
            if (instrumentSource != null && instrumentSource.isPlaying)
            {
                instrumentSource.Pause();
            }
        }
        else
        {
            // Kembalikan volume BGM lokal jika ada
            if (bgmSource != null) bgmSource.volume = normalVolume;

            // Stop instrumen
            if (instrumentSource != null)
            {
                instrumentSource.Stop();
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ResumeGame();
        }

        if (isPaused) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextInstrument();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousInstrument();
        }

        if (Input.GetMouseButtonDown(0) && InfoPanel.activeSelf)
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                InfoPanel.GetComponent<RectTransform>(), Input.mousePosition, null))
            {
                HideAllPanels();
            }
        }
    }

    void UpdateInstrument(bool playSound)
    {
        if (currentIndex < 0 || currentIndex >= instrumentNames.Length)
        {
            return;
        }

        if (mapAnimator != null)
        {
            mapAnimator.Play("zoom-out");
        }

        alatMusikNames.text = instrumentNames[currentIndex];
        descriptionText.text = instrumentDescriptions[currentIndex];

        if (instrumentSprites[currentIndex] != null)
        {
            instrumentImage.sprite = instrumentSprites[currentIndex];
            instrumentImage.enabled = true;
        }

        if (playSound) PlayInstrumentSound(currentIndex);
    }

    public void ShowInstrumentInfo(int index)
    {
        if (index < 0 || index >= instrumentNames.Length)
        {
            return;
        }

        currentIndex = index;
        UpdateInstrument(true);

        // InfoPanel.SetActive(true);
        // instrumentImage.gameObject.SetActive(true);
        informationPanelRoot.SetActive(true);
    }

    public void HideAllPanels()
    {
        if (isPaused) return;

        // InfoPanel.SetActive(false);
        // bigImagePanel.SetActive(false);
        if (mapAnimator != null)
        {
            mapAnimator.Play("zoom");
            Debug.Log("Zoom in triggered");
        }
        informationPanelRoot.SetActive(false);

        if (instrumentSource != null && instrumentSource.isPlaying)
        {
            instrumentSource.Stop();
        }
    }

    public void ShowBigImage()
    {
        if (currentIndex < 0 || currentIndex >= instrumentSprites.Length)
        {
            return;
        }

        bigInstrumentImage.sprite = instrumentSprites[currentIndex];
        bigImagePanel.SetActive(true);
    }

    public void NextInstrument()
    {
        if (isPaused) return;

        currentIndex = (currentIndex + 1) % instrumentNames.Length;
        UpdateInstrument(true); // Diubah menjadi true untuk langsung play suara saat ganti alat musik
    }

    public void PreviousInstrument()
    {
        if (isPaused) return;

        currentIndex = (currentIndex - 1 + instrumentNames.Length) % instrumentNames.Length;
        UpdateInstrument(true); // Diubah menjadi true untuk langsung play suara saat ganti alat musik
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            if (bgmSource != null) bgmSource.volume = pausedVolume;

            if (instrumentSource != null && instrumentSource.isPlaying)
            {
                instrumentSource.Pause();
            }
        }
        else
        {
            if (bgmSource != null) bgmSource.volume = normalVolume;

            if (instrumentSource != null)
            {
                instrumentSource.Stop();
            }
        }

        // Notifikasi GlobalAudioManager tentang perubahan pause state
        if (GlobalAudioManager.Instance != null)
        {
            GlobalAudioManager.Instance.SetPauseState(isPaused);
        }
    }

    public void ResumeGame()
    {
        if (!isPaused) return;

        isPaused = false;
        if (bgmSource != null) bgmSource.volume = normalVolume;

        if (instrumentSource != null)
        {
            instrumentSource.Stop();
        }

        // Notifikasi GlobalAudioManager tentang resume
        if (GlobalAudioManager.Instance != null)
        {
            GlobalAudioManager.Instance.SetPauseState(false);
        }
    }

    void PlayInstrumentSound(int index)
    {
        if (instrumentSource == null)
        {
            return;
        }

        if (index < 0 || index >= instrumentSounds.Length || instrumentSounds[index] == null)
        {
            return;
        }

        instrumentSource.Stop();
        instrumentSource.clip = instrumentSounds[index];
        instrumentSource.Play();
    }
}
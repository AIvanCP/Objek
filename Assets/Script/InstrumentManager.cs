using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class InstrumentManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI alatMusikNames;
    public TextMeshProUGUI descriptionText;
    public Image instrumentImage;
    public GameObject InfoPanel;
    public GameObject bigImagePanel;
    public Image bigInstrumentImage;
    public CanvasGroup uiCanvas;
    public GameObject informationPanelRoot;
    public Animator mapAnimator;

    [Header("Assets")]
    public Sprite[] instrumentSprites;
    public string[] instrumentNames;
    public string[] instrumentDescriptions;
    public AudioClip[] instrumentSounds;
    public Button[] instrumentButtons;

    [Header("Audio")]
    public AudioSource bgmSource;
    public AudioSource instrumentSource;
    public float normalVolume = 0.4f;
    public float pausedVolume = 0.2f;

    private int currentIndex = 0;
    private bool isPaused = false;

    void Start()
    {
        if (uiCanvas != null)
            uiCanvas.blocksRaycasts = true;

        if (instrumentSource == null)
        {
            instrumentSource = gameObject.AddComponent<AudioSource>();
            instrumentSource.playOnAwake = false;
        }

        // Setup tombol alat musik
        if (instrumentButtons != null)
        {
            for (int i = 0; i < instrumentButtons.Length; i++)
            {
                if (instrumentButtons[i] != null)
                {
                    int index = i;
                    instrumentButtons[i].onClick.AddListener(() => ShowInstrumentInfo(index));
                }
            }
        }

        UpdateInstrument(false);

        if (informationPanelRoot != null && informationPanelRoot.activeSelf)
            informationPanelRoot.SetActive(false);

        // Subscribe ke GlobalAudioManager event
        GlobalAudioManager.OnGamePauseStateChanged += HandlePauseStateChanged;
    }

    void OnDestroy()
    {
        GlobalAudioManager.OnGamePauseStateChanged -= HandlePauseStateChanged;
    }

    void HandlePauseStateChanged(bool paused)
    {
        isPaused = paused;

        if (isPaused)
        {
            if (bgmSource != null) bgmSource.volume = pausedVolume;
            if (instrumentSource != null && instrumentSource.isPlaying)
                instrumentSource.Pause();
        }
        else
        {
            if (bgmSource != null) bgmSource.volume = normalVolume;
            if (instrumentSource != null)
                instrumentSource.Stop();
        }
    }

    void Update()
    {
        // Jangan accept input kalau paused
        if (isPaused) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextInstrument();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousInstrument();
        }

        if (Input.GetMouseButtonDown(0) && InfoPanel != null && InfoPanel.activeSelf)
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
            return;

        if (mapAnimator != null)
            mapAnimator.Play("zoom-out");

        if (alatMusikNames != null)
            alatMusikNames.text = instrumentNames[currentIndex];

        if (descriptionText != null)
            descriptionText.text = instrumentDescriptions[currentIndex];

        if (instrumentImage != null && instrumentSprites[currentIndex] != null)
        {
            instrumentImage.sprite = instrumentSprites[currentIndex];
            instrumentImage.enabled = true;
        }

        if (playSound)
            PlayInstrumentSound(currentIndex);
    }

    public void ShowInstrumentInfo(int index)
    {
        if (index < 0 || index >= instrumentNames.Length)
            return;

        currentIndex = index;
        UpdateInstrument(true);

        if (informationPanelRoot != null)
            informationPanelRoot.SetActive(true);
    }

    public void HideAllPanels()
    {
        if (isPaused) return;

        if (mapAnimator != null)
            mapAnimator.Play("zoom");

        if (informationPanelRoot != null)
            informationPanelRoot.SetActive(false);

        if (instrumentSource != null && instrumentSource.isPlaying)
            instrumentSource.Stop();
    }

    public void ShowBigImage()
    {
        if (currentIndex < 0 || currentIndex >= instrumentSprites.Length)
            return;

        if (bigInstrumentImage != null && bigImagePanel != null)
        {
            bigInstrumentImage.sprite = instrumentSprites[currentIndex];
            bigImagePanel.SetActive(true);
        }
    }

    public void NextInstrument()
    {
        if (isPaused) return;

        currentIndex = (currentIndex + 1) % instrumentNames.Length;
        UpdateInstrument(true);
    }

    public void PreviousInstrument()
    {
        if (isPaused) return;

        currentIndex = (currentIndex - 1 + instrumentNames.Length) % instrumentNames.Length;
        UpdateInstrument(true);
    }

    void PlayInstrumentSound(int index)
    {
        if (instrumentSource == null || index < 0 || index >= instrumentSounds.Length || instrumentSounds[index] == null)
            return;

        instrumentSource.Stop();
        instrumentSource.clip = instrumentSounds[index];
        instrumentSource.Play();
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEditor;
using System;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _document;
    public String PlayScene;

    // Buttons
    public Button PlayButton;
    public Button CreditsButton;
    public Button SettingsButton;
    public Button QuitButton;
    public Button SettingsBackButton;
    public Button CreditsBackButton;
    public Button MenuBackButton;
    public Button MenuButton;
    public Toggle FullScreenToggle;

    // Panels
    public VisualElement settingsPanel;
    public VisualElement mainMenuPanel;
    public VisualElement creditPanel;
    public VisualElement menuPanel;

    // Sounds
    public AudioClip buttonClickSound;
    private AudioSource audioSource;

    // Camera
    public Camera mainCamera;
    public Vector3 initialPosition;
    public Vector3 initialRotation;

    // Objects toggled when going to credits
    public GameObject[] ParticleMenu;
    public GameObject[] ParticleCredits;

    // Camera rotation
    [SerializeField] private float rotationDuration = 1f;

    private List<Button> _menuButtons = new List<Button>();

    // Credits Scroller
    [Header("Credits Scroll")]
    public RectTransform creditsCanvas;
    public Vector3 creditsStartPos;
    public Vector3 creditsEndPos;
    public float normalSpeed = 20f;
    public float fastSpeed = 60f;

    private Coroutine scrollRoutine;

    void Awake()
    {
        _document = GetComponent<UIDocument>();
        audioSource = GetComponent<AudioSource>();

        // Grab buttons
        PlayButton = _document.rootVisualElement.Q("Play") as Button;
        CreditsButton = _document.rootVisualElement.Q("Credits") as Button;
        SettingsButton = _document.rootVisualElement.Q("Settings") as Button;
        QuitButton = _document.rootVisualElement.Q("Quit") as Button;
        SettingsBackButton = _document.rootVisualElement.Q("Settings_back") as Button;
        CreditsBackButton = _document.rootVisualElement.Q("Credit_back") as Button;
        MenuBackButton = _document.rootVisualElement.Q("Menu_back") as Button;
        MenuButton = _document.rootVisualElement.Q("Menu") as Button;
        FullScreenToggle = _document.rootVisualElement.Q("FullScreenMark") as Toggle;

        // Register button callbacks
        PlayButton.RegisterCallback<ClickEvent>(OnPlayGameClick);
        CreditsButton.RegisterCallback<ClickEvent>(OnCreditsClick);
        SettingsButton.RegisterCallback<ClickEvent>(OnSettingsClick);
        QuitButton.RegisterCallback<ClickEvent>(OnQuitClick);
        SettingsBackButton.RegisterCallback<ClickEvent>(OnBackToMenu);
        CreditsBackButton.RegisterCallback<ClickEvent>(OnBackToMenu);
        MenuBackButton.RegisterCallback<ClickEvent>(OnBackToMenu);
        MenuButton.RegisterCallback<ClickEvent>(OnMenu);

        // Grab panels
        settingsPanel = _document.rootVisualElement.Q("Settings_Panel") as VisualElement;
        mainMenuPanel = _document.rootVisualElement.Q("MainMenu_Panel") as VisualElement;
        creditPanel = _document.rootVisualElement.Q("Credit_Panel") as VisualElement;
        menuPanel = _document.rootVisualElement.Q("Menu_Panel") as VisualElement;

        // Initial state
        settingsPanel.style.display = DisplayStyle.None;
        creditPanel.style.display = DisplayStyle.None;
        menuPanel.style.display = DisplayStyle.None;
        mainMenuPanel.style.display = DisplayStyle.Flex;

        // Add click sound to all buttons
        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();
        foreach (var btn in _menuButtons)
        {
            btn.RegisterCallback<ClickEvent>(OnAllButtonClick);
        }

        // Setup camera initial state
        if (mainCamera != null)
        {
            mainCamera.transform.position = initialPosition;
            mainCamera.transform.rotation = Quaternion.Euler(initialRotation);
            mainCamera.orthographic = true;
        }

        foreach (GameObject obj in ParticleMenu)
            if (obj != null) obj.SetActive(true);

        foreach (GameObject obj in ParticleCredits)
            if (obj != null) obj.SetActive(false);

        // Sync fullscreen toggle at start
        if (FullScreenToggle != null)
        {
            FullScreenToggle.value = Screen.fullScreen;
            FullScreenToggle.RegisterValueChangedCallback(evt =>
            {
                Screen.fullScreen = evt.newValue;
                Debug.Log("Fullscreen toggled: " + evt.newValue);
            });
        }

        if (creditsCanvas != null)
            creditsCanvas.localPosition = creditsStartPos;
    }

    private void OnDisable()
    {
        PlayButton.UnregisterCallback<ClickEvent>(OnPlayGameClick);
        CreditsButton.UnregisterCallback<ClickEvent>(OnCreditsClick);
        SettingsButton.UnregisterCallback<ClickEvent>(OnSettingsClick);
        QuitButton.UnregisterCallback<ClickEvent>(OnQuitClick);
        SettingsBackButton.UnregisterCallback<ClickEvent>(OnBackToMenu);
        CreditsBackButton.UnregisterCallback<ClickEvent>(OnBackToMenu);
        MenuBackButton.UnregisterCallback<ClickEvent>(OnBackToMenu);
        MenuButton.UnregisterCallback<ClickEvent>(OnMenu);

        foreach (var btn in _menuButtons)
        {
            btn.UnregisterCallback<ClickEvent>(OnAllButtonClick);
        }
    }

    private void OnPlayGameClick(ClickEvent evt)
    {
        Debug.Log("Start Game");
       SceneManager.LoadScene(PlayScene);
    }

    private void OnMenu(ClickEvent evt)
    {
        creditPanel.style.display = DisplayStyle.None;
        settingsPanel.style.display = DisplayStyle.None;
        mainMenuPanel.style.display = DisplayStyle.None;
        menuPanel.style.display = DisplayStyle.Flex;
    }
    private void OnCreditsClick(ClickEvent evt)
    {
        creditPanel.style.display = DisplayStyle.Flex;
        settingsPanel.style.display = DisplayStyle.None;
        mainMenuPanel.style.display = DisplayStyle.None;
        menuPanel.style.display = DisplayStyle.None;

        if (mainCamera != null)
        {
            StopAllCoroutines();
            StartCoroutine(RotateCamera(new Vector3(90f, 0f, 0f), rotationDuration, false));
            mainCamera.orthographic = false;
        }

        foreach (var obj in ParticleMenu)
            if (obj != null) obj.SetActive(false);

        foreach (var obj in ParticleCredits)
            if (obj != null) obj.SetActive(true);

        StartCreditsScroll();
    }

    private void OnSettingsClick(ClickEvent evt)
    {
        settingsPanel.style.display = DisplayStyle.Flex;
        creditPanel.style.display = DisplayStyle.None;
        mainMenuPanel.style.display = DisplayStyle.None;
        menuPanel.style.display = DisplayStyle.None;

    }

    private void OnBackToMenu(ClickEvent evt)
    {
        settingsPanel.style.display = DisplayStyle.None;
        creditPanel.style.display = DisplayStyle.None;
        mainMenuPanel.style.display = DisplayStyle.Flex;
        menuPanel.style.display = DisplayStyle.None;


        if (mainCamera != null)
        {
            StopAllCoroutines();
            StartCoroutine(RotateCamera(initialRotation, rotationDuration, true));
        }

        foreach (GameObject obj in ParticleMenu)
            if (obj != null) obj.SetActive(true);

        foreach (GameObject obj in ParticleCredits)
            if (obj != null) obj.SetActive(false);

        if (creditsCanvas != null)
            creditsCanvas.localPosition = creditsStartPos;

        if (scrollRoutine != null)
        {
            StopCoroutine(scrollRoutine);
            scrollRoutine = null;
        }
    }

    private void OnQuitClick(ClickEvent evt)
    {
        Debug.Log("Quit Game");
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }

    private void OnAllButtonClick(ClickEvent evt)
    {
        if (buttonClickSound != null)
            audioSource.PlayOneShot(buttonClickSound);
    }

    private IEnumerator RotateCamera(Vector3 targetRotation, float duration, bool orth)
    {
        Quaternion startRot = mainCamera.transform.rotation;
        Quaternion endRot = Quaternion.Euler(targetRotation);
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / duration;
            mainCamera.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }
        mainCamera.orthographic = orth;
    }

    private void StartCreditsScroll()
    {
        if (creditsCanvas == null) return;

        if (scrollRoutine != null)
            StopCoroutine(scrollRoutine);

        creditsCanvas.localPosition = creditsStartPos;
        scrollRoutine = StartCoroutine(ScrollCredits());
    }

    private IEnumerator ScrollCredits()
    {
        while (true)
        {
            float speed = normalSpeed;
            Vector3 target = creditsEndPos;

            if (Input.GetKey(KeyCode.Space))
            {
                speed = fastSpeed;
                target = creditsEndPos;
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                speed = normalSpeed;
                target = creditsStartPos;
            }

            creditsCanvas.localPosition = Vector3.MoveTowards(
                creditsCanvas.localPosition,
                target,
                speed * Time.deltaTime
            );

            yield return null;
        }
    }
}

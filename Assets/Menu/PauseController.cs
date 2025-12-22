using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [Header("UI (found per scene)")]
    [SerializeField] private CanvasGroup pauseGroup;
    [SerializeField] private GameObject firstSelected;

    [Header("Input")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private string gameplayMap = "Player";
    [SerializeField] private string uiMap = "UI";

    [Header("Optional: disable gameplay scripts while menu is open")]
    [SerializeField] private Behaviour[] disableOnPause;

    [Header("Freeze world (optional)")]
    [SerializeField] private bool freezeTime = false; // если включишь — будет Time.timeScale = 0

    private static PauseController _instance;
    private bool _menuOpen;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        if (!playerInput) playerInput = GetComponent<PlayerInput>();
        _menuOpen = false;

        if (freezeTime) Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RebindPauseUIFromScene();
        HideMenuInstant();
        SwitchToGameplayInput();
    }

    private void RebindPauseUIFromScene()
    {
        var pausePanelGO = GameObject.Find("PausePanel");
        pauseGroup = pausePanelGO ? pausePanelGO.GetComponent<CanvasGroup>() : null;

        var resumeBtnGO = GameObject.Find("ResumeButton");
        firstSelected = resumeBtnGO;

        if (pauseGroup != null)
            pauseGroup.transform.SetAsLastSibling();
    }

    // ===== These methods are used by PlayerInput "Invoke Unity Events" =====
    public void PauseFromInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (_menuOpen) return;
        ShowMenu();
    }

    public void ResumeFromInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (!_menuOpen) return;
        HideMenu();
    }

    // Hook for Resume button (OnClick)
    public void ResumeFromButton() => HideMenu();

    private void ShowMenu()
    {
        _menuOpen = true;

        if (freezeTime) Time.timeScale = 0f;

        SetPauseUI(true);
        SetCursor(true);
        SetGameplayScripts(false);
        SwitchToUIInput();

        if (firstSelected != null && EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    private void HideMenu()
    {
        _menuOpen = false;

        if (freezeTime) Time.timeScale = 1f;

        SetPauseUI(false);
        SetCursor(false);
        SetGameplayScripts(true);
        SwitchToGameplayInput();
    }

    private void HideMenuInstant()
    {
        _menuOpen = false;

        if (freezeTime) Time.timeScale = 1f;

        SetPauseUI(false);
        SetCursor(false);
        SetGameplayScripts(true);
    }

    private void SetPauseUI(bool visible)
    {
        if (pauseGroup == null) return;

        if (visible) pauseGroup.transform.SetAsLastSibling();

        pauseGroup.alpha = visible ? 1f : 0f;
        pauseGroup.interactable = visible;
        pauseGroup.blocksRaycasts = visible;
    }

    private void SetGameplayScripts(bool enabled)
    {
        if (disableOnPause == null) return;
        for (int i = 0; i < disableOnPause.Length; i++)
            if (disableOnPause[i] != null)
                disableOnPause[i].enabled = enabled;
    }

    private void SetCursor(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void SwitchToUIInput()
    {
        if (playerInput != null)
            playerInput.SwitchCurrentActionMap(uiMap);
    }

    private void SwitchToGameplayInput()
    {
        if (playerInput != null)
            playerInput.SwitchCurrentActionMap(gameplayMap);
    }

    public void ExitToMenu(string sceneName)
    {
        if (freezeTime) Time.timeScale = 1f;
        _menuOpen = false;
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        if (freezeTime) Time.timeScale = 1f;
        _menuOpen = false;
        Application.Quit();
    }
}

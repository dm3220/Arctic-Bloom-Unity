using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string newGameSceneName = "GameScene";

    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject helpPanel;

    public void Start()
    {
        ShowMainMenu();
    }

    public void NewRun()
    {
        SceneManager.LoadScene(newGameSceneName);
    }

    public void ContinueGame()
    {
        Debug.Log("Continue");
    }

    public void OpenSettings()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingPanel != null) settingPanel.SetActive(true);
    }

    public void OpenHelp()
    {
        if (helpPanel != null) helpPanel?.SetActive(true);
    }

    public void CloseHelp()
    {
        if (helpPanel != null) helpPanel?.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("ExitGame CLICKED");

    #if UNITY_EDITOR
        // В редакторе — просто выходим из Play Mode
        UnityEditor.EditorApplication.isPlaying = false;
    #else
    // В любом нормальном билде (Windows, Linux, Android и т.д.)
    Application.Quit();
    #endif
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
    }

    private void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (settingPanel != null) settingPanel.SetActive(false);
    }

    public void BackFromSettings()
    {
        ShowMainMenu();
    }
}

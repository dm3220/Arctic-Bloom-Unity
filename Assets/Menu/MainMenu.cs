using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string newGameSceneName = "GameScene";

    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingPanel;

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

    }

    public void ExitGame()
    {
        Application.Quit();
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

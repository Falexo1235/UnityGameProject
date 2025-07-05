using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    [Header("Main Menu Buttons")]
    public UnityEngine.UI.Button playButton;
    public UnityEngine.UI.Button settingsButton;
    public UnityEngine.UI.Button exitButton;

    [Header("Settings Panel")]
    public UnityEngine.UI.Button backToMainButton;
    public UnityEngine.UI.Button resetSettingsButton;

    [Header("Settings Reference")]
    public GameSettings gameSettings;

    private void Start()
    {
        if (playButton != null) playButton.onClick.AddListener(StartGame);
        if (settingsButton != null) settingsButton.onClick.AddListener(OpenSettings);
        if (exitButton != null) exitButton.onClick.AddListener(ExitGame);
        if (backToMainButton != null) backToMainButton.onClick.AddListener(BackToMainMenu);
        if (resetSettingsButton != null) resetSettingsButton.onClick.AddListener(ResetSettings);

        ShowMainMenu();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void OpenSettings()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    public void ResetSettings()
    {
        if (gameSettings != null)
        {
            gameSettings.ResetToDefaults();
        }
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }
}
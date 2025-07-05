using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject victoryPanel;
    public GameObject defeatPanel;
    public GameObject menuPanel;

    [Header("UI Elements")]
    public UnityEngine.UI.Button restartButton;
    public UnityEngine.UI.Button mainMenuButton;

    private void Start()
    {
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
        menuPanel.SetActive(false);
        restartButton.onClick.AddListener(RestartLevel);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    public void ShowVictoryMenu()
    {
        victoryPanel.SetActive(true);
        menuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowDefeatMenu()
    {

        defeatPanel.SetActive(true);
        menuPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
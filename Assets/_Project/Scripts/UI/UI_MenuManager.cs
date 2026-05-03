using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UI_MenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _upgradePanel;
    [SerializeField] private GameObject _gameOverPanel;

    [Header("GameOver References")]
    [SerializeField] private TMP_Text _timeSurvivedText;
    [SerializeField] private TMP_Text _bestTimeText;

    [Header("Pause References")]
    [SerializeField] private TMP_Text _pauseTimeText;
    [SerializeField] private TMP_Text _pauseBestTimeText;

    private void Start()
    {
        // Panneli inizialmente spenti
        HideAllPanels();

        // Iscrivo evento
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }
    }

    private void OnDestroy()
    {
        // Disiscrivo evento per memory leak
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    private void HandleGameStateChanged(GameState newState)
    {
        HideAllPanels();

        switch (newState)
        {
            case GameState.Paused:
                if (_pausePanel) _pausePanel.SetActive(true);
                UpdatePauseTexts();
                break;
            case GameState.UpgradeMenu:
                if (_upgradePanel) _upgradePanel.SetActive(true);
                break;
            case GameState.GameOver:
                if (_gameOverPanel) _gameOverPanel.SetActive(true);
                UpdateGameOverTexts();
                break;
        }
    }

    private void HideAllPanels()
    {
        if (_pausePanel) _pausePanel.SetActive(false);
        if (_upgradePanel) _upgradePanel.SetActive(false);
        if (_gameOverPanel) _gameOverPanel.SetActive(false);
    }

    private void UpdatePauseTexts()
    {
        float survivedTime = GameManager.Instance.TotalTimeSurvived;
        float bestTime = PlayerPrefs.GetFloat("BestTime", 0f);

        SetOptimizedTimeText(_pauseTimeText, survivedTime);
        SetOptimizedTimeText(_pauseBestTimeText, bestTime);
    }

    private void UpdateGameOverTexts()
    {
        float survivedTime = GameManager.Instance.TotalTimeSurvived;
        float bestTime = PlayerPrefs.GetFloat("BestTime", 0f);

        SetOptimizedTimeText(_timeSurvivedText, survivedTime);
        SetOptimizedTimeText(_bestTimeText, bestTime);
    }

    private void SetOptimizedTimeText(TMP_Text targetText, float timeInSeconds)
    {
        int totalMinutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int hundredths = Mathf.FloorToInt((timeInSeconds % 1f) * 100f);

        if (targetText != null)
        {
            targetText.SetText(GameManager.TIME_FORMAT, totalMinutes, seconds, hundredths);
        }
    }

    public void Button_ResumeGame()
    {
        GameManager.Instance.ChangeState(GameState.Playing);
    }

    public void Button_RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void Button_QuitToMainMenu() // (EXIT)
    {
        Time.timeScale = 1f;
        GameManager.Instance.ChangeState(GameState.MainMenu);
        SceneManager.LoadScene(0);
    }
}

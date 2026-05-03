using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Default,
    MainMenu,
    Playing,
    Paused,
    UpgradeMenu,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton
    public GameState CurrentState { get; private set; } // Property

    public const string TIME_FORMAT = "{0:00}:{1:00}:{2:00}";

    public event Action<GameState> OnGameStateChanged;

    public float TotalTimeSurvived { get; private set; } // Property

    [Header("Audio Settings")]
    [SerializeField] private AudioClip _menuMusic;
    [Range(0f, 1f)][SerializeField] private float _menuVolume = 1f;

    [SerializeField] private AudioClip _gameMusic;
    [Range(0f, 1f)][SerializeField] private float _gameVolume = 0.3f;

    [SerializeField] private AudioClip _gameOverMusic;
    [Range(0f, 1f)][SerializeField] private float _gameOverVolume = 1f;

    private void Awake()
    {
        TotalTimeSurvived = 0f;

        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            ChangeState(GameState.MainMenu);
        }
        else
        {
            ChangeState(GameState.Playing);
        }
    }

    private void Update()
    {
        if (CurrentState == GameState.Playing)
        {
            TotalTimeSurvived += Time.deltaTime;
        }
    }

    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return;

        if (CurrentState == GameState.GameOver) return; // Check al GameOver da verificare

        if (CurrentState == GameState.UpgradeMenu && newState == GameState.Playing)
        {
            Debug.LogWarning("You must chose one upgrade!");
            return;
        }

        CurrentState = newState;

        ApplyTimeScale(newState);

        UpdateStateMusic(newState);

        if (newState == GameState.GameOver)
        {
            SaveBestTime();
        }

        OnGameStateChanged?.Invoke(newState);

        Debug.Log($"State changed in : {newState}"); // Check State
    }

    private void ApplyTimeScale(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
            case GameState.MainMenu:
                Time.timeScale = 1f; // Tempo normale
                break;
            case GameState.Paused:
            case GameState.UpgradeMenu:
            case GameState.GameOver:
                Time.timeScale = 0f; // Fermo il tempo
                break;
        }
    }

    private void UpdateStateMusic(GameState state)
    {
        if (AudioManager.Instance == null) return;

        switch (state)
        {
            case GameState.MainMenu:
                AudioManager.Instance.PlayMusic(_menuMusic, _menuVolume);
                break;

            case GameState.Playing:
                AudioManager.Instance.PlayMusic(_gameMusic, _gameVolume);
                break;

            case GameState.GameOver:
                if (_gameOverMusic != null)
                    AudioManager.Instance.PlayMusic(_gameOverMusic, _gameOverVolume);
                break;
        }
    }

    private void SaveBestTime()
    {
        float bestTime = PlayerPrefs.GetFloat("BestTime", 0f);
        if (TotalTimeSurvived > bestTime)
        {
            PlayerPrefs.SetFloat("BestTime", TotalTimeSurvived);
            PlayerPrefs.Save();
            Debug.Log($"New Record! {TotalTimeSurvived}");
        }
    }

    public void TogglePause()
    {
        if (CurrentState == GameState.Playing)
            ChangeState(GameState.Paused);
        else if (CurrentState == GameState.Paused)
            ChangeState(GameState.Playing);
    }
}

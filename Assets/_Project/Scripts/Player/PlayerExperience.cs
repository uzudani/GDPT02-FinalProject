using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _baseXpToLevelUp = 100;
    [SerializeField] private float _xpMultiplierPerLevel = 1.2f; // Incremento richiesta EXP per livello

    private int _currentLevel = 1;
    private int _currentXp = 0;
    private int _xpToNextLevel;

    private void Start()
    {
        _xpToNextLevel = _baseXpToLevelUp;

        if (UI_HUDManager.Instance != null)
        {
            UI_HUDManager.Instance.UpdateXP(_currentXp, _xpToNextLevel);
        }
    }

    // Funzione per Gemma XP
    public void AddXP(int amount)
    {
        _currentXp += amount;

        while (_currentXp >= _xpToNextLevel) // While per eventuali livellamenti a catena
        {
            LevelUp();
        }

        // Aggiornamento HUD XP
        if (UI_HUDManager.Instance != null)
        {
            UI_HUDManager.Instance.UpdateXP(_currentXp, _xpToNextLevel);
        }
    }

    private void LevelUp()
    {
        _currentLevel++;
        _currentXp -= _xpToNextLevel; // Eccesso XP conservato
        _xpToNextLevel = Mathf.RoundToInt(_xpToNextLevel * _xpMultiplierPerLevel); // Calcolo per nuova soglia livello

        if (UI_HUDManager.Instance != null)
        {
            UI_HUDManager.Instance.UpdateXP(_currentXp, _xpToNextLevel);
        }

        Debug.Log($"<color=yellow>LEVEL UP! Your level now is: {_currentLevel}. Next step: {_xpToNextLevel} XP</color>");

        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.AddPendingUpgrade();
        }
    }
}

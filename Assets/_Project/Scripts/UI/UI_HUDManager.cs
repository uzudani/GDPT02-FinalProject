using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HUDManager : MonoBehaviour
{
    public static UI_HUDManager Instance { get; private set; } // Singleton (passaggio dati)

    [Header("HUD Elements")]
    [SerializeField] private TMP_Text _waveTimerText;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _xpSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateTimer(float timeValue)
    {
        if (_waveTimerText == null) return;

        int totalMinutes = Mathf.FloorToInt(timeValue / 60f);
        int seconds = Mathf.FloorToInt(timeValue % 60f);
        int hundredths = Mathf.FloorToInt((timeValue % 1f) * 100f);

        _waveTimerText.SetText(GameManager.TIME_FORMAT, totalMinutes, seconds, hundredths);
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (_healthBar == null) return;

        _healthBar.maxValue = maxHealth;
        _healthBar.value = currentHealth;
    }

    public void UpdateXP(int currentXp, int targetXp)
    {
        if (_xpSlider != null)
        {
            _xpSlider.maxValue = targetXp;
            _xpSlider.value = currentXp;
        }
    }
}

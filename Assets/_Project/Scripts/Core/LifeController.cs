using System;
using UnityEngine;
public class LifeController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _baseMaxHp = 100f;
    [SerializeField] private bool _isPlayer = false;

    private float _currentMaxHp;
    private float _currentHp;
    private bool _isDead;
    private PlayerStats _stats;

    public event Action OnTakeDamage;
    public event Action OnDeath;

    public float LifePercentage => _currentMaxHp / _baseMaxHp;

    private void Start()
    {
        _currentMaxHp = _baseMaxHp;
        _currentHp = _currentMaxHp;
        _isDead = false;

        if (_isPlayer)
        {
            _stats = GetComponent<PlayerStats>();

            if (_stats == null)
            {
                Debug.LogWarning("PlayerStats not found on Player!"); // English Log
            }

            if (UI_HUDManager.Instance != null)
            {
                UI_HUDManager.Instance.UpdateHealth(_currentMaxHp, _baseMaxHp);
            }
        }
    }
    public void TakeDamage(float damage)
    {
        if (_isDead || _currentHp <= 0) return;

        if (_isPlayer && _stats != null)
        {
            damage *= _stats.DamageResistanceMultiplier;
        }

        _currentHp -= damage;

        if (_isPlayer && UI_HUDManager.Instance != null)
        {
            UI_HUDManager.Instance.UpdateHealth(_currentHp, _currentMaxHp);
        }

        OnTakeDamage?.Invoke();

        if (_currentHp <= 0)
        {
            Death();
        }
    }

    public void Heal(float amount)
    {
        if (_isDead) return;

        _currentHp += amount;

        if (_currentHp > _currentMaxHp)
        {
            _currentHp = _currentMaxHp;
        }

        if (_isPlayer && UI_HUDManager.Instance != null)
        {
            UI_HUDManager.Instance.UpdateHealth(_currentHp, _currentMaxHp);
        }

        Debug.Log($"<color=green>Heal {amount} HP! {_currentHp}/{_currentMaxHp}</color>");
    }

    public void SetMaxHp(float newMaxHp)
    {
        _baseMaxHp = newMaxHp;
        _currentMaxHp = _baseMaxHp;
        _currentHp = _currentMaxHp;
        _isDead = false;

        if (_isPlayer && UI_HUDManager.Instance != null)
        {
            UI_HUDManager.Instance.UpdateHealth(_currentMaxHp, _baseMaxHp);
        }
    }

    public void UpdateMaxHealthFromStats()
    {
        if (!_isPlayer || _stats == null) return;

        float oldMaxHp = _currentMaxHp;
        _currentMaxHp = _baseMaxHp * _stats.MaxHealthMultiplier;

        // Cura del player per differenza max HP
        float diff = _currentMaxHp - oldMaxHp;
        _currentHp += diff;

        if (UI_HUDManager.Instance != null)
        {
            UI_HUDManager.Instance.UpdateHealth(_currentHp, _currentMaxHp);
        }

        Debug.Log($"Max HP updated: {_currentMaxHp}");
    }

    private void Death()
    {
        if (_isDead) return;
        _isDead = true;

        OnDeath?.Invoke();

        if (_isPlayer)
        {
            PlayerController playerController = GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TriggerDeathAnimation();
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.ChangeState(GameState.GameOver);
            }
        }
        else
        {
            //Per nemici
            gameObject.SetActive(false);
        }

        //gameObject.SetActive(false);
    }
}

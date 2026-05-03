using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Current Multipliers")]
    [SerializeField] private float _moveSpeedMultiplier = 1f;
    [SerializeField] private float _damageMultiplier = 1f;
    [SerializeField] private float _maxHealthMultiplier = 1f;
    [SerializeField] private float _attackSpeedMultiplier = 1f;
    [SerializeField] private float _damageResistanceMultiplier = 1f;

    public float MoveSpeedMultiplier => _moveSpeedMultiplier;
    public float DamageMultiplier => _damageMultiplier;
    public float MaxHealthMultiplier => _maxHealthMultiplier;
    public float AttackSpeedMultiplier => _attackSpeedMultiplier;
    public float DamageResistanceMultiplier => _damageResistanceMultiplier;

    private void Start()
    {
        WeaponBase[] startingWeapons = GetComponentsInChildren<WeaponBase>();

        foreach (WeaponBase weapon in startingWeapons)
        {
            weapon.InitializeWeapon(this);
        }
    }

    public void AddMoveSpeed(float amount)
    {
        _moveSpeedMultiplier *= amount;
        Debug.Log($"New Move Speed Multiplier: {_moveSpeedMultiplier}");
    }

    public void AddDamage(float amount)
    {
        _damageMultiplier *= amount;
        Debug.Log($"New Damage Multiplier: {_damageMultiplier}");
    }

    public void AddMaxHealth(float amount)
    {
        _maxHealthMultiplier *= amount;
        Debug.Log($"New Max Health Multiplier: {_maxHealthMultiplier}");
    }

    public void AddAttackSpeed(float amount)
    {
        _attackSpeedMultiplier *= amount;
        Debug.Log($"New Attack Speed Multiplier: {_attackSpeedMultiplier}");
    }
    public void AddDamageResistance(float amount)
    {
        _damageResistanceMultiplier *= amount;
        Debug.Log($"New Damage Resistance Multiplier: {_damageResistanceMultiplier}");
    }
}

using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Weapon Configuration")]
    [SerializeField] protected SO_WeaponData _weaponData;

    protected Transform _currentTarget;
    protected float _currentCooldown;
    protected PlayerStats _playerStats;

    public void InitializeWeapon(PlayerStats stats)
    {
        _playerStats = stats;
    }

    public virtual float GetCalculatedDamage()
    {
        if (_weaponData == null) return 0f;

        float currentDamageMultiplier = _playerStats != null ? _playerStats.DamageMultiplier : 1f;
        return _weaponData.Damage * currentDamageMultiplier;
    }

    protected virtual void Update()
    {
        if (_weaponData == null) return;

        if (_currentCooldown > 0)
        {
            _currentCooldown -= Time.deltaTime;
        }
        else
        {
            FindClosestEnemy();

            if (_currentTarget != null)
            {
                // Calcoli centralizzati
                float currentAttackSpeed = _playerStats != null ? Mathf.Max(_playerStats.AttackSpeedMultiplier, 0.01f) : 1f;
                float currentDamageMultiplier = _playerStats != null ? _playerStats.DamageMultiplier : 1f;
                float finalDamage = _weaponData.Damage * currentDamageMultiplier;

                Fire(finalDamage);

                _currentCooldown = _weaponData.FireRate / currentAttackSpeed;
            }
        }
    }

    private void FindClosestEnemy()
    {
        float maxRange = 0f;

        // Casting per tipo di arma
        SO_RangedWeapon rangedData = _weaponData as SO_RangedWeapon;
        SO_MeleeWeapon meleeData = _weaponData as SO_MeleeWeapon;

        if (rangedData != null)
        {
            maxRange = rangedData.BulletSpeed * rangedData.Lifetime;
        }
        else if (meleeData != null)
        {
            maxRange = meleeData.AttackRadius;
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, maxRange);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hit.transform;
                }
            }
        }

        _currentTarget = closestEnemy;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            LifeController enemyLife = other.GetComponent<LifeController>();

            if (enemyLife != null)
            {
                enemyLife.TakeDamage(GetCalculatedDamage());
            }
        }
    }

    protected abstract void Fire(float calculatedDamage);
}

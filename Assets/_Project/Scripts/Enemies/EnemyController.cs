using UnityEngine;

[RequireComponent(typeof(LifeController))]
[RequireComponent(typeof(EnemyAI))]
public class EnemyController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _damageTickRate = 1f; // Danno ogni 1 secondo
    [SerializeField] private Transform _dropPoint;

    private LifeController _lifeController;
    private EnemyAI _enemyAI;
    private float _currentDamage;
    private float _nextDamageTime;

    // Salvataggio dati per Loot-Table
    private SO_EnemyData _myData;

    private void Awake()
    {
        _lifeController = GetComponent<LifeController>();
        _enemyAI = GetComponent<EnemyAI>();
    }

    private void OnEnable()
    {
        _lifeController.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        _lifeController.OnDeath -= HandleDeath;
    }

    public void Initialize(SO_EnemyData data, float multiplier)
    {
        _myData = data; // Memorizzo "ID" nemico

        float finalHp = data.BaseHp * multiplier;
        float finalSpeed = data.BaseSpeed * multiplier;
        _currentDamage = data.BaseDamage * multiplier;

        _lifeController.SetMaxHp(finalHp);
        _enemyAI.SetSpeed(finalSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LifeController playerLife = other.GetComponent<LifeController>();

            if (playerLife != null)
            {
                playerLife.TakeDamage(_currentDamage);

                _nextDamageTime = Time.time + _damageTickRate;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time >= _nextDamageTime)
            {
                LifeController playerLife = other.GetComponent<LifeController>();

                if (playerLife != null)
                {
                    playerLife.TakeDamage(_currentDamage);

                    _nextDamageTime = Time.time + _damageTickRate;
                }
            }
        }
    }

    private void HandleDeath()
    {
        if (_myData != null && _myData.DeathSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayAudio(_myData.DeathSound, _myData.TargetChannel, _myData.DeathSoundVolume);
        }

        if (_myData == null || _myData.Drops == null || _myData.Drops.Length == 0) return;

        Vector3 spawnPosition = _dropPoint != null ? _dropPoint.position : transform.position + Vector3.up;

        foreach (SO_BaseDrop drop in _myData.Drops)
        {
            float roll = Random.Range(0f, 100f);

            if (roll <= drop.DropChance)
            {
                GameObject dropObj = ObjectPoolManager.Instance.SpawnObject(
                    drop.Prefab,
                    spawnPosition,
                    drop.Prefab.transform.rotation
                );

                Collectible collectible = dropObj.GetComponent<Collectible>();
                if (collectible != null)
                {
                    collectible.Initialize(drop);
                }
            }
        }
    }
}

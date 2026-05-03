using UnityEngine;

[System.Serializable]
public class Wave // "Regole" ondata
{
    public string waveName;
    public SO_EnemyData[] enemyDatas;
    public float spawnRate;
    public float waveDuration;
    public float statsMultiplier = 1f;
}

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private Wave[] _waves;
    [SerializeField] private Transform _player;
    [SerializeField] private float _spawnRadius = 15f;

    [Header("Spawn Zone")]
    [SerializeField] private Collider _playableSpawnArea;

    [Header("Cycle Loop Settings")]
    [SerializeField] private float _difficultyIncreasePerCycle = 0.25f; // % stats a wave
    [SerializeField] private float _minSpawnRate = 0.1f; // Limite sicurezza velocita' spawn per bug e crash

    private int _currentWaveIndex = 0;
    private float _nextSpawnTime;
    private float _waveEndTime;
    private float _globalDifficultyMultiplier = 1f; // Difficolta' nel tempo

    private void Start()
    {
        if (_waves.Length == 0 || _player == null)
        {
            Debug.LogError("Check waves or Player in WaveManager!");
            enabled = false;
            return;
        }

        StartWave(_currentWaveIndex);
    }

    private void Update()
    {
        TimeAndWavesLogic();
    }

    private void StartWave(int index)
    {
        Debug.Log($"Start Wave: {_waves[index].waveName} - Global Multiplier: {_globalDifficultyMultiplier}");

        _waveEndTime = Time.time + _waves[index].waveDuration; // Timer ondata attuale
        _nextSpawnTime = Time.time;
    }

    private void TimeAndWavesLogic()
    {
        // UI Time
        if (UI_HUDManager.Instance != null && GameManager.Instance != null)
        {
            UI_HUDManager.Instance.UpdateTimer(GameManager.Instance.TotalTimeSurvived);
        }

        // Gestione ondate
        if (Time.time >= _waveEndTime)
        {
            _currentWaveIndex++;

            if (_currentWaveIndex >= _waves.Length) // Loop ondate con incremento difficolta'
            {
                _currentWaveIndex = 0;
                _globalDifficultyMultiplier += _difficultyIncreasePerCycle;
                Debug.Log($"Increasing difficulty to x{_globalDifficultyMultiplier}");
            }

            StartWave(_currentWaveIndex);
        }

        // Logica spawn
        if (Time.time >= _nextSpawnTime)
        {
            SpawnEnemy();

            float currentSpawnRate = _waves[_currentWaveIndex].spawnRate / _globalDifficultyMultiplier; // Calcolo e velocizzo tempo spawn in base a ondate e difficolta'
            currentSpawnRate = Mathf.Max(currentSpawnRate, _minSpawnRate); // Condizione per non scendere sotto il limite

            _nextSpawnTime = Time.time + currentSpawnRate;
        }
    }
    private void SpawnEnemy() // Focus attorno al player in una determinata zona e inizializzo grazie a ObjectPool
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized * _spawnRadius;
        Vector3 spawnPosition = _player.position + new Vector3(randomCircle.x, 0f, randomCircle.y);

        if (_playableSpawnArea != null) // Limite area
        {
            spawnPosition = _playableSpawnArea.ClosestPoint(spawnPosition); // Forzatura in caso di coordinate "fuori collider"

            spawnPosition.y = _player.position.y;
        }

        Wave currentWave = _waves[_currentWaveIndex];

        if (currentWave.enemyDatas == null || currentWave.enemyDatas.Length == 0)
        {
            Debug.LogWarning($"Attenzione: Nessun nemico assegnato alla Wave {currentWave.waveName}!");
            return;
        }

        int randomIndex = Random.Range(0, currentWave.enemyDatas.Length); // Momentaneamente random

        SO_EnemyData selectedEnemyData = currentWave.enemyDatas[randomIndex];

        GameObject enemyObj = ObjectPoolManager.Instance.SpawnObject(
            selectedEnemyData.EnemyPrefab,
            spawnPosition,
            Quaternion.identity
        );

        EnemyController enemyScript = enemyObj.GetComponent<EnemyController>();
        if (enemyScript != null)
        {
            float finalMultiplier = currentWave.statsMultiplier * _globalDifficultyMultiplier;

            enemyScript.Initialize(selectedEnemyData, finalMultiplier);
        }
    }
}

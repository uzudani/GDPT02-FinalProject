using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemy Data")]
public class SO_EnemyData : ScriptableObject
{
    [Header("Base Info")]
    [SerializeField] private string _enemyName = "New Enemy";

    [Header("Base Stats")]
    [SerializeField] private float _baseHp = 100f;
    [SerializeField] private float _baseDamage = 10f;
    [SerializeField] private float _baseSpeed = 3f;

    [Header("Visuals")]
    [SerializeField] private GameObject _enemyPrefab;

    [Header("Loot Table")]
    [SerializeField] private SO_BaseDrop[] _drops;

    [Header("Audio")]
    [SerializeField] private AudioClip _deathSound;
    [Range(0f, 1f)]
    [SerializeField] private float _deathSoundVolume = 0.5f;
    [SerializeField] private AudioChannel _targetChannel = AudioChannel.PitchSFX;

    public string EnemyName => _enemyName;
    public float BaseHp => _baseHp;
    public float BaseDamage => _baseDamage;
    public float BaseSpeed => _baseSpeed;
    public GameObject EnemyPrefab => _enemyPrefab;
    public SO_BaseDrop[] Drops => _drops;
    public AudioClip DeathSound => _deathSound;
    public float DeathSoundVolume => _deathSoundVolume;
    public AudioChannel TargetChannel => _targetChannel;
}
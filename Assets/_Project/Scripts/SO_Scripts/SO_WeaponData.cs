using UnityEngine;

public abstract class SO_WeaponData : ScriptableObject
{
    [Header("Base Info")]
    [SerializeField] private string _weaponName = "New Weapon";

    [Header("Base Stats")]
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _fireRate = 1f;

    [Header("Base Audio")]
    [SerializeField] private AudioChannel _targetChannel = AudioChannel.Weapon;
    [SerializeField] private AudioClip _useSound;
    [Range(0f, 1f)] // Slider
    [SerializeField] private float _soundVolume = 0.5f;
    [SerializeField] private bool _loopSound = false;

    public string WeaponName => _weaponName;
    public float Damage => _damage;
    public float FireRate => _fireRate;
    public AudioChannel TargetChannel => _targetChannel;
    public AudioClip UseSound => _useSound;
    public bool LoopSound => _loopSound;
    public float SoundVolume => _soundVolume;
}

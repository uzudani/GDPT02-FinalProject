using UnityEngine;

public abstract class SO_BaseDrop : ScriptableObject
{
    [Header("Info")]
    [SerializeField] private string _itemName;
    [SerializeField] private GameObject _prefab;

    [Header("Audio")]
    [SerializeField] private AudioClip _pickupSound;
    [Range(0f, 1f)]
    [SerializeField] private float _soundVolume = 0.5f;
    [SerializeField] private AudioChannel _targetChannel = AudioChannel.PitchSFX;

    [Header("Drop Chance")]
    [Range(0f, 100f)]
    [SerializeField] private float _dropChance;

    public string ItemName => _itemName;
    public GameObject Prefab => _prefab;
    public AudioClip PickupSound => _pickupSound;
    public AudioChannel TargetChannel => _targetChannel;
    public float SoundVolume => _soundVolume;
    public float DropChance => _dropChance;

    // Funzione per i child personalizzabile
    public abstract void ApplyEffect(GameObject collector);
}

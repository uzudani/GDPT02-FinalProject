using UnityEngine;

public class OrbitalWeapon : WeaponBase
{
    [Header("Audio Settings")]
    [SerializeField] private UnityEngine.Audio.AudioMixerGroup _weaponMixerGroup;

    private AudioSource _weaponAudioSource;

    private void Start()
    {
        _weaponAudioSource = gameObject.AddComponent<AudioSource>();
        _weaponAudioSource.playOnAwake = false;
        _weaponAudioSource.spatialBlend = 0f;

        if (_weaponMixerGroup != null)
        {
            _weaponAudioSource.outputAudioMixerGroup = _weaponMixerGroup;
        }

        if (_weaponData != null && _weaponData.UseSound != null)
        {
            _weaponAudioSource.clip = _weaponData.UseSound;
            _weaponAudioSource.loop = _weaponData.LoopSound;
            _weaponAudioSource.volume = _weaponData.SoundVolume;
            _weaponAudioSource.Play();
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged += HandleGameState;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged -= HandleGameState;
        }
    }

    private void HandleGameState(GameState newState)
    {
        if (newState == GameState.GameOver)
        {
            if (_weaponAudioSource != null)
            {
                _weaponAudioSource.Stop();
            }
        }

        else if (newState == GameState.Paused)
        {
            _weaponAudioSource?.Pause();
        }
        else if (newState == GameState.Playing)
        {
            _weaponAudioSource?.UnPause();
        }
    }

    protected override void Update()
    {
        if (_weaponData == null || _playerStats == null) return;

        SO_OrbitalWeapon orbitalData = _weaponData as SO_OrbitalWeapon;
        if (orbitalData == null) return;

        transform.position = _playerStats.transform.position;

        float currentAttackSpeed = Mathf.Max(_playerStats.AttackSpeedMultiplier, 0.01f);
        float finalRotationSpeed = orbitalData.RotationSpeed * currentAttackSpeed;

        transform.Rotate(Vector3.up * finalRotationSpeed * Time.deltaTime);
    }

    protected override void Fire(float calculatedDamage) { }
}

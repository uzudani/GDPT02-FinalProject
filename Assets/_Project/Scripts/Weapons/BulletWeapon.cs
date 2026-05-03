using UnityEngine;

public class BulletWeapon : WeaponBase
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform _firePoint;

    protected override void Fire(float calculatedDamage)
    {
        // CASTING: Dico al codice di leggere _weaponData come un SO_RangedWeapon
        SO_RangedWeapon rangedData = _weaponData as SO_RangedWeapon;

        if (rangedData == null)
        {
            Debug.LogError("Chack Weapon Type!");
            return;
        }

        if (rangedData.BulletPrefab == null || _firePoint == null)
        {
            Debug.LogWarning("Bullet prefab or FirePoint missing!");
            return;
        }

        _firePoint.LookAt(_currentTarget.position);

        GameObject bullet = ObjectPoolManager.Instance.SpawnObject(
            rangedData.BulletPrefab,
            _firePoint.position,
            _firePoint.rotation
        );

        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.Initialize(rangedData.BulletSpeed, calculatedDamage, rangedData.Lifetime);
        }

        if (AudioManager.Instance != null && _weaponData.UseSound != null)
        {
            AudioManager.Instance.PlayAudio(_weaponData.UseSound, _weaponData.TargetChannel, _weaponData.SoundVolume);
        }
    }
}

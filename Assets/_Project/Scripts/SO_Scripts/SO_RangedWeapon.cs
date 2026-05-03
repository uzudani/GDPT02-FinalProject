using UnityEngine;

[CreateAssetMenu(fileName = "NewRangedWeapon", menuName = "Weapons/Ranged Weapon")]
public class SO_RangedWeapon : SO_WeaponData
{
    [Header("Ranged Settings")]
    [SerializeField] private float _bulletSpeed = 20f;
    public float BulletSpeed => _bulletSpeed;

    [SerializeField] private float _lifetime = 3f;
    public float Lifetime => _lifetime;

    [SerializeField] private GameObject _bulletPrefab;
    public GameObject BulletPrefab => _bulletPrefab;
}

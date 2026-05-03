using UnityEngine;

[CreateAssetMenu(fileName = "NewOrbitalWeapon", menuName = "Weapons/Orbital Weapon")]
public class SO_OrbitalWeapon : SO_WeaponData
{
    [Header("Orbital Settings")]
    [SerializeField] private float _rotationSpeed = 150f;
    public float RotationSpeed => _rotationSpeed;
}

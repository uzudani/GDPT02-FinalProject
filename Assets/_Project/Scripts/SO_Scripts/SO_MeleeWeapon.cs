using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeWeapon", menuName = "Weapons/Melee Weapon")]
public class SO_MeleeWeapon : SO_WeaponData
{
    [Header("Melee Settings")]
    [SerializeField] private float _attackRadius = 2f;
    public float AttackRadius => _attackRadius;

    [SerializeField] private float _knockbackForce = 5f;
    public float KnockbackForce => _knockbackForce;
}

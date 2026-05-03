using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponUpgrade", menuName = "ScriptableObjects/Upgrades/Weapon Upgrade")]
public class SO_WeaponUpgrade : SO_Upgrade
{
    [Header("Weapon Settings")]
    [SerializeField] private GameObject _weaponPrefabToEquip;

    public override void ApplyUpgrade(GameObject player)
    {
        if (_weaponPrefabToEquip != null)
        {
            GameObject newWeapon = Instantiate(_weaponPrefabToEquip, player.transform.position, Quaternion.identity);

            WeaponBase weaponScript = newWeapon.GetComponent<WeaponBase>(); // Check WeaponBase e passaggio statistiche

            PlayerStats stats = player.GetComponent<PlayerStats>();

            if (weaponScript != null && stats != null)
            {
                weaponScript.InitializeWeapon(stats);
            }
        }
    }
}

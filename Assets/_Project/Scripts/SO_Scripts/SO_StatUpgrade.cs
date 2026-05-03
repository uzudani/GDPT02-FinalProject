using UnityEngine;

[CreateAssetMenu(fileName = "NewStatUpgrade", menuName = "ScriptableObjects/Upgrades/Stat Upgrade")]
public class SO_StatUpgrade : SO_Upgrade
{
    public enum StatToUpgrade { MoveSpeed, Damage, MaxHealth, AttackSpeed, DamageResistance, Heal }

    [Header("Stat Settings")]
    [SerializeField] private StatToUpgrade _targetStat;
    [SerializeField] private float _statMultiplier = 1.1f;

    public override void ApplyUpgrade(GameObject player)
    {
        PlayerStats stats = player.GetComponent<PlayerStats>();
        LifeController life = player.GetComponent<LifeController>();

        if (stats != null)
        {
            switch (_targetStat)
            {
                case StatToUpgrade.MoveSpeed:
                    stats.AddMoveSpeed(_statMultiplier);
                    break;
                case StatToUpgrade.Damage:
                    stats.AddDamage(_statMultiplier);
                    break;
                case StatToUpgrade.MaxHealth:
                    stats.AddMaxHealth(_statMultiplier);
                    if (life != null) life.UpdateMaxHealthFromStats();
                    break;
                case StatToUpgrade.AttackSpeed:
                    stats.AddAttackSpeed(_statMultiplier);
                    break;
                case StatToUpgrade.DamageResistance:
                    stats.AddDamageResistance(_statMultiplier);
                    break;
                case StatToUpgrade.Heal:
                    if (life != null)
                    {
                        life.Heal(_statMultiplier);
                    }
                    break;
            }
        }
        else
        {
            Debug.LogWarning("PlayerStats component missing on Player!");
        }
    }
}

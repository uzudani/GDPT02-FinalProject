using UnityEngine;

// Enum per rarita'
public enum UpgradeRarity { Common, Rare, Epic, Legendary }

public abstract class SO_Upgrade : ScriptableObject
{
    [Header("UI Info")]
    [SerializeField] private string _upgradeName;
    [SerializeField] private Sprite _icon;
    [TextArea(3, 5)][SerializeField] private string _description;

    [Header("Rarity")]
    [SerializeField] private UpgradeRarity _rarity;

    public string UpgradeName => _upgradeName;
    public Sprite Icon => _icon;
    public string Description => _description;
    public UpgradeRarity Rarity => _rarity;
    public abstract void ApplyUpgrade(GameObject player);
}

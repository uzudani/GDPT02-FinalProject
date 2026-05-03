using UnityEngine;

[CreateAssetMenu(fileName = "NewHealDrop", menuName = "ScriptableObjects/Drops/Heal Drop")]
public class SO_HealDrop : SO_BaseDrop
{
    [Header("Heal Settings")]
    public float HealAmount;

    public override void ApplyEffect(GameObject collector)
    {
        Debug.Log($"<color=green>Hai raccolto {ItemName}. +{HealAmount} HP!</color>");

        // Cura Player
        // collector.GetComponent<LifeController>().TakeDamage(-HealAmount); 
    }
}

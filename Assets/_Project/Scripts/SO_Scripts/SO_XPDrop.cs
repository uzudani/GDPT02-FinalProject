using UnityEngine;

[CreateAssetMenu(fileName = "NewXPDrop", menuName = "ScriptableObjects/Drops/XP Drop")]
public class SO_XPDrop : SO_BaseDrop
{
    [Header("Impostazioni XP")]
    public int XpAmount;

    public override void ApplyEffect(GameObject collector)
    {
        PlayerExperience playerXp = collector.GetComponent<PlayerExperience>();

        if (playerXp != null)
        {
            // Paddaggio punti XP
            playerXp.AddXP(XpAmount);
            Debug.Log($"<color=cyan>Hai raccolto {ItemName}. +{XpAmount} XP!</color>");
        }
    }
}

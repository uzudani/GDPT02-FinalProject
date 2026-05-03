using UnityEngine;

[CreateAssetMenu(fileName = "ColorPalette", menuName = "ScriptableObjects/Config/Color Palette")]
public class SO_ColorPalette : ScriptableObject
{
    [Header("Rarity Colors")]
    public Color common = Color.gray;
    public Color rare = Color.blue;
    public Color epic = Color.magenta;
    public Color legendary = Color.yellow;

    [Header("UI Colors")]
    public Color xpBarColor;
    public Color healthBarColor;
}

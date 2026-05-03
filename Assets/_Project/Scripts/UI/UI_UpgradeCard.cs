using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeCard : MonoBehaviour
{
    [Header("Riferimenti UI")]
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _backgroundImage;

    private SO_Upgrade _currentUpgradeData;

    // Call per Manager (Data)
    public void Initialize(SO_Upgrade data, Color rarityColor)
    {
        _currentUpgradeData = data;

        // Corrispondenze SO
        _nameText.text = data.UpgradeName;
        _descriptionText.text = data.Description;
        _iconImage.sprite = data.Icon;

        // Colore per rarita'
        _backgroundImage.color = rarityColor;
    }

    public void OnCardClicked()
    {
        if (_currentUpgradeData != null)
        {
            UpgradeManager.Instance.SelectUpgrade(_currentUpgradeData);
        }
    }
}

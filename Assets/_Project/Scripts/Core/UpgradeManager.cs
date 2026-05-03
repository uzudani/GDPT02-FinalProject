using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject _upgradePanel;
    [SerializeField] private UI_UpgradeCard[] _cardsUI;
    [SerializeField] private SO_ColorPalette _colorPalette;

    [Header("Data")]
    [SerializeField] private List<SO_Upgrade> _allAvailableUpgrades;
    [SerializeField] private SO_Upgrade _fallbackUpgrade;

    private List<SO_Upgrade> _runtimeAvailableUpgrades;
    private int _pendingUpgrades = 0; // Aggiornamenti in sospeso

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (_upgradePanel != null) _upgradePanel.SetActive(false);
    }

    private void Start()
    {
        _runtimeAvailableUpgrades = new List<SO_Upgrade>(_allAvailableUpgrades); // Copia carte in mazzo attivo
    }

    public void AddPendingUpgrade()
    {
        _pendingUpgrades++; // Aggiunta livello in coda

        if (!_upgradePanel.activeSelf) // Condizione per pannello non attivo
        {
            ShowUpgradeScreen();
        }
    }

    public void ShowUpgradeScreen()
    {
        Time.timeScale = 0f;

        List<SO_Upgrade> selectedUpgrades = GetRandomUpgrades(3);

        while (selectedUpgrades.Count < _cardsUI.Length)
        {
            selectedUpgrades.Add(_fallbackUpgrade);
        }

        for (int i = 0; i < _cardsUI.Length; i++)
        {
            SO_Upgrade currentData = selectedUpgrades[i];

            // Colore in base alla palette
            Color cardColor = Color.white;
            if (_colorPalette != null)
            {
                switch (currentData.Rarity)
                {
                    case UpgradeRarity.Common: cardColor = _colorPalette.common; break;
                    case UpgradeRarity.Rare: cardColor = _colorPalette.rare; break;
                    case UpgradeRarity.Epic: cardColor = _colorPalette.epic; break;
                    case UpgradeRarity.Legendary: cardColor = _colorPalette.legendary; break;
                }
            }

            _cardsUI[i].gameObject.SetActive(true);
            _cardsUI[i].Initialize(currentData, cardColor);
        }

        _upgradePanel.SetActive(true);
    }

    // Pesca random (MODIFICA FUTURA: Peso o Percentuale?)
    private List<SO_Upgrade> GetRandomUpgrades(int count)
    {
        List<SO_Upgrade> selection = new List<SO_Upgrade>();
        List<SO_Upgrade> copyList = new List<SO_Upgrade>(_runtimeAvailableUpgrades);

        for (int i = 0; i < count; i++)
        {
            if (copyList.Count == 0) break;

            int randomIndex = Random.Range(0, copyList.Count);
            selection.Add(copyList[randomIndex]);

            // Rimozione temporanea per evitare duplicati
            copyList.RemoveAt(randomIndex);
        }

        return selection;
    }

    public void SelectUpgrade(SO_Upgrade selectedUpgrade)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            selectedUpgrade.ApplyUpgrade(player);
        }
        else
        {
            Debug.LogError("Player not found!");
        }

        // Rimozione carta solo se non e' premio di consolazione
        if (selectedUpgrade != _fallbackUpgrade && _runtimeAvailableUpgrades.Contains(selectedUpgrade))
        {
            _runtimeAvailableUpgrades.Remove(selectedUpgrade);
        }

        _pendingUpgrades--; // Riduzione post scelta

        if (_pendingUpgrades > 0)
        {
            ShowUpgradeScreen();
        }
        else
        {
            CloseUpgradePanel();
        }
    }

    private void CloseUpgradePanel()
    {
        _upgradePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text _bestTimeText;

    private void Start()
    {
        UpdateBestTimeDisplay();
    }

    private void UpdateBestTimeDisplay()
    {
        // Recupero save
        float bestTime = PlayerPrefs.GetFloat("BestTime", 0f);

        if (bestTime <= 0f)
        {
            if (_bestTimeText != null) _bestTimeText.text = "--:--:--"; // Prima partita
            return;
        }

        int totalMinutes = Mathf.FloorToInt(bestTime / 60f);
        int seconds = Mathf.FloorToInt(bestTime % 60f);
        int hundredths = Mathf.FloorToInt((bestTime % 1f) * 100f);

        if (_bestTimeText != null)
        {
            _bestTimeText.SetText(GameManager.TIME_FORMAT, totalMinutes, seconds, hundredths);
        }
    }

    public void Button_PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Button_QuitGame()
    {
        Debug.Log("Try to quit!");
        Application.Quit();
    }
}

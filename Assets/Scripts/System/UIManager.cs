using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text temperatureText;
    [SerializeField] private TMP_Text energyText;

    private GameManager gm;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        TrySubscribe();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Unsubscribe();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // На всякий случай переподключаемся после загрузки сцены
        TrySubscribe();

        if (gm != null)
        {
            // Проталкиваем текущие значения сразу в UI
            UpdateTime(gm.GetTimeString());
            UpdateTemperature(gm.temperature);
            UpdateEnergy(gm.energy);
        }
    }

    private void TrySubscribe()
    {
        // Если уже подписаны на актуальный GameManager — ничего не делаем
        if (gm == GameManager.Instance && gm != null)
            return;

        Unsubscribe();

        gm = GameManager.Instance;
        if (gm == null)
        {
            // GameManager ещё не успел проинициализироваться
            Debug.LogWarning("[UIManager] GameManager.Instance == null, жду…");
            return;
        }

        gm.OnTimeChanged += UpdateTime;
        gm.OnTemperatureChanged += UpdateTemperature;
        gm.OnEnergyChanged += UpdateEnergy;

        Debug.Log("[UIManager] Подписался на события GameManager");
    }

    private void Unsubscribe()
    {
        if (gm == null) return;

        gm.OnTimeChanged -= UpdateTime;
        gm.OnTemperatureChanged -= UpdateTemperature;
        gm.OnEnergyChanged -= UpdateEnergy;
        gm = null;
    }

    // ----- Handlers -----

    private void UpdateTime(string timeStr)
    {
        if (timeText != null)
            timeText.text = $"Time: {timeStr}";
    }

    private void UpdateTemperature(float temp)
    {
        if (temperatureText != null)
            temperatureText.text = $"Temperature:\n{Mathf.RoundToInt(temp)}°C";
    }

    private void UpdateEnergy(float energy)
    {
        if (energyText != null)
            energyText.text = $"Energy:\n{Mathf.RoundToInt(energy)}%";
    }
}

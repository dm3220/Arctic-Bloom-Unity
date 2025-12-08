using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ---------- Singleton ----------
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ---------- Time ----------
    [Header("Time")]
    [SerializeField] private int hours = 0;
    [SerializeField] private int minutes = 0;
    public event Action<string> OnTimeChanged;

    /// <summary>Добавить минуты (используется циклом дня/ночи).</summary>
    public void AddMinutes(int add)
    {
        int total = hours * 60 + minutes + add;
        if (total < 0) total = (24 * 60 + (total % (24 * 60))) % (24 * 60);
        hours = (total / 60) % 24;
        minutes = total % 60;
        OnTimeChanged?.Invoke(GetTimeString());
    }

    public string GetTimeString()
    {
        return $"{hours:00}:{minutes:00}";
    }

    // ---------- Temperature ----------
    [Header("Temperature")]
    [Tooltip("Текущая температура, °C")]
    public float temperature = 0f;
    public event Action<float> OnTemperatureChanged;

    /// <summary>Изменить температуру на value (может быть отрицательной).</summary>
    public void ChangeTemperature(float value)
    {
        temperature += value;
        OnTemperatureChanged?.Invoke(temperature);
    }

    // ---------- Energy ----------
    [Header("Energy")]
    [Tooltip("Текущая энергия, % (0..100)")]
    public float energy = 100f;
    public event Action<float> OnEnergyChanged;

    /// <summary>Изменить энергию на delta (отриц. = расход, полож. = пополнение).</summary>
    public void ChangeEnergy(float delta)
    {
        energy = Mathf.Clamp(energy + delta, 0f, 100f);
        OnEnergyChanged?.Invoke(energy);
    }

    [Header("Energy drain params")]
    [Tooltip("Базовая скорость убывания энергии в %/сек.")]
    public float baseEnergyDrainPerSec = 0.3f;

    [Tooltip("Множитель убывания энергии, когда игрок в укрытии (0.2 = в 5 раз медленнее).")]
    [Range(0f, 1f)] public float shelterDrainMultiplier = 0.2f;

    // ---------- Shelter ----------
    [Header("Shelter")]
    [Tooltip("Находится ли игрок в укрытии.")]
    public bool inShelter = false;
    public event Action<bool> OnShelterChanged;

    /// <summary>Установить состояние 'в укрытии'.</summary>
    public void SetShelter(bool isInShelter)
    {
        if (inShelter == isInShelter) return;
        inShelter = isInShelter;
        OnShelterChanged?.Invoke(inShelter);
        Debug.Log(inShelter ? "Игрок вошел в укрытие" : "Игрок вышел из укрытия");
    }

    // ---------- Initial push to UI ----------
    private void Start()
    {
        // Сразу отправляем текущие значения в HUD,
        // чтобы тексты обновились на старте сцены.
        OnTimeChanged?.Invoke(GetTimeString());
        OnTemperatureChanged?.Invoke(temperature);
        OnEnergyChanged?.Invoke(energy);
        OnShelterChanged?.Invoke(inShelter);
    }
}

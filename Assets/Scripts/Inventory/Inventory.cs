using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    public ItemStack[] slots = new ItemStack[12];
    public int activeIndex = 0;

    public event Action OnChanged;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        // Инициализируем массив
        for (int i = 0; i < slots.Length; i++)
            slots[i] = new ItemStack();
    }

    public void SetActive(int index)
    {
        activeIndex = Mathf.Clamp(index, 0, slots.Length - 1);
        OnChanged?.Invoke();
    }

    public ItemStack GetActive() => slots[activeIndex];

    public bool Add(Item item, int amount = 1)
    {
        if (item == null || amount <= 0) return false;

        // 1) Стекуем в существующие стеки
        for (int i = 0; i < slots.Length && amount > 0; i++)
        {
            var s = slots[i];
            if (s.item == item && s.count < item.maxStack)
            {
                int canPut = Mathf.Min(amount, item.maxStack - s.count);
                s.count += canPut;
                amount -= canPut;
            }
        }

        // 2) Кладём в пустые ячейки
        for (int i = 0; i < slots.Length && amount > 0; i++)
        {
            var s = slots[i];
            if (s.IsEmpty)
            {
                int put = Mathf.Min(amount, item.maxStack);
                s.item = item;
                s.count = put;
                amount -= put;
            }
        }

        OnChanged?.Invoke();
        return amount == 0; // true, если всё поместили
    }

    public bool RemoveFromActive(int amount = 1)
    {
        var s = GetActive();
        if (s.IsEmpty || amount <= 0) return false;

        s.count -= amount;
        if (s.count <= 0) s.Clear();

        OnChanged?.Invoke();
        return true;
    }

    public void ClearAll()
    {
        for (int i = 0; i < slots.Length; i++) slots[i].Clear();
        OnChanged?.Invoke();
    }
}

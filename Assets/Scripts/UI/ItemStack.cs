using System;
using UnityEngine;

[Serializable]
public struct ItemStack
{
    public Item item;     // тип с заглавной буквы
    public int count;

    public bool IsEmpty => item == null || count <= 0;

    public void Clear()
    {
        item = null;
        count = 0;
    }

    // опционально: удобные методы
    public int Add(int amount)
    {
        if (item == null || amount <= 0) return 0;
        int space = Mathf.Max(0, item.maxStack - count);
        int add = Mathf.Min(space, amount);
        count += add;
        return amount - add; // остаток
    }

    public int Remove(int amount)
    {
        if (IsEmpty || amount <= 0) return 0;
        int rem = Mathf.Min(count, amount);
        count -= rem;
        if (count <= 0) item = null;
        return rem;
    }
}

using UnityEngine;
using System;

[System.Serializable]
public class ItemStack//одна ячейка инвентаря
{
    public ItemData item;
    public int count;

    public bool isEmpty
    {
        get { return item == null || count <= 0; }
    }

    public void Clear()
    {
        item = null; count = 0;
    }
}
public class Inventory : MonoBehaviour// логика инвентаря
{
    public static Inventory Instance {  get; private set; }

    [Header("Drop settings")]
    [SerializeField] private GameObject pickupPrefab;
    [SerializeField] private Transform playerTransform;

    public ItemStack[] slots1 = new ItemStack[8];//8 слотов

    public int activeIndex = 0;//какой слот активный

    public event Action OnChanged;

    [Header("UI Slots")]
    public InventorySlotUI[] slots;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        for (int i = 0; i < slots1.Length; i++)
        {
            slots1[i] = new ItemStack();
        }
    }

    public void SetActive(int index)
    {
        activeIndex = Mathf.Clamp(index, 0, slots1.Length - 1);
        OnChanged?.Invoke();
    }

    public ItemStack GetActive()
    {
        return slots1[activeIndex];
    }

    public bool Add(ItemData item, int amount = 1)
    {
        if (item == null || amount <= 0)
            return false;

        for (int i = 0; i < slots1.Length && amount > 0; i++)
        {
            ItemStack s = slots1[i];

            if (s.item == item && s.count < item.maxStack)
            {
                int free = item.maxStack - s.count;
                int toAdd = Mathf.Min(amount, free);

                s.count += toAdd;
                amount -= toAdd;
            }
        }

        for (int i = 0; i < slots1.Length && amount > 0; i++)
        {
            ItemStack s = slots1[i];

            if (s.isEmpty)
            {
                int toAdd = Mathf.Min(amount, item.maxStack);

                s.item = item;
                s.count = toAdd;

                amount -= toAdd;
            }
        }

        OnChanged?.Invoke();

        return amount == 0;
    }

    public bool RemoveFromSlot(int index, int amount = 1)
    {
        if (index < 0 || index >= slots1.Length) return false;

        ItemStack s = slots1[index];

        if (s.isEmpty || amount <= 0)
        {
            return false;
        }

        s.count -= amount;

        if (s.count <= 0)
        {
            s.Clear();
        }

        OnChanged?.Invoke();
        return true;
    }

    public bool RemoveFromActive(int amount = 1)
    {
        return RemoveFromSlot(activeIndex, amount);
    }

    public void DropItem(int index)
    {
        if (index < 0 || index >= slots1.Length)
        {
            Debug.LogWarning("DropFromSlot: неверный индекс слота " + index);
            return;
        }

        ItemStack s = slots1[index];

        if (s.isEmpty)
        {
            Debug.Log("DropFromSlot: слот пуст, дроп невозможен");
            return;
        }

        if (pickupPrefab == null)
        {
            Debug.LogWarning("DropFromSlot: pickupPrefab не назначен в инспекторе");
            return;
        }

        if (playerTransform == null)
        {
            Debug.LogWarning("DropFromSlot: playerTransform не назначен в инспекторе");
            return;
        }

        ItemData item = s.item;
        int amount = s.count;

        bool removed = RemoveFromSlot(index, amount);

        if (!removed)
        {
            Debug.Log("DropFromSlot: не удалось удалить предмет из инвентаря, дроп отменен");
            return;
        }

        Vector3 spawnPos = playerTransform.position + new Vector3(2f,1f,0f);

        GameObject go = Instantiate(pickupPrefab, spawnPos, Quaternion.identity);

        SimplePickupItem pickup = go.GetComponent<SimplePickupItem>();
        if (pickup != null)
        {
            pickup.item = item;
            pickup.amount = amount;
        }
    }

    public void RefreshUI()
    {
        if (slots == null || slots.Length == 0)
            return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < slots1.Length && slots1[i] != null && !slots1[i].isEmpty)
            {
                slots[i].SetItem(slots1[i].item, slots1[i].count);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform slotsParent;      // сам Hotbar (контейнер с GridLayoutGroup)
    public GameObject slotPrefab;      // префаб "Slot"

    Inventory inv;
    List<SlotUI> slotUIs = new List<SlotUI>();

    void Awake()
    {
        inv = Inventory.Instance;
        if (inv == null) inv = FindObjectOfType<Inventory>();

        BuildSlots();
    }

    void OnEnable()
    {
        if (inv != null) inv.OnChanged += RefreshAll;
        RefreshAll();
    }

    void OnDisable()
    {
        if (inv != null) inv.OnChanged -= RefreshAll;
    }

    void BuildSlots()
    {
        // очистим детей, если есть
        for (int i = slotsParent.childCount - 1; i >= 0; i--)
            Destroy(slotsParent.GetChild(i).gameObject);

        slotUIs.Clear();

        for (int i = 0; i < 12; i++)
        {
            var go = Instantiate(slotPrefab, slotsParent);
            go.name = $"Slot_{i + 1}";
            var ui = go.GetComponent<SlotUI>();
            if (ui == null) ui = go.AddComponent<SlotUI>();
            ui.AutoWire();
            ui.index = i;
            slotUIs.Add(ui);
        }
    }

    public void RefreshAll()
    {
        if (inv == null || inv.slots == null || inv.slots.Length == 0) return;

        for (int i = 0; i < slotUIs.Count; i++)
        {
            var ui = slotUIs[i];
            var stack = inv.slots[i];
            bool selected = (i == inv.activeIndex);
            ui.Refresh(stack, selected);
        }
    }
}

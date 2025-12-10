using UnityEngine;

public class InventoryUI : MonoBehaviour//связь между Inventory и виртуальными слотами
{
    public Inventory inventory;
    public InventorySlotUI[] slotUIs;

    private void Start()
    {
        if (inventory == null)
            inventory = Inventory.Instance;

        if (inventory != null )
        {
            inventory.OnChanged += Refresh;
        }

        Refresh();
    }

    private void OnDestroy()
    {
        if ( inventory != null )
        {
            inventory.OnChanged -= Refresh;
        }
    }

    private void Refresh()
    {
        if (inventory == null || slotUIs == null)
            return;

        int len = Mathf.Min(slotUIs.Length, inventory.slots1.Length);

        for (int i = 0; i < len; i++)
        {
            slotUIs[i].SetItem(inventory.slots1[i]);
        }
    }
}

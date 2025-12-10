using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler//отображение одного слота в интерфейсе
{
    [Header("Index in inventory")]
    public int index;

    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text amountText;

    [Header("Item data")]
    public ItemData item;
    public int amount;

    public void SetItem(ItemStack stack)
    {
        if (stack == null || stack.isEmpty)
        {
            ClearSlot();
        }
        else
        {
            SetItem(stack.item, stack.count);
        }
    }

    public void SetItem(ItemData newItem, int newAmount)
    {
        item = newItem;
        amount = newAmount;

        if (item != null && amount > 0)
        {
            icon.enabled = true;
            icon.sprite = item.icon;
            amountText.text = amount > 1 ? amount.ToString() : "";
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        item = null;
        amount = 0;
        icon.enabled = false;
        icon.sprite = null;
        amountText.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null || amount <= 0)
        {
            Debug.Log("Клик по пустому слоту");
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("ЛКМ по слоту, дропаем: " + item.name);
            TryDropItem();
        }
    }

    public void TryDropItem()
    {
        if (item == null || amount <= 0)
        {
            Debug.Log("Слот пуст, дроп невозможен");
            return;
        }

        if (Inventory.Instance == null)
        {
            Debug.LogWarning("Inventory.Instance == null");
            return;
        }

        Inventory.Instance.DropItem(index); 

        ClearSlot();
    }
}
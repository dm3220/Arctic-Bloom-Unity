using UnityEngine;

public class SimplePickupItem : MonoBehaviour, IInteractable//предмет на земле который можно поднять
{
    public ItemData item;
    public int amount = 1;

    public void Interact()
    {
        if (Inventory.Instance == null)
        {
            Debug.LogWarning("Inventory.Instance == null");
            return;
        }

        bool added = Inventory.Instance.Add(item, amount);

        if (added)
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Инвентарь заполнен, предмет не вмещается");
        }
    }
}

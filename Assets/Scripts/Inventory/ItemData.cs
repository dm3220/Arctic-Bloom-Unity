using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]//описание предметов
public class ItemData : ScriptableObject
{
    public string id;//уникальный id
    public string displayName;//имя ui
    public Sprite icon;//иконка слота
    public int maxStack = 10;//максимум штук
}

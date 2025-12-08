using UnityEngine;

[CreateAssetMenu(menuName = "ArcticBloom/Item", fileName = "NewItem")]
public class Item : ScriptableObject
{
    [Header("ID и визуал")]
    public string id;
    public string displayName;
    public Sprite icon;

    [Header("Стек")]
    public bool stackable = true;
    public int maxStack = 99;
}

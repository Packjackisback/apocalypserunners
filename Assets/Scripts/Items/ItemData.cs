using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    [Header("Basic Info")]
    public string itemID;
    public string displayName;
    public Sprite icon;

    [Header("Inventory")]
    public ItemType itemType;
    public int maxStack = 1;
}

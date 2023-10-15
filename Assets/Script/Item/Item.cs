using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    EQUIP,
    USE,
    ETC
}
[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject
{

    public ItemType itemType = ItemType.ETC;
    public string itemName = "∫”¿∫ ≤…";
    public string itemExplanation = "∫”¿∫ªˆ¿ª ∂Á¥¬ ≤…¿Ã¥Ÿ.";
    public Sprite itemSprite;

    public bool IsOverlappingItem()
    {
        if (itemType == ItemType.ETC)
            return true;
        if (itemType == ItemType.USE)
            return true;
        return false;
    }

    public bool Equals(Item item)
    {
        if (item == null)
            return false;
        Debug.Log("item.itemName:" + item.itemName);
        Debug.Log("itemName:" + itemName);
        return item.itemName == itemName;
    }
}

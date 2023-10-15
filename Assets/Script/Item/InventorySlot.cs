using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class InventorySlot : MonoBehaviour
{
    private Item item;
    private int number;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Sprite defaltImage;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text numberText;

    public void SetSlot(Item item, int number)
    {
        this.item = item;
        this.number = number;
        image.sprite = item.itemSprite;
        nameText.text = item.itemName;
        numberText.text = number.ToString();
    }
    public void ClearSlot()
    {
        this.item = null;
        this.number = 0;
        image.sprite = defaltImage;
        nameText.text = null;
        numberText.text = null;
    }
    public void SetSlot(InventorySlot inventorySlot)
    {
        if (inventorySlot == null)
            return;
        this.item = inventorySlot.item;
        this.number = inventorySlot.number;
        image.sprite = item.itemSprite;
        nameText.text = item.name;
        numberText.text = number.ToString();
    }

    public void AddNumber(int number)
    {
        this.number += number;
        numberText.text = this.number.ToString();
    }
    public void SubNumber(int number)
    {
        this.number -= number;
        if (this.number < 1)
            ClearSlot();
        else
            numberText.text = this.number.ToString();
    }

    public bool IsEmpty()
    {
        return item == null;
    }
    public Item GetItem()
    {
        return item;
    }
    public int GetNumber()
    {
        return number;
    }
}

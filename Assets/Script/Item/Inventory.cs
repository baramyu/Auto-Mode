using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public UnityEvent onChangeItem;


    public static Inventory instance;
    public static int MAX_ITEM_SLOT = 30;

    [SerializeField]
    private Transform content;
    [SerializeField]
    private InventorySlot inventorySlotPrefab;
    private InventorySlot[] inventorySlotArray = new InventorySlot[MAX_ITEM_SLOT];

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        for(int i = 0; i < MAX_ITEM_SLOT; i++)
        {
            InventorySlot inventorySlot = Instantiate(inventorySlotPrefab, content);
            inventorySlotArray[i] = inventorySlot;
            inventorySlot.SetSlot(DataManager.instance.inventorySlotArray[i]);
        }
    }

    public bool PickUpItem(Item item, int number)
    {
        if (item.IsOverlappingItem())
        {
            for (int i = 0; i < inventorySlotArray.Length; i++)
            {
                if (item.Equals(inventorySlotArray[i].GetItem()))
                {
                    inventorySlotArray[i].AddNumber(number);
                    onChangeItem.Invoke();
                    return true;
                }
            }
        }

        for (int i = 0; i < inventorySlotArray.Length; i++)
        {
            if (inventorySlotArray[i].IsEmpty())
            {
                inventorySlotArray[i].SetSlot(item, number);
                onChangeItem.Invoke();
                return true;
            }
        }

        return false; //아이템 획득 실패
    }
    public bool CutdownItem(Item item, int number)
    {
        for (int i = 0; i < inventorySlotArray.Length; i++)
        {
            if (item.Equals(inventorySlotArray[i].GetItem()))
            {
                if (item.IsOverlappingItem())
                    inventorySlotArray[i].SubNumber(number);
                else
                    inventorySlotArray[i].ClearSlot();
                onChangeItem.Invoke();
                return true;
            }
        }

        return false; //아이템 상실 실패
    }


    public int hasItem(Item item)
    {
        int number = 0;

        foreach(var itemSlot in inventorySlotArray)
        {
            if(item == itemSlot.GetItem())
                number += itemSlot.GetNumber();
        }

        return number;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [System.Serializable]
    public class InventorySlot
    {
        private int currentStack;
        [System.NonSerialized]
        private ItemData storedItem;

        [SerializeField]
        private int itemID;

        public InventorySlot(ItemData item, int quantity)
        {
            currentStack = quantity;
            storedItem = item;
        }

        // Create an empty slot by default
        public InventorySlot()
        {
            currentStack = 0;
        }

        public int CurrentStack
        {
            get { return this.currentStack; }
            set { this.currentStack = value; }
        }

        public ItemData StoredItem
        {
            get { return this.storedItem; }
            set
            {
                ItemData data = value;
                if (data == null)
                    itemID = -1;
                else
                    itemID = data.ItemID;

                this.storedItem = value;
            }
        }

        public int ItemID
        {
            get { return this.itemID; }
            set { this.itemID = value; }
        }


        public void EmptySlot()
        {
            currentStack = 0;
            storedItem = null;

        }

    }
    [SerializeField]
    private InventorySlot[] inventoryItemsArr;
    private const int maxNumberOfSlots = 100;

    public Inventory()
    {

        inventoryItemsArr = new InventorySlot[maxNumberOfSlots];

        for (int i = 0; i < maxNumberOfSlots; i++)
        {
            inventoryItemsArr[i] = new InventorySlot();
            inventoryItemsArr[i].ItemID = -1;
        }
    }




    public InventorySlot[] InventoryItemsArr
    {
        get { return this.inventoryItemsArr; }

    }




    public GameObject RetrieveItem(int index)
    {
        if (index < inventoryItemsArr.Length)
            return inventoryItemsArr[index].StoredItem.ObjectReference;
        else
            return null;
    }




    public void AddToInventory(ItemData item, int quantity = 1)
    {
        
        foreach (InventorySlot slot in InventoryItemsArr)
        {
            InventorySlot tempSlot = slot;

            //fill up the slot with the existing item
            if (tempSlot.StoredItem == null || item.ItemID == tempSlot.ItemID)
            {
                if (tempSlot.StoredItem == null)
                    tempSlot.StoredItem = item;


                int currentVal = tempSlot.CurrentStack + quantity;
                if (currentVal <= tempSlot.StoredItem.MaxStackSize)
                {
                    tempSlot.CurrentStack = currentVal;
                    break;
                }
                else
                {
                    quantity = currentVal - tempSlot.StoredItem.MaxStackSize;
                    tempSlot.CurrentStack = tempSlot.StoredItem.MaxStackSize;
                    continue;

                }

            }
        }
    }

    public void FetchAllStoredItemsFromID()
    {
        foreach (InventorySlot s in inventoryItemsArr)
        {
            s.StoredItem = ItemManager.Instance.GetItemData(s.ItemID);
        }
    }


}













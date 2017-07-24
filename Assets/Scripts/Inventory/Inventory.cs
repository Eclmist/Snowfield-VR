using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    [System.Serializable]
	public class InventorySlot
	{
		private int currentStack;

		private IStorable storedItem;

		public InventorySlot(IStorable item,int quantity)
		{
			currentStack = quantity;
			storedItem = item;
		}

        // An empty slot
        public InventorySlot()
        {
            currentStack = 0;
        }

		public int CurrentStack
		{
			get { return this.currentStack; }
			set { this.currentStack = value; }
		}

		public IStorable StoredItem
		{
			get { return this.storedItem; }
			set { this.storedItem = value; }
		}

        public void EmptySlot()
        {
            currentStack = 0;
            storedItem = null;

        }

	}

	private List<InventorySlot> inventoryItems = new List<InventorySlot>();

    
    public List<InventorySlot> InventoryItems
    {
        get { return this.inventoryItems; }

    }

    public InventorySlot this[int index]  
    {
        get { return this.inventoryItems[index]; }
    }


    public void AddToInventory(IStorable item)
    {

        bool added = false;

        foreach (InventorySlot slot in inventoryItems)
        {
            if (slot.CurrentStack == slot.StoredItem.MaxStackSize)
                continue;

            if(item.ItemID == slot.StoredItem.ItemID)
            {
                slot.CurrentStack++;
            }
        }

        // Add item to an empty slot
        if(!added)
        {
            InventorySlot slot = new InventorySlot(item, 1);
            inventoryItems.Add(slot);
        }

    }

   


    public GameObject RetrieveItem(int index)
    {
        if (index < inventoryItems.Count)
            return inventoryItems[index].StoredItem.ObjectReference;
        else
            return null;
    }



}

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

    private InventorySlot[] inventoryItemsArr;
    private const int maxNumberOfSlots = 100;

    public Inventory()
    {

        inventoryItemsArr = new InventorySlot[maxNumberOfSlots];

        for(int i = 0;i < maxNumberOfSlots; i++)
        {
            inventoryItemsArr[i] = new InventorySlot() ;
        }
    }

   


    public InventorySlot[] InventoryItemsArr
    {
        get { return this.inventoryItemsArr; }

    }

    //public InventorySlot this[int index]  
    //{
    //    get { return this.inventoryItems[index]; }
    //}


    public void AddToInventory(IStorable item)
    {
        bool added = false;
        for(int i = 0 ; i < inventoryItemsArr.Length; i++)
        {
            InventorySlot tempSlot = inventoryItemsArr[i];

            if (tempSlot.StoredItem != null)
                if(tempSlot.CurrentStack >= tempSlot.StoredItem.MaxStackSize)
                    continue;


            if(tempSlot.StoredItem != null)
            {
                if (item.ItemID == tempSlot.StoredItem.ItemID)
                {
                    tempSlot.CurrentStack++;
                    added = true;
                }
            }
            
        }

        // Add item to an empty slot
        if(!added)
        {   
            foreach(InventorySlot s in inventoryItemsArr)
            {
                if(s.StoredItem == null)
                {
                    s.StoredItem = item;
                    s.CurrentStack++;
                    break;
                }
            }

            
        }

    }

   

    public GameObject RetrieveItem(int index)
    {
        if (index < inventoryItemsArr.Length)
            return inventoryItemsArr[index].StoredItem.ObjectReference;
        else
            return null;
    }

 
 


}

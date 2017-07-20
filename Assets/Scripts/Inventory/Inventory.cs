using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

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

	[SerializeField]
	private int maxSlots;
	private InventorySlot[] inventoryItems ;

    protected virtual void Start()
	{
		inventoryItems = new InventorySlot[maxSlots];
        InitializeEmptySlots();
	}
		
 
    
	public InventorySlot[] InventoryItems
    {
        get { return this.inventoryItems; }
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
            foreach(InventorySlot slot in inventoryItems)
            {
                if(slot.StoredItem == null)
                {
                    slot.StoredItem = item;
                }
            }
        }

    }

	public GameObject RetrieveItem(int index)
    {
        if (index < inventoryItems.Length)
            return inventoryItems[index].StoredItem.ObjectReference;
        else
            return null;
    }


    private void InitializeEmptySlots()
    {
        for (int i = 0; i < inventoryItems.Length; i++)
            inventoryItems[i] = new InventorySlot();
    }






}

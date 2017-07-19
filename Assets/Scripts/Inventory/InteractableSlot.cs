using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableSlot : VR_Interactable_UI {

    private Image image;
    private Sprite itemDisplayIcon;
    private Text stack;

    Color temp;
    
    private Inventory.InventorySlot slot;

	// Use this for initialization
	void Start () {
        image = GetComponentInChildren<Image>();
        itemDisplayIcon = image.sprite;
        stack = GetComponentInChildren<Text>();
        DisplayInfo();
	}
	
	// Update is called once per frame
	void Update ()
    {
        base.Update();
 
	}

    public Inventory.InventorySlot Slot
    {
        get { return this.slot; }
        set { this.slot = value; }
    }

    private void DisplayInfo()
    {
        if (slot.StoredItem != null)
        {
            temp = image.color;
            temp.a = 1;
            image.color = temp;
            itemDisplayIcon = slot.StoredItem.Icon;
        }
        else
        {
            temp.a = 0;
            image.color = temp;
        }

        if (slot.CurrentStack != -1)
        {
            stack.gameObject.SetActive(true);
            stack.text = slot.CurrentStack.ToString();
        }
        else
            stack.gameObject.SetActive(false);

    }


    private void RemoveFromSlot(Transform t)
    {
        if(slot.StoredItem != null)
        {
            slot.CurrentStack--;
            if(slot.CurrentStack < 1)
            {
                slot.EmptySlot();
            }

            Instantiate(slot.StoredItem.ObjectReference,t);
            
        }
    }

  
    private void AddToSlot(IStorable item)
    {
        if(slot.StoredItem == null)
        {
            slot.StoredItem = item;
        }
        else if(slot.StoredItem.ItemID == item.ItemID)
        {
            if(slot.CurrentStack < slot.StoredItem.MaxStackSize)
            {
                Destroy(item.ObjectReference);
            }
            else
            {
                //show red outline
            }
        }
       
    }

    protected override void OnTriggerPress()
    {
        base.OnTriggerPress();
        
        // if controller is holding an item, call AddToSlot() *pass in the item it is holding*
        // else call RemoveFromSlot()
    }









}

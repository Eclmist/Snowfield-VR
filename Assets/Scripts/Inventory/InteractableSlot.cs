using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableSlot : VR_Interactable_UI
{

    private Image image;
    private Text stack;
    private VR_Interactable_Object pendingItem;
    private StoragePanel storagePanel;

    private Inventory.InventorySlot slot;

    // Use this for initialization
    void Start()
    {

        storagePanel = GetComponentInParent<StoragePanel>();
        GetComponent<BoxCollider>().isTrigger = true;
        image = GetComponentInChildren<Image>();
        stack = GetComponentInChildren<Text>();
    }

    protected override void Update()
    {
        base.Update();
        DisplayInfo();
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
            image.color = new Color(1, 1, 1, 1);
            stack.color = new Color(1, 1, 1, 1);
            //temp = image.color;
            //temp.a = 1;
            //image.color = temp;
            image.sprite = slot.StoredItem.Icon;
            stack.text = slot.CurrentStack.ToString();


        }
        else
        {
            image.color = new Color(0, 0, 0, 0);
            stack.color = new Color(0, 0, 0, 0);
        }

        

    }



    private void RemoveFromSlot()
    {
        if (storagePanel.SafeToUse)
        {
            if (slot.StoredItem != null)
            {
                slot.CurrentStack--;
                
                Debug.Log(slot.StoredItem.ObjectReference);
                VR_Interactable instanceInteractable = Instantiate(slot.StoredItem.ObjectReference).GetComponent<VR_Interactable>();
                currentInteractingController.SetInteraction(instanceInteractable);
                if (slot.CurrentStack < 1)
                {
                    slot.EmptySlot();
                }
            }
        }

    }


    private void AddToSlot(IStorable item)
    {

        if (storagePanel.SafeToUse)
        {
            if (slot.StoredItem == null)
            {
                Debug.Log(item.ObjectReference);
                slot.StoredItem = item;
                slot.CurrentStack++;
                currentInteractingController = null;
            }
            else if (slot.StoredItem.ItemID == item.ItemID)
            {
                if (slot.CurrentStack < slot.StoredItem.MaxStackSize)
                {
                    Debug.Log("Added");
                    slot.CurrentStack++;
                }
                else
                {
                    //show red outline
                }
            }
        }


    }

    //protected override void OnTriggerEnter(Collider other)
    //{
    //    storagePanel.NumberOfHoveredSlots++;
    //}

    //protected override void OnTriggerExit(Collider other)
    //{
    //    storagePanel.NumberOfHoveredSlots--;
    //}



    protected override void OnTriggerPress()
    {
        if (currentInteractingController.UI == this)
        {

            GenericItem g = currentInteractingController.GetComponentInChildren<GenericItem>();

            if (g)
            {
                ItemData d = ItemManager.Instance.GetItemData(g.ItemID);
                currentInteractingController.Model.SetActive(true);
                AddToSlot(d);
                Destroy(g.gameObject);
                
            }
            else
            {
                RemoveFromSlot();
            }


            // if controller is holding an item, call AddToSlot() *pass in the item it is holding*
            // else call RemoveFromSlot()
        }

    }

    protected override void OnControllerEnter()
    {
        base.OnControllerEnter();
        if (currentInteractingController)
        {
            currentInteractingController.UI = this;
            Debug.Log("asdsadasdsakjdghaskdjahsdkjashdjksad");
        }

    }

    protected override void OnControllerExit()
    {
        if (currentInteractingController.UI == this)
            currentInteractingController.UI = null;
        base.OnControllerExit();
    }








}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoragePanel : GenericItem {

    [SerializeField]
    private int inventorySize;

    private Inventory inventory;
    
	// Use this for initialization
	void Start ()
    {
        inventory = new Inventory(inventorySize);
	}
	
	

    void UpdateInventoryItems()
    {
        foreach(Inventory.InventorySlot slot in inventory.InventoryItems)
        {
            
        }




    }






}

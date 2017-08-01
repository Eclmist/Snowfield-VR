using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBuy : MonoBehaviour {

    private ItemData itemData;

    public ItemData _ItemData
    {
        get { return this.itemData; }
        set { this.itemData = value; }
    }

    public void SelectItem()
    {
        Merchant.Instance.BuyItem(itemData);
    }
	
	
}

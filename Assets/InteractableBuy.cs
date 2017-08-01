using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableBuy : MonoBehaviour {

    [SerializeField]
    private Image image;
    [SerializeField]
    private Text text;

    private ItemData itemData;

    public void Initialize(ItemData itemData)
    {
        this.itemData = itemData;
    }
    

    public void SelectItem()
    {
        Merchant.Instance.BuyItem(itemData);
    }
	
	
}

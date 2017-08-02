using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour {

    [SerializeField]
    private List<GenericItem> buyableGenericItems;
    [SerializeField]
    private MerchantPanel merchantPanel;
    [SerializeField]
    private float offsetFromMerchant;
    private List<ItemData> buyableItemData = new List<ItemData>();

	private MerchantPanel panelInstance = null;


    // Use this for initialization
    void Start ()
	{

        PopulateBuyableItemDataList();

	}


    public void SpawnMerchantPanel()
    {
		if(!panelInstance)
		{
			panelInstance = Instantiate(merchantPanel, transform.position + transform.forward * offsetFromMerchant, merchantPanel.transform.rotation);
			panelInstance.InitializeAndDisplayCatalog(buyableItemData);
		}
			
    }

	public void CloseMerchantPanel()
	{
		if(panelInstance)
			Destroy(panelInstance.gameObject);
	}


    // Retrieve the itemData for all buyable items
    private void PopulateBuyableItemDataList()
    {
        foreach (GenericItem g in buyableGenericItems)
        {
            ItemData data = ItemManager.Instance.GetItemData(g.gameObject);

            if (data != null)
                buyableItemData.Add(data);
        }
    }

   
}

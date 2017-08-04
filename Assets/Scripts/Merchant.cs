using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour {

    [SerializeField]
    private List<GenericItem> buyableGenericItems;
    [SerializeField]
    private MerchantPanel merchantPanel;
    [SerializeField]
    private Vector3 offsetFromMerchant;
    private List<ItemData> buyableItemData = new List<ItemData>();

	private MerchantPanel panelInstance = null;

    // Use this for initialization
    void Start ()
	{

        PopulateBuyableItemDataList();

	}


    public MerchantPanel SpawnMerchantPanel()
    {
		if(!panelInstance)
		{
			panelInstance = Instantiate(merchantPanel, transform.position + offsetFromMerchant, merchantPanel.transform.rotation);
			panelInstance.InitializeAndDisplayCatalog(buyableItemData);

            return merchantPanel;
		}

        return null;
			
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

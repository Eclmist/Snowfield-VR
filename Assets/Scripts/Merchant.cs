using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{

    [SerializeField]
    private List<GenericItem> buyableGenericItems;
    [SerializeField]
    private MerchantPanel merchantPanel;
    [SerializeField]
    private Vector3 offsetFromMerchant;
    private List<ItemData> buyableItemData = new List<ItemData>();

    // Use this for initialization
    protected void Start()
    {
        PopulateBuyableItemDataList();
    }
    
    public MerchantPanel SpawnMerchantPanel()
    {
        MerchantPanel panelInstance = Instantiate(merchantPanel, transform.position + (transform.forward * offsetFromMerchant.z)  + (transform.up * offsetFromMerchant.y), merchantPanel.transform.rotation);
        panelInstance.InitializeAndDisplayCatalog(buyableItemData);
		panelInstance.transform.LookAt(Player.Instance.transform);

        return panelInstance;
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
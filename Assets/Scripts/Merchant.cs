using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Merchant : MonoBehaviour
{
    public static Merchant Instance;

    [SerializeField]
    private GameObject interactableBuy;
    [SerializeField]
    private GridLayoutGroup layoutGroup;
    
    [SerializeField]
    private List<GenericItem> buyableGenericItems;
    private List<ItemData> buyableItemData;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        buyableItemData = new List<ItemData>();
        PopulateBuyableItemDataList();
        CreateCatalog();
	}
	

    private void PopulateBuyableItemDataList()
    {
        foreach(GenericItem g in buyableGenericItems)
        {
            ItemData data = ItemManager.Instance.GetItemData(g.gameObject);

            if(data != null)
                buyableItemData.Add(data);
        }
    }
    
    private void CreateCatalog()
    {
        foreach(GenericItem g in buyableGenericItems)
            Instantiate(interactableBuy, layoutGroup.transform, false);
    }

    public void BuyItem(ItemData data)
    {
       GameManager.Instance.AddPlayerGold(-data.Cost);
       TextSpawnerManager.Instance.SpawnText("-" + data.Cost.ToString(),Color.red,transform);
        StoragePanel.Instance._Inventory.AddToInventory(data);
    }


}

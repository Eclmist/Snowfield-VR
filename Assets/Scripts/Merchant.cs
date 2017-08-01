using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Merchant : MonoBehaviour
{
    public static Merchant Instance;

    [SerializeField]
    private InteractableBuy interactableBuy;
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
        //CreateCatalog();
	}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
            BuyItem(buyableItemData[0]);
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
        foreach(ItemData data in buyableItemData)
            Instantiate(interactableBuy, layoutGroup.transform, false).Initialize(data);
    }

    public void BuyItem(ItemData data)
    {
        if(Player.Instance.Gold <= data.Cost)
        {
            TextSpawnerManager.Instance.SpawnText("Not enough gold", Color.red, transform);
        }
        else
        {
            GameManager.Instance.AddPlayerGold(-data.Cost);
            TextSpawnerManager.Instance.SpawnText("-" + data.Cost.ToString() + "g", Color.red, transform);
            StoragePanel.Instance._Inventory.AddToInventory(data);
        }
        
    }

    


}

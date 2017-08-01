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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            BuyItem();
    }

    public void Initialize(ItemData itemData)
    {
        this.itemData = itemData;
        image.sprite = itemData.Icon;
        text.text = itemData.Cost.ToString() + " g";

    }


    public void BuyItem()
    {
        if (Player.Instance.Gold <= itemData.Cost)
        {
            TextSpawnerManager.Instance.SpawnText("Not enough gold", Color.red, transform);
        }
        else
        {
            GameManager.Instance.AddPlayerGold(-itemData.Cost);
            TextSpawnerManager.Instance.SpawnText("-" + itemData.Cost.ToString() + "g", Color.red, transform);
            StoragePanel.Instance._Inventory.AddToInventory(itemData,1);
        }

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {


    public static ItemManager Instance;

    public string path;

    [SerializeField]
    private List<ItemData> itemDataList = new List<ItemData>();

    private Dictionary<int, GameObject> itemDictionary = new Dictionary<int, GameObject>();
    private Dictionary<int, ItemData> itemDataDictionary = new Dictionary<int, ItemData>();


    public List<ItemData> ItemDataList
    {
        get { return this.itemDataList; }
    }

    public Dictionary<int, GameObject> ItemDictionary
    {
        get { return this.itemDictionary; }
    }

    public Dictionary<int, ItemData> ItemDataDictionary
    {
        get { return this.itemDataDictionary; }
    }

    public int NumberOfItems
    {
        get { return this.itemDictionary.Count; }
    }

    void Awake()
    {
        Instance = this;

		foreach (ItemData data in itemDataList)
		{
			itemDictionary.Add(data.ItemID, data.ObjectReference);
			itemDataDictionary.Add(data.ItemID, data);
		}
	}

	void Start()
    {
        
    }

    public GameObject SpawnItem(int id,Transform trans)
    {
        if(id < itemDictionary.Count)
        {
            return Instantiate(itemDictionary[id],trans);
        }
        else
        {
            return null;
        }
    }

    public GenericItem GetItem(int id)
    {
        if (id < itemDictionary.Count)
        {
            return itemDictionary[id].GetComponent<GenericItem>();
        }
        else
        {
            return null;
        }
    }

    public ItemData GetItemData(int id )
    {
        if (id < itemDataDictionary.Count)
        {
            return itemDataDictionary[id];
        }
        else
        {
            return null;
        }
    }

    public bool IsUnlocked(ItemData item)
    {
        return (item.LevelUnlocked >= Player.Instance.GetJob(item.ObjectReference.GetComponent<GenericItem>().JobType).Level);   
    }
    
    


 
}

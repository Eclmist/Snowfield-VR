using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCollector : MonoBehaviour, ICanSerialize
{

	protected List<GenericItem> allExistingItem = new List<GenericItem>();
    protected void Start()
    {
		GetExistingItems();
        Load();
    }

    public string SerializedFileName
    {
        get
        {
            return "SceneDataInfo";
        }
    }
    public void Save()
    {
        List<GenericItemSceneData> allData = new List<GenericItemSceneData>();
        GenericItem[] allObjects = FindObjectsOfType<GenericItem>();

        foreach (GenericItem item in allObjects)
        {
			if (allExistingItem.Contains(item))
				continue;

            if (item.isActiveAndEnabled && !item.GetComponent<DroppedItem>())
                allData.Add(item.GetSceneData());
        }
        SerializeManager.Save(SerializedFileName, allData);
    }

    

    public void Load()
    {
        List<GenericItemSceneData> allData = (List<GenericItemSceneData>)SerializeManager.Load(SerializedFileName);
		
        if (allData != null)
            foreach (GenericItemSceneData data in allData)
            {
                ItemData itemData = ItemManager.Instance.GetItemData(data.ID);
                if (itemData != null)
                {
                    GenericItem item = Instantiate(itemData.ObjectReference).GetComponent<GenericItem>();
                    item.SetupObject(data);
                }

            }
    }

	protected void GetExistingItems()
	{
		GenericItem[] allObjects = FindObjectsOfType<GenericItem>();
		foreach (GenericItem item in allObjects)
		{
			allExistingItem.Add(item);
		}
	}
}

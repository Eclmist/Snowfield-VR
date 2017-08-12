using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCollector : MonoBehaviour, ICanSerialize
{
    protected void Start()
    {
        Load();
    }

    public void Save()
    {
        List<GenericItemSceneData> allData = new List<GenericItemSceneData>();
        GenericItem[] allObjects = FindObjectsOfType<GenericItem>();
        foreach (GenericItem item in allObjects)
        {
            if (item.isActiveAndEnabled && !GetComponent<DroppedItem>())
                allData.Add(item.GetSceneData());
        }
        SerializeManager.Save("SceneDataInfo", allData);
    }

    public void Load()
    {
        List<GenericItemSceneData> allData = (List<GenericItemSceneData>)SerializeManager.Load("SceneDataInfo");
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
}

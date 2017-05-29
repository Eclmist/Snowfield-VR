using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {


    public static ItemManager Instance;

    [SerializeField]
    private List<GameObject> items = new List<GameObject>();

    private Dictionary<int, GameObject> itemDictionary = new Dictionary<int, GameObject>();


    public List<GameObject> Items
    {
        get { return this.items; }
    }

    public int NumberOfItems
    {
        get { return this.itemDictionary.Count; }
    }

    void Awake()
    {
        Instance = this;

        Object[] loadedStuff = Resources.LoadAll( "Items", typeof(GameObject));

        if (loadedStuff != null)
        {
            for(int i = 0;i < loadedStuff.Length;i++)
            {
                
                GameObject g = loadedStuff[i] as GameObject;
                g.GetComponent<GenericItem>().ID = i;
                items.Add(g);
                itemDictionary.Add(i,g);
            }

        }
        else
        {
            Debug.Log("Prefabs cant be found. Check if prefabs are in resources folder.");
        }

       
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


 
}

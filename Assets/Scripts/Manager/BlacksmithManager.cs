using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithManager : MonoBehaviour {

    public static BlacksmithManager Instance;
    
    [SerializeField]
    private string rootFolderName; 
    [SerializeField]
    private List<PhysicalMaterial> materialList;    // Stores all materials based on ingots

    [Header("Stored prefabs/references")]
    [SerializeField]
    private List<GameObject> availableIngots;   // Store ingot prefabs
    [SerializeField]
    private List<GameObject> availableOres;     // Store ore prefabs
    

    public List<Ingot> Ingots
    {
        get
        {
            List<Ingot> list = new List<Ingot>();

            foreach (GameObject g in availableIngots)
                list.Add(g.GetComponent<Ingot>());

            return list;
        }
    }

    public List<Ore> Ores
    {
        get
        {
            List<Ore> list = new List<Ore>();

            foreach (GameObject g in availableOres)
                list.Add(g.GetComponent<Ore>());

            return list;
        }
    }

    public List<PhysicalMaterial> MaterialList
    {
        get { return this.materialList; }
    }



    void Awake()
    {
        Instance = this;
        availableIngots = new List<GameObject>();
        availableOres = new List<GameObject>();
        materialList = new List<PhysicalMaterial>();

        StorePrefabReferences("Ingots",availableIngots);
        StorePrefabReferences("Ores", availableOres);
        CheckPhysicalMaterials(availableIngots);

    }

    // Spawns the requested ingot in the game
    public void SpawnIngot(TYPE type, Transform transform)
    {
        foreach (GameObject i in availableIngots)
        {
            if (type == i.GetComponent<Ingot>().PhysicalMaterial.Type)
                Instantiate(i, transform);
        }
    }

    // Spawns the requested ore in the game
    public void SpawnOre(TYPE type, Transform transform)
    {
        foreach (GameObject i in availableOres)
        {
            if (type == i.GetComponent<Ore>().Type)
                Instantiate(i, transform);
        }
    }

    




    // Populates a list with prefabs from the requested folder
    private void StorePrefabReferences(string subFolder, List<GameObject> prefabList)
    {

       Object[] loadedStuff = Resources.LoadAll(rootFolderName + "/" + subFolder,typeof(GameObject));
        
        if(loadedStuff != null)
        {
            foreach (Object o in loadedStuff)
            {
                prefabList.Add(o as GameObject);
            }
        }
        else
        {
            Debug.Log("Prefabs cant be found. Check if prefabs are in resources folder.");
        }

    }

    // Populates the material list
    private void CheckPhysicalMaterials(List<GameObject> someList)
    {
        if(someList != null)
        {
            foreach (GameObject item in someList)
            {
                Ingot ingotref =  item.GetComponent<Ingot>();
                StorePhysicalMaterial(ingotref);
            }
        }
        else
        {
            Debug.Log("ingot list is empty");
        }
        
    }


    //Adds a UNIQUE material type to the material list
    private void StorePhysicalMaterial(Ingot item)
    {
        bool alreadyExist = false;

        if (materialList.Count < 1)
            materialList.Add(item.PhysicalMaterial);

        foreach(PhysicalMaterial pm in materialList)
        {
            if(item.PhysicalMaterial.Type == pm.Type)
            {
                alreadyExist = true;
                break;
            }
        }

        if (!alreadyExist)
            materialList.Add(item.PhysicalMaterial);
    }

   

   
}

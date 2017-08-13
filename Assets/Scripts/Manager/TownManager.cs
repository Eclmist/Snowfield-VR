using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TownManager : MonoBehaviour
{

    public static TownManager Instance;

    [SerializeField]
    private Town currentTown;

    // Use this for initialization
    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("There should only be one instance of townmanager running");
            Destroy(this);
        }
    }

    //public IEnumerator SpawnCoroutine(

    void Update()
    {
        UpdateTown();
    }

    private void UpdateTown()
    {
        
    }
    public Node GetRandomSpawnPoint()
    {
        int shopIndex = UnityEngine.Random.Range(0, currentTown.SpawnPoint.Count);
        shopIndex = shopIndex == currentTown.SpawnPoint.Count ? shopIndex - 1 : shopIndex;
        if (shopIndex >= 0)
            return currentTown.SpawnPoint[shopIndex];
        else
            return null;
    }

   



    public Shop GetRandomShop(List<Shop> visitedShop)
    {
        List<Shop> possibleShops = new List<Shop>();
        possibleShops.AddRange(currentTown.Shops);
        possibleShops.RemoveAll(Shop => visitedShop.Contains(Shop));

        if (possibleShops.Count != 0)
        {
            int shopIndex = UnityEngine.Random.Range(0, possibleShops.Count);
            return possibleShops[shopIndex];
        }
        else
            return null;
    }

    public int NumberOfShopsUnvisited(List<Shop> visitedShop)
    {
        return currentTown.Shops.Count - visitedShop.Count;
    }
    public Town CurrentTown
    {
        get
        {
            return currentTown;
        }
    }


    public void ChangeTown(Town _town)
    {
        currentTown = _town;
    }
}

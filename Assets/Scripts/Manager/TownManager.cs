using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TownManager : MonoBehaviour
{

    public static TownManager Instance;

    [SerializeField]
    private Town currentTown;
    [SerializeField]
    [Tooltip("The time in seconds between each ai spawns")]
    [Range(0, 100)]
    private float aiSpawnTimer;//can be made to react with gamemanager in the future
    private float timer;
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
        foreach (Shop shop in visitedShop)
        {
            if (possibleShops.Contains(shop))
                possibleShops.Remove(shop);
        }

        possibleShops.RemoveAll(Shop => Shop.Owner == null);
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

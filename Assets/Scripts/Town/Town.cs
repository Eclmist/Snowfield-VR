using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Town : MonoBehaviour
{//Can be used to decide the type of adventurers/structures etc

    [SerializeField][Range(5,30)][Tooltip("Max number of AI in town")]
    private int population = 5;

    [SerializeField]
    private List<Shop> allShops = new List<Shop>();

    private List<AI> listOfAI = new List<AI>();

    [SerializeField]
    private List<AI> typeOfAI = new List<AI>();

    [SerializeField]
    private List<Transform> spawnPoints = new List<Transform>();

    public List<AI> AIs
    {
        get
        {
            return listOfAI;
        }
    }


    private void Awake()
    {
        allShops = new List<Shop>();
        listOfAI = new List<AI>();
    }

    private void Start()
    {
        allShops.AddRange(GetComponentsInChildren<Shop>());
    }

    public int Population//used to decide how many requests/day etc
    {
        get
        {
            return population;
        }
    }

    public Town(int _difficulty)
    {
        population = _difficulty;
    }

    public List<Shop> Shops
    {
        get
        {
            return allShops;
        }
    }

    public Transform getRandomSpawnPoint()
    {
        int shopIndex = Random.Range(0, spawnPoints.Count);
        shopIndex = shopIndex == spawnPoints.Count ? shopIndex - 1 : shopIndex;
        if (shopIndex >= 0)
            return spawnPoints[shopIndex];
        else
            return null;
    }

    public AI getRandomAIType()
    {
        int aiCount = Random.Range(0, typeOfAI.Count);
        aiCount = aiCount == typeOfAI.Count ? aiCount - 1 : aiCount;
        if (aiCount >= 0)
            return typeOfAI[aiCount];
        else
            return null;
    }
}

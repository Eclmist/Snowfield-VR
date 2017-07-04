using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Town : MonoBehaviour
{//Can be used to decide the type of adventurers/structures etc

    [SerializeField]
    [Range(1, 30)]
    [Tooltip("Max number of AI in town")]
    private int population = 1;

    [SerializeField]
    private List<Shop> allShops = new List<Shop>();

    private List<AI> listOfAI = new List<AI>();

    [SerializeField]
    private List<AI> typeOfAI = new List<AI>();

    [SerializeField]
    private List<Node> spawnPoints = new List<Node>();


    public List<AI> AIs
    {
        get
        {
            return listOfAI;
        }
    }

    public List<AI> AITypes
    {
        get
        {
            return typeOfAI;
        }
    }

    public List<Node> SpawnPoint
    {
        get
        {
            return spawnPoints;
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


}

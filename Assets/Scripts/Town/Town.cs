using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Town : MonoBehaviour
{//Can be used to decide the type of adventurers/structures etc

    [SerializeField]
    [Range(1, 30)]
    [Tooltip("Number of AI in town that are generated at start")]
    private int basePopulation = 1;

    [SerializeField]
    private List<Shop> allShops = new List<Shop>();

    [SerializeField]
    private List<Node> spawnPoints = new List<Node>();

    [SerializeField]
    private Node monsterPoint;

    [SerializeField]
    private Transform treasure;

    public Transform Treasure
    {
        get
        {
            return treasure;
        }
    }

    public Node MonsterPoint
    {
        get
        {
            return monsterPoint;
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
    }

    private void Start()
    {
        allShops.AddRange(GetComponentsInChildren<Shop>());
    }

    public int Population//used to decide how many requests/day etc
    {
        get
        {
            return basePopulation;
        }
    }

    public Town(int _difficulty)
    {
        basePopulation = _difficulty;
    }

    public List<Shop> Shops
    {
        get
        {
            return allShops;
        }
    }

    

}

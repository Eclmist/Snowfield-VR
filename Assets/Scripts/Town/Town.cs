using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Town : MonoBehaviour
{//Can be used to decide the type of adventurers/structures etc

    [SerializeField]
    [Range(0, 30)]
    [Tooltip("Max number of AI in town")]
    private int population = 1;

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

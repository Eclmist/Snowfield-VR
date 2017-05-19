using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownManager : MonoBehaviour
{

    public static TownManager Instance;

    [SerializeField]
    private Town currentTown;
    [SerializeField][Tooltip("The time in seconds between each ai spawns")][Range(1,100)]
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

    void Start()
    {
        //currentTown = (Town)SerializeManager.Load("TownData");
        
    }

    void Update()
    {
        UpdateTown();
    }

    private void UpdateTown()
    {
        if (CurrentTown != null && currentTown.AIs.Count != CurrentTown.Population)
        {
            timer += Time.deltaTime;
            if (timer > aiSpawnTimer)
            {
                timer = 0;
                
                Transform randomSpawn = CurrentTown.getRandomSpawnPoint();
                AI randomAI = CurrentTown.getRandomAIType();
                if(randomSpawn && randomAI)
                currentTown.AIs.Add(Instantiate(CurrentTown.getRandomAIType(), randomSpawn.position, randomSpawn.rotation).GetComponent<AI>());
            }
        }
    }
    public Shop GetRandomShop()
    {
        int shopIndex = Random.Range(0, currentTown.Shops.Count);
        shopIndex = shopIndex == currentTown.Shops.Count ? shopIndex - 1 : shopIndex;
        if (shopIndex >= 0)
            return currentTown.Shops[shopIndex];
        else
            return null;
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

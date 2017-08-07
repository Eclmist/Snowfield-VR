using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIManager : MonoBehaviour
{

    private List<AdventurerAIData> listOfAIData;

    [SerializeField]
    private List<AI> typeOfAI = new List<AI>();
    [SerializeField]
    private List<MerchantAI> merchants = new List<MerchantAI>();
    public static AIManager Instance;

    [SerializeField]
    [Range(0, 50)]
    protected int timeBetweenAISpawn;


    protected void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("There should only be one instance of AImanager running");
            Destroy(this);
        }
    }

    protected void Start()
    {
        HandleAdventurerAI();
    }

    protected void HandleAdventurerAI()
    {
        listOfAIData = (List<AdventurerAIData>)SerializeManager.Load("AIData");
        if (listOfAIData == null)
        {
            listOfAIData = new List<AdventurerAIData>();
        }

        if (listOfAIData.Count < TownManager.Instance.CurrentTown.Population)
        {
            for (int i = listOfAIData.Count; i < TownManager.Instance.CurrentTown.Population; i++)
            {
                listOfAIData.Add(CreateNewAdventurerAI());
            }
        }
        else if (listOfAIData.Count > TownManager.Instance.CurrentTown.Population)
        {
            while (listOfAIData.Count > TownManager.Instance.CurrentTown.Population)
            {
                listOfAIData.RemoveAt(listOfAIData.Count - 1);
            }
        }

        int timePeriod = 0;
        foreach (AdventurerAIData data in listOfAIData)
        {
            timePeriod += timeBetweenAISpawn;
            AI newActor = ((GameObject)Resources.Load(data.Path)).GetComponent<AI>();
            AI ai = Instantiate(newActor).GetComponent<AI>();
            ai.Data = data;
            ai.gameObject.SetActive(false);
            StartCoroutine(SpawnCoroutine(ai, timePeriod, TownManager.Instance.GetRandomSpawnPoint()));
        }
    }

    protected AdventurerAIData CreateNewAdventurerAI()
    {
        AI newAI = GetRandomAIType();
        string myPath = "AIs\\" + newAI.name;
        AdventurerAIData newData = new AdventurerAIData(newAI.Data, newAI.name, myPath);//Random name gen
        return newData;
    }

    public void Spawn(AI currentAI, float timeToSpawn, Node spawnNode, UnityAction invokingEvent = null, float intervalBetweenInvoke = 0)
    {
        StartCoroutine(SpawnCoroutine(currentAI, timeToSpawn, spawnNode, invokingEvent, intervalBetweenInvoke));
    }

    protected IEnumerator SpawnCoroutine(AI currentAI, float timeToSpawn, Node spawnNode, UnityAction invokingEvent = null, float intervalBetweenInvoke = 0)
    {

        if (invokingEvent == null)
        {
            yield return new WaitForSecondsRealtime(timeToSpawn);
        }
        else
        {
            while (timeToSpawn > 0)
            {
                yield return new WaitForSecondsRealtime(intervalBetweenInvoke);
                invokingEvent();
                timeToSpawn -= intervalBetweenInvoke;
            }
        }
        currentAI.transform.position = spawnNode.Position;
        currentAI.Spawn();
    }


    private void OnDisable()
    {
        //SerializeManager.Save("AIData", listOfAIData);
    }

    public void SpawnMerchant()
    {
        if (merchants.Count > 0)
        {
            int index = Random.Range(0, merchants.Count);
            MerchantAI AI = Instantiate(merchants[index]).GetComponent<MerchantAI>();
            AI.gameObject.SetActive(false);
            Spawn(AI, 0, TownManager.Instance.GetRandomSpawnPoint());
        }
        else
            Debug.Log("No merchants");
    }

    public AI GetRandomAIType()
    {
        int aiCount = Random.Range(0, typeOfAI.Count);

        aiCount = aiCount == typeOfAI.Count ? aiCount - 1 : aiCount;

        if (aiCount >= 0)
            return typeOfAI[aiCount];
        else
            return null;
    }
}

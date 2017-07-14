using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
public class AIManager : MonoBehaviour
{

    private List<AdventurerAIData> listOfAIData;

    private List<AI> allAIsInScene = new List<AI>();
    [SerializeField]
    private List<AI> typeOfAI = new List<AI>();

    public static AIManager Instance;



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

        listOfAIData = (List<AdventurerAIData>)SerializeManager.Load("AIData");
        Debug.Log(listOfAIData);
        if (listOfAIData == null)
        {
            listOfAIData = new List<AdventurerAIData>();
        }

        if (listOfAIData.Count < TownManager.Instance.CurrentTown.Population)
        {
            for (int i = listOfAIData.Count; i < TownManager.Instance.CurrentTown.Population; i++)
            {
                listOfAIData.Add(CreateNewAdventurerAI());
                Debug.Log("created");
            }
        }
        else if (listOfAIData.Count > TownManager.Instance.CurrentTown.Population)
        {
            while (listOfAIData.Count > TownManager.Instance.CurrentTown.Population)
            {
                listOfAIData.RemoveAt(listOfAIData.Count - 1);
            }
        }

        foreach (ActorData data in listOfAIData)
        {
            AI newActor = ((GameObject)Resources.Load(data.Path)).GetComponent<AI>();
            AI ai = Instantiate(newActor).GetComponent<AI>();
            ai.Data = data;
            allAIsInScene.Add(ai);
            ai.gameObject.SetActive(false);
            StartCoroutine(SpawnCoroutine(ai, Random.Range(1, 20), TownManager.Instance.GetRandomSpawnPoint()));
        }
    }

    protected AdventurerAIData CreateNewAdventurerAI()
    {
        AI newAI = GetRandomAIType();
        string myPath = "AIs\\" + newAI.name;
        AdventurerAIData newData = new AdventurerAIData(myPath, "NewAI");
        return newData;
    }

    public void Spawn(AI currentAI, float timeToSpawn, Node spawnNode, UnityAction invokingEvent = null, float intervalBetweenInvoke = 0)
    {
        StartCoroutine(SpawnCoroutine(currentAI, timeToSpawn, spawnNode, invokingEvent,intervalBetweenInvoke));
    }

    protected IEnumerator SpawnCoroutine(AI currentAI, float timeToSpawn, Node spawnNode, UnityAction invokingEvent = null, float intervalBetweenInvoke = 0)
    {
        
        if (invokingEvent == null)
        {
            Debug.Log("hit1");
            yield return new WaitForSecondsRealtime(timeToSpawn);
        }
        else
        {
            while(timeToSpawn > 0)
            {
                yield return new WaitForSecondsRealtime(intervalBetweenInvoke);
                invokingEvent();
                timeToSpawn -= intervalBetweenInvoke;
            }
        }
        Debug.Log("hit2");
        currentAI.transform.position = spawnNode.Position;
        currentAI.gameObject.SetActive(true);
    }


    private void OnDisable()
    {
        SerializeManager.Save("AIData", listOfAIData);
    }

    public void SetAllAIState(ActorFSM.FSMState state)
    {
        foreach (AI ai in allAIsInScene)
        {
            ai.ChangeState(state);
        }
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

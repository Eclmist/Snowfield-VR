using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

        if(listOfAIData.Count < TownManager.Instance.CurrentTown.Population)
        {
            for (int i = listOfAIData.Count; i < TownManager.Instance.CurrentTown.Population; i++)
            {
                listOfAIData.Add(CreateNewAdventurerAI());
            }
        }
        else if(listOfAIData.Count > TownManager.Instance.CurrentTown.Population)
        {
            while(listOfAIData.Count > TownManager.Instance.CurrentTown.Population)
            {
                listOfAIData.RemoveAt(listOfAIData.Count - 1);
            }
        }

        foreach(ActorData data in listOfAIData)
        {
            AI newActor =((GameObject)Resources.Load(data.Path)).GetComponent<AI>();
            
            StartCoroutine(SpawnCoroutine(newActor, Random.Range(1, 20), TownManager.Instance.GetRandomSpawnPoint(), data, true));
        }
    }

    public AdventurerAIData CreateNewAdventurerAI()
    {
        AI newAI = GetRandomAIType();
        string myPath = "AIs\\" + newAI.name;
        AdventurerAIData newData = new AdventurerAIData(myPath, "NewAI");
        return newData;
    }

    public IEnumerator SpawnCoroutine(AI currentAI, float timeToSpawn, Node spawnNode,ActorData data = null, bool instantiateNew = false)
    {
        yield return new WaitForSecondsRealtime(timeToSpawn);
        if (instantiateNew)
        {
            AI ai = Instantiate(currentAI, spawnNode.Position, Quaternion.identity).GetComponent<AI>();
            ai.Data = data;
            allAIsInScene.Add(ai);
        }

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

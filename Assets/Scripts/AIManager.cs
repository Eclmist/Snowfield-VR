using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIManager : MonoBehaviour
{

    private List<AdventurerAIData> listOfAIData = new List<AdventurerAIData>(), instantiatedAIData = new List<AdventurerAIData>();

    [SerializeField]
    private List<AI> typeOfAI = new List<AI>();
    [SerializeField]
    private List<MerchantAI> merchants = new List<MerchantAI>();
    private TextAsset genericNameParts;
    private string[] nameParts;
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

        LoadNameParts();
    }

    protected void Start()
    {
        GenerateBaseAIs();
        StartSpawningAllAdventurerAIs();
    }

    public void GenerateBaseAIs()
    {
        List<AdventurerAIData> aiData = (List<AdventurerAIData>)SerializeManager.Load("AIData");
        if (aiData == null)
        {
            for (int i = 0; i < TownManager.Instance.CurrentTown.Population; i++)
            {
                CreateNewAdventurerAI(randomColorGenerator(), randomColorGenerator(),GetRandomUniqueName(), Random.Range(1.1f,1.3f));
            }
        }
        else
            listOfAIData = aiData;
    }

    public void StartSpawningAllAdventurerAIs()
    {
        int timePeriod = 0;
        foreach (AdventurerAIData data in listOfAIData)
        {
            timePeriod += timeBetweenAISpawn;
            InstantiateNewAdventurerAI(data, timePeriod);
        }
    }

    public AdventurerAI InstantiateNewAdventurerAI(AdventurerAIData data, float time = 0)
    {
        AdventurerAI adventurerAI = null;
        if (listOfAIData.Contains(data) && !instantiatedAIData.Contains(data))
        {
            adventurerAI = Instantiate(((GameObject)Resources.Load(data.Path))).GetComponent<AdventurerAI>();
            instantiatedAIData.Add(data);
            adventurerAI.Data = data;
            CharacterRigDefiner def = adventurerAI.GetComponent<CharacterRigDefiner>();
            Debug.Log(def.HairMat.Count);
            foreach (Material m in def.HairMat)
            {
                m.SetColor("_Color", data.CustomizeInfo.HairColor);
            }

            foreach (Material m in def.EyeMaterial)
            {
                m.SetColor("_Color", data.CustomizeInfo.EyeColor);
            }

            adventurerAI.transform.localScale = Vector3.one * data.CustomizeInfo.Scale;

            if (time > 0)
                adventurerAI.gameObject.SetActive(false);

            StartCoroutine(SpawnCoroutine(adventurerAI, time, TownManager.Instance.GetRandomSpawnPoint()));
        }
        Debug.Assert(adventurerAI != null, "AI already exist or data has not been created");


        return adventurerAI;
    }

    public AdventurerAIData CreateNewAdventurerAI(Color hairColor, Color eyeColor, string name = null, float scale = 1.3f)
    {
        AI newAI = GetRandomAIType();
        string myPath = "AIs\\" + newAI.name;

        AdventurerAIData.CharacterInformation ci = new AdventurerAIData.CharacterInformation(scale, hairColor, eyeColor);

        AdventurerAIData newData = new AdventurerAIData(ci, newAI.Data, name, myPath);//Random name gen
        listOfAIData.Add(newData);
        Debug.Log("created");
        return newData;
    }


    protected Color randomColorGenerator()
    {
        float r, g, b;
        r = Random.Range(0, 1f);
        g = Random.Range(0, 1f);
        b = Random.Range(0, 1f);
        return new Color(r, g, b);
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


    public void InstantiateMerchant()
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

    private void LoadNameParts()
    {
        genericNameParts = Resources.Load("GenericNameParts") as TextAsset;

        if (genericNameParts == null)
            Debug.LogError("Cannot find textfile containing names");
        else
        {
            nameParts = genericNameParts.text.Split('\n');
        }
    }

    private string GetRandomUniqueName()
    {
        Debug.Log(nameParts.Length);
        string tempName = "UnityChan";

        if (nameParts.Length > 0)
            tempName = nameParts[Random.Range(0, nameParts.Length)] + nameParts[Random.Range(0, nameParts.Length)]
                + nameParts[Random.Range(0, nameParts.Length)]
                + ((int)(Random.Range(1, 99)));

        return tempName;

    }

    public void Respawn(AI ai)
    {
        ai.Data.GetJob(JobType.COMBAT).ReduceExperiencePercentage(.1f);
        Spawn(ai, ai.Data.GetJob(JobType.COMBAT).Level * GameConstants.Instance.RespawnTimer, TownManager.Instance.GetRandomSpawnPoint());
    }
}

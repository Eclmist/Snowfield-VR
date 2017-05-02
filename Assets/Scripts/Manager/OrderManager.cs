using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class OrderManager : MonoBehaviour
{

    public static OrderManager Instance;

    public int baseGoldMultiplier;
    public int baseDurationMultiplier;
    public bool save;

    [SerializeField]
    private List<OrderTemplate> templateList;
    [SerializeField]
    private List<PhysicalMaterial> materialList;


    private List<OrderTemplate> availableTemplatesForCurrentLevel;


    public List<OrderTemplate> TemplateList
    {
        get { return this.templateList; }
        set { this.templateList = value; }
    }

    public List<PhysicalMaterial> MaterialList
    {
        get { return this.materialList; }
        set { this.materialList = value; }
    }

    // Use this for initialization
    void Awake()
    {

        //LoadTemplates();

        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("There should only be one instance of request board running");
            Destroy(this);
        }

        availableTemplatesForCurrentLevel = new List<OrderTemplate>();
        //PopulateLists();
        

        if (templateList.Count < 1)
            Debug.Log("Template list is empty");

    }

    void Start()
    {

        UpdateAvailableTemplates();
        Debug.Log(availableTemplatesForCurrentLevel.Count);
    }

 
    void Update()
    {
        if(save)
        {
            SerializeManager.Save("templateList", templateList);
            SerializeManager.Save("materialList", materialList);
          
            
        }

      
    }

    public void UpdateAvailableTemplates()
    {
       foreach(Job job in Player.Instance.JobListReference)
        {
            foreach(OrderTemplate ot in templateList)
            {
                if (ot.JobType == job.Type && ot.LevelUnlocked <= job.Level)
                    availableTemplatesForCurrentLevel.Add(ot);
            }
        }
            
              
    }




    private void PopulateLists()
    {
        SerializeManager.Load("templateList");
        UpdateAvailableTemplates();
        SerializeManager.Load("materialList");

    }



    private void LoadTemplates()
    {
        foreach(OrderTemplate ot in templateList)
        {
            ot.Sprite = Resources.Load("Templates/" + ot.SpriteIndex.ToString()) as Sprite;
        }

    }
    

   

    public void NewRequest()
    {
        int totalExperienceValue = 0;
        foreach (Job job in Player.Instance.JobListReference)
        {
            totalExperienceValue += job.Experience;
        }

        int randomValue = Random.Range(0, totalExperienceValue);
        totalExperienceValue = 0;
        foreach (Job job in Player.Instance.JobListReference)
        {
            totalExperienceValue += job.Experience;
            if (totalExperienceValue >= randomValue)
            {
                Order o = GenerateOrder(job);
                if (o != null)
                    OrderBoard.Instance.SpawnOnBoard(o);
            }
        }

    }


    private Order GenerateOrder(Job job)
    {
        // TO DO
        Player currentPlayer = Player.Instance;
        Order newOrder = null;
        List<OrderTemplate> currentTemplateList = GetTemplatesForJobType(job.Type);
        // Get a random template
        OrderTemplate currentTemplate = currentTemplateList[Random.Range(0, currentTemplateList.Count)];

        // Generate order based on job type
        switch(job.Type)
        {
            case JobType.ALCHEMY:
                break;
            case JobType.BLACKSMITH:

                PhysicalMaterial currentMaterial = BlacksmithManager.Instance.MaterialList[Random.Range(0, BlacksmithManager.Instance.MaterialList.Count)];

                newOrder = new Order(currentMaterial.Name + " " + currentTemplate.ProductSuffix.ToString()
            , currentTemplate.Sprite
            , job.Level * baseDurationMultiplier * currentTemplate.Duration
            , job.Level * currentTemplate.BaseGold * currentMaterial.CostMultiplier *baseGoldMultiplier);

                break;
        }


        return newOrder;

    }

    public void CompletedOrder(bool success,int reward)
    {
        if (success)
        {
            GameManager.Instance.AddPlayerGold(reward);
        }
    }

    // Filter templateList by JobTpye
    private List<OrderTemplate> GetTemplatesForJobType(JobType jobType)
    {
        List<OrderTemplate> requestedList = new List<OrderTemplate>();

        foreach(OrderTemplate ot in availableTemplatesForCurrentLevel)
        {
            if (ot.JobType == jobType)
                requestedList.Add(ot);
        }

        return requestedList;


    }



}

using System.Collections.Generic;

using UnityEngine;

public class OrderManager : MonoBehaviour

{
    public static OrderManager Instance;

    public int baseGoldMultiplier;

    public int baseDurationMultiplier;

    [SerializeField]
    private List<OrderTemplate> templateList;

    private List<OrderTemplate> availableTemplatesForCurrentLevel;

    public List<OrderTemplate> TemplateList

    {
        get { return this.templateList; }

        set { this.templateList = value; }
    }

    // Use this for initialization

    private void Awake()

    {
        //LoadTemplates();

        if (!Instance)

        {
            Instance = this;

            availableTemplatesForCurrentLevel = new List<OrderTemplate>();

            //PopulateLists();

            if (templateList.Count < 1)

                Debug.Log("Template list is empty");
        }
    }

    private void Start()

    {
        //Debug.Log(availableTemplatesForCurrentLevel.Count);
    }

    public void NewRequest()

    {
        int totalExperienceValue = 0;

        foreach (Job job in Player.Instance.JobList)

        {
            totalExperienceValue += job.Experience;
        }

        int randomValue = Random.Range(0, totalExperienceValue);

        totalExperienceValue = 0;

        foreach (Job job in Player.Instance.JobList)

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

        OrderTemplate currentTemplate = templateList[Random.Range(0, templateList.Count)];

        // Generate order based on job type

        switch (job.Type)

        {
            case JobType.ALCHEMY:

                break;

            case JobType.BLACKSMITH:

                PhysicalMaterial currentMaterial = BlacksmithManager.Instance.MaterialList[Random.Range(0, BlacksmithManager.Instance.MaterialList.Count)];

                newOrder = new Order(ItemManager.Instance.GetItemData(currentTemplate.ReferenceItemID).ObjectReference.name

            , ItemManager.Instance.GetItemData(currentTemplate.ReferenceItemID).Icon

            , job.Level * baseDurationMultiplier * currentTemplate.Duration

            , job.Level * currentTemplate.BaseGold * currentMaterial.CostMultiplier * baseGoldMultiplier);

                break;
        }

        return newOrder;
    }

    public void CompletedOrder(bool success, int reward)

    {
        if (success)

        {
            GameManager.Instance.AddPlayerGold(reward);
        }
    }

    // Filter templateList by JobTpye

    private List<OrderTemplate> GetTemplatesForJobType(JobType jobType)


    public void StartRequest(AdventurerAI ai, Order order)

    {

        Debug.Log(order);

        if (order != null)

            OrderBoard.Instance.SpawnOnBoard(order, ai);

    }

        foreach (OrderTemplate ot in availableTemplatesForCurrentLevel)


    public Order GenerateOrder()
    {


        // TO DO





        Player currentPlayer = Player.Instance;





        Order newOrder = null;




        // Get a random template





        OrderTemplate currentTemplate = templateList[Random.Range(0, templateList.Count)];







        // Generate order based on job type

        int totalExperienceValue = 0;


        foreach (Job job in Player.Instance.JobList)


        {


            totalExperienceValue += job.Experience;


        }


        int randomValue = Random.Range(0, totalExperienceValue);


        totalExperienceValue = 0;





        foreach (Job job in Player.Instance.JobList)

        {


            totalExperienceValue += job.Experience;


            if (totalExperienceValue >= randomValue)

            {





                switch (job.Type)







                {



                    case JobType.ALCHEMY:







                        break;







                    case JobType.BLACKSMITH:







                        PhysicalMaterial currentMaterial = BlacksmithManager.Instance.MaterialList[Random.Range(0, BlacksmithManager.Instance.MaterialList.Count)];







                        newOrder = new Order(ItemManager.Instance.GetItemData(currentTemplate.ReferenceItemID).ObjectReference.name







                    , ItemManager.Instance.GetItemData(currentTemplate.ReferenceItemID).Icon







                    , job.Level * baseDurationMultiplier * currentTemplate.Duration







                    , job.Level * currentTemplate.BaseGold * currentMaterial.CostMultiplier * baseGoldMultiplier,





                    currentTemplate.ReferenceItemID);







                        break;



                }







                return newOrder;




            }


        }

        return null;



    }





    public void CompletedOrder(bool success, int reward)





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





        foreach (OrderTemplate ot in availableTemplatesForCurrentLevel)





        {


            if (ot.JobType == jobType)





                requestedList.Add(ot);


        }





        return requestedList;


    }


}
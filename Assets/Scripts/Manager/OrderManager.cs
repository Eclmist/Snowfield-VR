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

    }


    

    public void CompletedOrder(bool success, int reward)

    {
        if (success)

        {
            GameManager.Instance.AddPlayerGold(reward);
        }
    }

    // Filter templateList by JobTpye




    public void StartRequest(AdventurerAI ai, Order order)

    {

        if (order != null)

            OrderBoard.Instance.SpawnOnBoard(order, ai);
    }

       

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
	
						ItemData refData = ItemManager.Instance.GetItemData(currentTemplate.ReferenceItemID);
						PhysicalMaterial currentMaterial = null;
						CraftedItem tempCraftedItem = refData.ObjectReference.GetComponent<CraftedItem>();

						if (tempCraftedItem)
							currentMaterial = BlacksmithManager.Instance.GetPhysicalMaterialInfo(tempCraftedItem.GetPhysicalMaterial());

						newOrder = new Order(ItemManager.Instance.GetItemData(currentTemplate.ReferenceItemID).ObjectReference.name

					, ItemManager.Instance.GetItemData(currentTemplate.ReferenceItemID).Icon

					, job.Level * baseDurationMultiplier * currentTemplate.Duration

					, job.Level * currentTemplate.BaseGold * currentMaterial.CostMultiplier * baseGoldMultiplier,

					currentTemplate.ReferenceItemID,
					currentMaterial.type);

						break;

					case JobType.COMBAT:

						break;
					

				}


				
            }
        }

		return newOrder;
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
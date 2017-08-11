using System.Collections.Generic;

using UnityEngine;

public class OrderManager : MonoBehaviour

{
    public static OrderManager Instance;  
    [SerializeField]
    private int baseDuration;
    [SerializeField][Range(1,10)]
    private int levelToDurationMultiplier;


    // Use this for initialization

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

    }

    private void Update()
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

                        Ingot tempIngot = ItemManager.Instance.GetRandomUnlockedIngot();
                        Debug.Log(tempIngot.name);
                        ItemData refData = WeaponTierManager.Instance.GetRandomWeaponInTypeClass(tempIngot.PhysicalMaterial.type);
                        Debug.Log("Current order is: " + refData.ObjectReference.name);
                        PhysicalMaterial currentMaterial = null;
                        CraftedItem tempCraftedItem = refData.ObjectReference.GetComponent<CraftedItem>();

                        if (tempCraftedItem)
                            currentMaterial = BlacksmithManager.Instance.GetPhysicalMaterialInfo(tempCraftedItem.GetPhysicalMaterial());

                        newOrder = new Order(refData.ObjectReference.name

                    , refData.Icon

                    , ((job.Level * levelToDurationMultiplier) + baseDuration)

                    , job.Level  * currentMaterial.CostMultiplier ,

                    refData.ItemID,
                    currentMaterial.type);

                        break;

					case JobType.COMBAT:

						break;
					

				}

            }
        }

		return newOrder;
	}

   
}
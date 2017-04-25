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
       
        LoadTemplates();
        Instance = this;

        //if (!Instance)
        //{
        //    Instance = this;
        //}
        //else
        //{
        //    Debug.Log("There should only be one instance of request board running");
        //    Destroy(this);
        //}

        SerializeManager.Load("templateList");
        SerializeManager.Load("materialList");

        if (templateList.Count < 1)
            Debug.Log("Template list is empty");

    }
 
    void Update()
    {
        if(save)
        {
            SerializeManager.Save("templateList", templateList);
            SerializeManager.Save("materialList", materialList);
          
            
        }
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
                GenerateOrder();
            }
        }

    }


    private void GenerateOrder()
    {
        // TO DO
        Player currentPlayer = Player.Instance;
        Job randJob = currentPlayer.JobListReference[Random.Range(0,currentPlayer.JobListReference.Count - 1)];

        Order newOrder = new Order(materialList[Random.Range(0, materialList.Count - 1)].Name + " sword"
            ,templateList[Random.Range(0,templateList.Count -1 )].Sprite
            ,randJob.Level * baseDurationMultiplier
            , randJob.Level * baseGoldMultiplier);

        OrderBoard.Instance.SpawnOnBoard(newOrder); 
    }

    public void CompletedOrder(bool success,int reward)
    {
        if (success)
        {
            GameManager.Instance.AddPlayerGold(reward);
        }
    }



}

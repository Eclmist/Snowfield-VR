using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    [System.Serializable]
    public struct OrderTemplate
    {
        public Sprite sprite;
        public Mesh mesh;
        public int baseGold;

    }

    [System.Serializable]
    public struct PhysicalMaterial
    {
        public string name;
        public int costMultiplier;
    }

    public static OrderManager Instance;

    [SerializeField]
    private List<OrderTemplate> templateList;
    [SerializeField]
    private List<PhysicalMaterial> materialList;

    // Use this for initialization
    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("There should only be one instance of request board running");
            Destroy(this);
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
                //DataManager.OpenRequest
                //Request newRequest = new Request(null, null, 0, 0);//To be auto generated
                //OrderBoard.Instance.SpawnOnBoard(newRequest);
                //DataManager.CloseRequest
                GenerateOrder();


            }
        }

    }


    private void GenerateOrder()
    {



    }

    //private OrderTemplate GetRandomTemplate()
    //{
    //    int total = templateList.Count;
        
    //}


}

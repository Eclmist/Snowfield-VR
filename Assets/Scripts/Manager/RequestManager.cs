using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestManager : MonoBehaviour
{

    public static RequestManager Instance;


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
            }
        }
    }



}

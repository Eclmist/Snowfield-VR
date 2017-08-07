using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SUFFIX
{
    sword,
    potion
}

[System.Serializable]
public class OrderTemplate {


    [SerializeField]
    private int baseGold;
    [SerializeField]
    private int duration;
    [SerializeField]
    private JobType jobType;
    [SerializeField]
    private int referenceItemID;


    public int BaseGold
    {
        get { return this.baseGold; }
        set { this.baseGold = value; }
    }

    public int Duration
    {
        get { return this.duration; }
        set { this.duration = value; }
    }



    public JobType JobType
    {
        get { return this.jobType; }
        set { this.jobType = value; }
    }

    

    public int ReferenceItemID
    {
        get { return this.referenceItemID; }
    }






}

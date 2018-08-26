using UnityEngine;

[System.Serializable]
public abstract class Quest
{
    [SerializeField]
    private string name;



    [SerializeField]
    private JobType jobType;



    [SerializeField]
    private GameObject reward;

    [SerializeField]
    private int expectedCrates;

    [SerializeField]
    private int experience;



    public Quest(JobType jobType)

    {

        name = "New Quest";

        this.jobType = jobType;

    }



    public Quest(string name, JobType jobType, GameObject reward, int expectedCrates, int experience)

    {

        this.name = name;

        this.jobType = jobType;

        this.reward = reward;

        this.expectedCrates = expectedCrates;

        this.experience = experience;

    }



    public string Name

    {

        get { return this.name; }

        set { this.name = value; }

    }



    public JobType JobType

    {

        get { return this.jobType; }

        set { this.jobType = value; }

    }



    public GameObject Reward

    {

        get { return this.reward; }

        set { this.reward = value; }

    }

    public int ExpectedCrates
    {
        get { return this.expectedCrates; }
        set { this.expectedCrates = value; }
    }


    public int Experience

    {

        get { return this.experience; }

        set { this.experience = value; }

    }




}
using UnityEngine;

[System.Serializable]
public abstract class Quest : ICanTalk
{
    [SerializeField]
    private string name;

    [SerializeField]
    private JobType jobType;

    [SerializeField]
    private GameObject reward;

    [SerializeField]
    private Session dialog;

    [SerializeField]
    private Session endDialog;

    [SerializeField]
    private int experience;

    private bool isCompleted;

    public Quest(JobType jobType)
    {
        name = "New Quest";
        this.jobType = jobType;
    }

    public Quest(string name, JobType jobType, GameObject reward, Session dialog, int experience)
    {
        this.name = name;
        this.jobType = jobType;
        this.reward = reward;
        this.dialog = dialog;
        this.experience = experience;
        this.isCompleted = false;
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

    public Session Dialog
    {
        get { return this.dialog; }
        set { this.dialog = value; }
    }

    public Session EndDialog
    {
        get { return this.endDialog; }
        set { this.endDialog = value; }
    }

    public int Experience
    {
        get { return this.experience; }
        set { this.experience = value; }
    }

    public bool IsCompleted
    {
        get { return this.isCompleted; }
        set { this.isCompleted = value; }
    }

    public Session Session
    {
        get
        {
            return this.dialog;
        }

        set
        {
            this.dialog = value;
        }
    }

}
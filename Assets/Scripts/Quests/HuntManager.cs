using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntManager : MonoBehaviour {

    public static HuntManager Instance;

    [SerializeField]
    private string subFolder;



    [SerializeField]private string defaultDailyQuestTitle = "Daily Quest";
    [SerializeField]private List<StoryLine> storylines = new List<StoryLine>();
    [SerializeField]private Queue<AdventurerAI> aiQueue;
    private Queue<Hunt> questDialogQueue;
    private bool isProcessing;

    public List<StoryLine> Storylines
    {
       get { return this.storylines; }
        set { this.storylines = value; }
    }
    
    public string SubFolder
    {
        get { return this.subFolder; }
    }


	void Awake()
    {
        Instance = this;

    }

    void Start()
    {
        aiQueue = new Queue<AdventurerAI>();
        questDialogQueue = new Queue<Hunt>();
    }

    void Update()
    {
        if(aiQueue.Count >= 1)
        {
            ProcessQuests(aiQueue.Peek());
        }

        ProcessDialogQueue();

    }



    public void Clear()
    {
        storylines[0].Clear();
    }

    public StoryLine GetStoryLine(JobType jt)
    {
        StoryLine temp = null;

        foreach(StoryLine sl in storylines)
        {
            if(jt == sl.JobType)
            {
                temp = sl;
                break;
            }
        }

        return temp;
    }


    public bool GotLobang(List<StoryHunt> list)
    {

        bool temp = false;

        foreach(StoryHunt hunt in list)
        {
            StoryLine line = GetStoryLine(hunt.StoryLine);

            if (hunt.IsCompleted && hunt.ProgressionIndex + 1 < line.Count
             && line[hunt.ProgressionIndex + 1].RequiredLevel <= Player.Instance.GetJob(hunt.JobType).Level)
            {
                temp = true;
                break;
            }
       
        }

        return temp;
    }

    public void UpdateQuests(AdventurerAI ai)
    {
        aiQueue.Enqueue(ai);
        ai.isConversing = true;
    }


    public void CompleteQuest(Hunt hunt)
    {
        hunt.IsCompleted = true;
    }


    private void GetNextQuest(StoryHunt hunt)
    {
        StoryLine line = GetStoryLine(hunt.StoryLine);

        if (hunt.IsCompleted && hunt.ProgressionIndex + 1 < line.Count
             && line[hunt.ProgressionIndex + 1].RequiredLevel <= Player.Instance.GetJob(hunt.JobType).Level)
        {
            hunt = line[hunt.ProgressionIndex + 1];
            StartQuestDialog(hunt);
        }

           
    }

    private void ProcessDialogQueue()
    {

        if(!DialogManager.Instance.IsOccupied)
        {
            DialogManager.Instance.SetCurrentSession(questDialogQueue.Peek().Dialog);
            DialogManager.Instance.DisplayDialogBox();
            questDialogQueue.Dequeue();
        }

        
    }

    private void ProcessQuests(AdventurerAI ai)
    {

        if(ai.Quests.Count < NumberOfUnlockedStoryLines())
            GiveStartingQuests(ai.Quests);

        foreach(StoryHunt hunt in ai.Quests)
        {
            if(hunt.IsCompleted)
            {
                GetNextQuest(hunt);
            }

        }


        ai.IsConversing = false;
        aiQueue.Dequeue();

    }

    // Gives the starting quest of each storyline if it does not exist
    private void GiveStartingQuests(List<StoryHunt> list)
    {

        if (list.Count < 1)
        {
            foreach(StoryLine sl in storylines)
            {
                list.Add(sl[0]);
            }
        }
        else
        {
            foreach (StoryLine sl in storylines)
            {
                bool exist = false;

                foreach (StoryHunt hunt in list)
                {
                    if (sl[0].StoryLine == hunt.StoryLine)
                    {
                        exist = true;
                    }

                    if (!exist)
                        list.Add(sl[0]);
                }
            }
        }
        
    }

    

    // Check how many unlocked storylines
    private int NumberOfUnlockedStoryLines()
    {
        int i = 0;

        foreach(StoryLine sl in storylines)
        {
            if (sl.IsUnlocked)
                i++;
        }

        return i;
    }
  

    // Set the dialog for the quest so that dialog manager knows what to display
    // Push quest dialog into a queue
    private void StartQuestDialog(Hunt hunt)
    {
        questDialogQueue.Enqueue(hunt);
    }



        
    









}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntManager : MonoBehaviour {

    public static HuntManager Instance;

    [SerializeField]
    private string subFolder;



    [SerializeField]private string defaultDailyQuestTitle = "Daily Quest";
    [SerializeField]private List<StoryLine> storylines = new List<StoryLine>();
    [SerializeField]private List<DailyHunt> dailyHuntList;

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

    }

    void Update()
    {
        
    }

    public void Clear()
    {
        storylines[0].Clear();
    }

    //public StoryHunt RequestForStoryHunt(AdventurerAI actor,JobType type)
    //{

    //    if(actor.CurrentStoryQuest != null)
    //    {
    //        return storyHuntList[actor.currentStoryQuest.ProgressionIndex + 1];
    //    }
    //    else
    //    {
    //        return storyHuntList[0];
    //    }
    //}

    public bool GotLobang(List<StoryHunt> list)
    {
        bool temp = false;

        foreach(StoryHunt hunt in list)
        {
            if(hunt == null || hunt.IsCompleted)
            {
                temp = true;
                break;
            }
       
        }

        return temp;
    }

    public StoryHunt GetNextQuest(StoryHunt hunt, StoryLine line)
    {
        if (hunt == null)
            return line[0];
          else if (hunt.ProgressionIndex + 1 <line.Count
               && line[hunt.ProgressionIndex + 1].RequiredLevel <= Player.Instance.GetJob(hunt.JobType).Level)
               return line[hunt.ProgressionIndex + 1];


        return null;
    }

    public void UpdateQuests(Actor actor)
    {
        
    }

    public DailyHunt GetDailyHunt()
    {
        return dailyHuntList[(int)Random.Range(0, dailyHuntList.Count - 1)];
    }

    // Gives the next quest in the story
    public void CompleteHunt(Hunt hunt)
    {

        //Hunt hunt = actor.currentQuest;

        //if (hunt.GetType() == typeof(StoryHunt))
        //{
        //    StoryHunt sh = (StoryHunt)hunt;
        //    int playerProgressionIndex = sh.ProgressionIndex;
        //    // Give the next story quest
        //    actor.currentQuest = storyHuntList[playerProgressionIndex + 1];
        //}
        //else if (hunt.GetType() == typeof(DailyHunt))
        //{

        //}

    }









}

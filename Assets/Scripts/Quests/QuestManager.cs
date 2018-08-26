using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    public static QuestManager Instance;

    [SerializeField]
    private List<StoryLine> storylines = new List<StoryLine>();
    //[SerializeField]private Queue<AdventurerAI> aiQueue;
    //private Queue<Hunt> questDialogQueue;

    public List<QuestEntryGroup<StoryQuest>> CreateNewStoryLines()
    {
        List<QuestEntryGroup<StoryQuest>> newStoryLine = new List<QuestEntryGroup<StoryQuest>>();
        foreach (StoryLine line in storylines)
        {
            QuestEntryGroup<StoryQuest> newLine = new QuestEntryGroup<StoryQuest>(line.JobType);
            newStoryLine.Add(newLine);
        }
        return newStoryLine;
    }

    public List<StoryLine> Storylines
    {
        get { return this.storylines; }
        set { this.storylines = value; }
    }

    void Awake()
    {
        Instance = this;
    }



    public void Clear()
    {
        storylines[0].Clear();
    }

    public StoryQuest GetQuest(int progressionIndex, JobType type)//Get Quest based of Entry group index
    {
        foreach (StoryLine myLine in storylines)
        {
            if (myLine.JobType == type)
            {
                if (progressionIndex < myLine.Quests.Count)
                    return myLine.Quests[progressionIndex];
                break;
            }
        }
        return null;
    }

    public bool CanStartQuest(QuestEntryGroup<StoryQuest> questEntry)
    {
        StoryQuest quest = GetQuest(questEntry.ProgressionIndex, questEntry.JobType);
        if (Player.Instance.Data.GetJob(quest.JobType).Level >= quest.RequiredLevel)
            return true;
        else
            return false;
    }

    //private void ProcessDialogQueue()
    //{

    //    if(!DialogManager.Instance.IsOccupied)
    //    {
    //        DialogManager.Instance.SetCurrentSession(questDialogQueue.Peek().Dialog);
    //        DialogManager.Instance.DisplayDialogBox();
    //        questDialogQueue.Dequeue();
    //    }    
    //}

    //private void ProcessQuests(AdventurerAI ai)
    //{

    //    if(ai.Quests.Count < NumberOfUnlockedStoryLines())
    //        GiveStartingQuests(ai.Quests);

    //    foreach(StoryHunt hunt in ai.Quests)
    //    {
    //        if(hunt.IsCompleted)
    //        {
    //            GetNextQuest(hunt);
    //        }

    //    }


    //    ai.IsConversing = false;
    //    aiQueue.Dequeue();

    //}

    // Gives the starting quest of each storyline if it does not exist
    //private void GiveStartingQuests(List<StoryQuest> list)
    //{

    //    if (list.Count < 1)
    //    {
    //        foreach(StoryLine sl in storylines)
    //        {
    //            list.Add(sl[0]);
    //        }
    //    }
    //    else
    //    {
    //        foreach (StoryLine sl in storylines)
    //        {
    //            bool exist = false;

    //            foreach (StoryQuest hunt in list)
    //            {
    //                if (sl[0].StoryType == hunt.StoryType)
    //                {
    //                    exist = true;
    //                }

    //                if (!exist)
    //                    list.Add(sl[0]);
    //            }
    //        }
    //    }

    //}



    // Check how many unlocked storylines
    //private int NumberOfUnlockedStoryLines()
    //{
    //    int i = 0;

    //    foreach(StoryLine sl in storylines)
    //    {
    //        if (sl.IsUnlocked)
    //            i++;
    //    }

    //    return i;
    //}


    // Set the dialog for the quest so that dialog manager knows what to display
    // Push quest dialog into a queue
    //private void StartQuestDialog(Hunt hunt)
    //{
    //    questDialogQueue.Enqueue(hunt);
    //}


    //----------------------------------------------------------------------------------------------------------//

    //public bool GetNextQuest(List<StoryHunt> list)
    //{

    //    List<StoryHunt> tempList = new List<StoryHunt>();

    //    for(int i = 0; i <list.Count;i++)
    //    {
    //        StoryHunt hunt =  list[i];

    //        StoryLine line = GetStoryLine(hunt.StoryType);

    //        if (hunt.IsCompleted && hunt.ProgressionIndex + 1 < line.Count
    //             && line[hunt.ProgressionIndex + 1].RequiredLevel <= Player.Instance.GetJob(hunt.JobType).Level)
    //        {
    //            //tempList.Add(line[hunt.ProgressionIndex + 1]);
    //            tempList.Add(hunt);

    //        }
    //    }


    //    if (tempList.Count > 0)
    //    {
    //        StoryHunt currentHunt = tempList[(int)Random.Range(0, tempList.Count)];
    //        currentHunt =  GetStoryLine(currentHunt.StoryType)[currentHunt.ProgressionIndex + 1];


    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }



    //}









}

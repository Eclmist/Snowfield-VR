using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestEntry<T> where T : Quest
{
	private bool hasStarted,isCompleted,checkedInVisit;
	private int timeToComplete;

	public QuestEntry()
	{
		hasStarted = false;
		isCompleted = false;
        checkedInVisit = false;
	}

    public bool Checked
    {
        get
        {
            return checkedInVisit;
        }
        set
        {
            checkedInVisit = value;
        }
    }

	//public Session Session
	//{
	//	get
	//	{
	//		if (!isCompleted)
	//			return currentQuest.Dialog;
	//		else
	//			return currentQuest.EndDialog;
	//	}
	//}

	public bool Completed
	{
		get
		{
			return isCompleted;
		}
		set
		{
			isCompleted = value;
		}
	}

	public bool Started
	{
		get
		{
			return hasStarted;
		}
		set
		{
			hasStarted = value;
		}
	}

	

	public void StartQuest(int time)
	{
		timeToComplete = time;
		hasStarted = true;
	}

	public void ProgressQuest()
	{
		timeToComplete--;
		if (timeToComplete <= 0)
			isCompleted = true;
	}
}

[System.Serializable]
public class QuestEntryGroup<T> where T : Quest
{
    private QuestEntry<T> currentEntry;

	private bool isCompleted;

	private int progressionIndex;

	private JobType jobType;

	public JobType JobType
	{
		get { return this.jobType; }
	}

	public QuestEntry<T> Quest
	{
		get
		{
            return currentEntry;
		}
        set
        {
            currentEntry = value;
        }
	}

	public int ProgressionIndex
	{
		get
		{
			return progressionIndex;
		}
		set
		{
			progressionIndex = value;
		}
	}

	public bool Completed
	{
		get
		{
			return isCompleted;
		}
		set
		{
			isCompleted = value;
		}
	}

	public QuestEntryGroup(JobType type)
	{
		isCompleted = false;
		progressionIndex = 0;
		jobType = type;
	}
}

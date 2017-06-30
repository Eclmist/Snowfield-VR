using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEntry<T> where T : Quest
{

	private T currentQuest;
	private bool hasStarted,isCompleted;
	private int timeToComplete;

	public QuestEntry(T quest)
	{
		hasStarted = false;
		isCompleted = false;
		currentQuest = quest;
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

	public T Quest
	{
		get
		{
			return currentQuest;
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

public class QuestEntryGroup<T> where T : Quest
{
	private List<QuestEntry<T>> quests;

	private bool isCompleted;

	private int progressionIndex;

	private JobType jobType;

	public JobType JobType
	{
		get { return this.jobType; }
	}

	public List<QuestEntry<T>> Quest
	{
		get
		{
			return quests;
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
		quests = new List<QuestEntry<T>>();
		progressionIndex = 0;
		jobType = type;
	}

	public int Count
	{
		get { return quests.Count; }
	}

	public void Add(QuestEntry<T> _item)
	{
		quests.Add(_item);
	}

	public QuestEntry<T> this[int index]   // Indexer declaration  
	{
		get
		{
			return quests[index];
		}
		set
		{
			quests[index] = value;
		}
	}

}

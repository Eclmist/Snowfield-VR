using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartQuestInteraction : QuestInteraction
{

    public override bool StartInteraction()
    {
        hasInteracted = true;
        if (currentAI is AdventurerAI)
        {
            QuestEntryGroup<StoryQuest> startableQuest = (currentAI as AdventurerAI).QuestBook.GetStartableGroup();

            if (startableQuest != null)
            {
                int expectedCrateNumber = QuestManager.Instance.GetQuest(startableQuest.ProgressionIndex,startableQuest.JobType).ExpectedCrates;
                OptionPane op = UIManager.Instance.Instantiate(UIType.OP_YES_NO,
                    "Quest", "<b>Start Quest: " + QuestManager.Instance.GetQuest(startableQuest.ProgressionIndex, startableQuest.JobType).Name + " \n(Requires " + expectedCrateNumber + " EXPCrates) </b>",
                    transform.position, Player.Instance.transform, transform);
                op.SetEvent(OptionPane.ButtonType.Yes, StartQuestYESDelegate);
				if (expectedCrateNumber > Player.Instance.EXPCrates)
				{
					op.GetButton(0).GetComponent<Collider>().enabled = false;
					Debug.Log("Checked");
				}

                currentQuestGroup = startableQuest;
                currentUI = op;
                return true;
            }
        }
        return false;
    }

    public void StartQuestYESDelegate()
    {
        currentQuestGroup.Quest.StartQuest();
    }

}

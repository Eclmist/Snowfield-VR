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
                int expectedCrateNumber = QuestManager.Instance.GetQuest(startableQuest).ExpectedCrates;
                OptionPane op = UIManager.Instance.Instantiate(UIType.OP_YES_NO,
                    "Quest", "Start Quest: " + QuestManager.Instance.GetQuest(startableQuest).Name + " \n(Requires " + expectedCrateNumber + " EXPCrates)",
                    transform.position, Player.Instance.transform, transform);
                op.SetEvent(OptionPane.ButtonType.Yes, StartQuestYESDelegate);
				if (expectedCrateNumber > Player.Instance.EXPBottles)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteQuestInteraction : QuestInteraction
{


    public override bool StartInteraction()
    {
        hasInteracted = true;
        QuestEntryGroup<StoryQuest> completableGroup = currentAI.QuestBook.GetCompletableGroup();
        if (completableGroup != null)
        {
            OptionPane op = UIManager.Instance.Instantiate(UIType.OP_OK,
                "Quest", "Complete Quest: " + QuestManager.Instance.GetQuest(completableGroup),
                transform.position, Player.Instance.transform, transform);
            op.SetEvent(OptionPane.ButtonType.Ok, CompleteQuestDelegate);
            currentQuestGroup = completableGroup;
            currentPane = op;
            return true;
        }
        return false;
    }

    public void CompleteQuestDelegate()
    {
        StoryQuest quest = QuestManager.Instance.GetQuest(currentQuestGroup);
        currentAI.GainExperience(quest.Experience);
        currentAI.QuestBook.RequestNextQuest(currentQuestGroup);
    }


}
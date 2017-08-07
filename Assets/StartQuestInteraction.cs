﻿using System.Collections;
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
                OptionPane op = UIManager.Instance.Instantiate(UIType.OP_YES_NO,
                    "Quest", "Start Quest: " + QuestManager.Instance.GetQuest(startableQuest).Name,
                    transform.position, Player.Instance.transform, transform);
                op.SetEvent(OptionPane.ButtonType.Yes, StartQuestYESDelegate);
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
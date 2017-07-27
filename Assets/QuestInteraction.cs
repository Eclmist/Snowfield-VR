using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AdventurerAI))]
public abstract class QuestInteraction : InteractionsWithPlayer
{

    protected QuestEntryGroup<StoryQuest> currentQuestGroup;

}




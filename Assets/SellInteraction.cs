﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellInteraction : InteractionsWithPlayer {

    protected ItemData sellItemData;
    public override bool StartInteraction()
    {
        hasInteracted = true;
        sellItemData = ItemManager.Instance.GetRandomUnlockedItem();

        if (sellItemData != null)
        {
            OptionPane op = UIManager.Instance.Instantiate(UIType.OP_OK, "SellItem", currentAI.Data.Name + " has sold you " + sellItemData.ObjectReference.name, transform.position, Player.Instance.transform, transform);
            op.SetEvent(OptionPane.ButtonType.Ok, SellItemDelegate);
            currentPane = op;
            return true;
        }

        return false;
    }

    protected void SellItemDelegate()
    {
        GameManager.Instance.AddPlayerGold(25);
        GameManager.Instance.AddToPlayerInventory(sellItemData);

    }
}
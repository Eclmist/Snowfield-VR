using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderInteraction : InteractionsWithPlayer
{
    protected Order currentOrder;
    public override bool StartInteraction()
    {
        hasInteracted = true;
        if (!OrderBoard.Instance.IsMaxedOut)
        {
            currentOrder = OrderManager.Instance.GenerateOrder();
            if (currentOrder != null)
            {
                OptionPane op = UIManager.Instance.Instantiate(UIType.OP_YES_NO, "Order", "Start Order: " + currentOrder.Name, transform.position, Player.Instance.transform, transform);
                op.SetEvent(OptionPane.ButtonType.Yes, StartOrderYesDelegate);
                op.SetEvent(OptionPane.ButtonType.No, StartOrderNoDelegate);
                currentPane = op;
                return true;
            }
        }
        return false;
    }

    public void StartOrderYesDelegate()
    {
        OrderManager.Instance.StartRequest(currentAI, currentOrder);
    }

    public void StartOrderNoDelegate()
    {
        currentAI = null;
    }

}

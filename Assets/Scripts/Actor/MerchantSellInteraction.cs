using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantSellInteraction : InteractionsWithPlayer {

    Merchant merchant;

    protected override void Awake()
    {
        base.Awake();
        merchant = GetComponent<Merchant>();
        if (!merchant)
        {
            Destroy(this);
        }
    }

    public override bool StartInteraction()
    {
        hasInteracted = true;
        currentUI = merchant.SpawnMerchantPanel();
        return true;
    }
}

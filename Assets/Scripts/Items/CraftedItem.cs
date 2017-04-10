using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftedItem : GenericItem {

    protected virtual void UseItem()
    {
        Debug.Log("You are using " + this.name);
    }

    void Start()
    {

    }


	
}

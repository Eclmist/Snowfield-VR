using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment ,IBlockable {

    private bool blocked;

    public bool Blocked
    {
        get
        {
            return blocked;
        }
        set
        {
            blocked = value;
        }
    }

    protected override void UseItem()
    {
        base.UseItem();
    }

    protected override void OnTriggerStay(Collider collision)
    {
        base.OnTriggerStay(collision);
        if (!blocked)
        {
            IBlock item = collision.GetComponent<IBlock>();
            if (item != null && item.CanBlock)
            {
                blocked = true;
            }
        }
    }

    protected override void OnTriggerExit(Collider collision)
    {
        base.OnTriggerExit(collision);
        if (blocked)
        {
            IBlock item = collision.GetComponent<IBlock>();
            if (item != null && item.CanBlock)
            {
                blocked = false;
            }
        }
    }


}

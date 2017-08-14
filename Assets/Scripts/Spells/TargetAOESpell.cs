using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAOESpell : Spell {

    [SerializeField]
    protected float attackRadius;

    protected float nearestDistance = 0;

    protected GameObject monster;

    protected GameObject nearestMob;

    public override void InitializeSpell(SpellHandler handler)
    {
        base.InitializeSpell(handler);

        nearestMob = CheckForMonsterDistance();

        if(nearestMob != null)
        {
            transform.position = new Vector3(nearestMob.transform.position.x, nearestMob.transform.position.y + 4, nearestMob.transform.position.z);
            transform.parent = nearestMob.transform;
        }
        
    }

    protected GameObject CheckForMonsterDistance()
    {

        Collider[] hitColliders = Physics.OverlapSphere(Player.Instance.transform.position, 7);

        foreach (Collider c in hitColliders)
        {
            Monster mob = c.GetComponent<Monster>();

            float distance = Vector3.Distance(Player.Instance.transform.position, mob.gameObject.transform.position);

            if(distance <= nearestDistance || nearestDistance == 0)
            {
                monster = mob.gameObject;
                nearestDistance = distance;
            }
        }

        return monster;
    }

}

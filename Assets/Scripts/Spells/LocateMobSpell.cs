using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocateMobSpell : MonoBehaviour {

    [SerializeField]
    protected RFX4_ParticleTrail trail;

    protected GameObject monster;
    protected float nearestDistance = 0;

    [SerializeField]
    protected GameObject point;

    // Use this for initialization
    void OnEnable()
    {
        if (trail.Target != null) trail.targetT = trail.Target.transform;

        if (trail.Target == null)
        {
            Collider[] col = Physics.OverlapSphere(point.transform.position, 4);

            if (col != null)
            {
                foreach (Collider c in col)
                {
                    Monster mob = c.GetComponent<Monster>();

                    if (mob)
                        monster = mob.gameObject;

                }

                if (monster != null)
                {
                    trail.Target = monster;
                    trail.targetT = trail.Target.transform;
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (monster == null || trail.Target.GetComponent<Monster>().StatContainer.GetStat(Stats.StatsType.HEALTH).Current <= 0 || trail.Target == null)
        {
            Collider[] col = Physics.OverlapSphere(point.transform.position, 4);

            if (col != null)
            {
                foreach (Collider c in col)
                {
                    Monster mob = c.GetComponent<Monster>();

                    if (mob)
                        monster = mob.gameObject;

                }

                if (monster != null)
                {
                    trail.Target = monster;
                    trail.targetT = trail.Target.transform;
                }
            }
        }
    }
}

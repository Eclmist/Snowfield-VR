using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerCollider : MonoBehaviour {

    [SerializeField]
    protected int DPS;

    protected float seconds;

    //protected Collider[] collider;

    protected void Start()
    {
        //if(transform.childCount > 0)
        //{
        //    for(int i = 0; i < transform.childCount; i++)
        //    {
        //        if(transform.GetChild(i).name == "DamageCollider")
        //        {
        //            collider = GetComponentsInChildren<Collider>();
        //        }
        //    }
        //}
    }

    private void OnTriggerStay(Collider other)
    {
        seconds += Time.deltaTime;

        if (seconds >= 0.5f)
        {
            Monster mob = other.GetComponent<Monster>();
			if (mob)
				Player.Instance.CastSpell(Player.Instance.StatContainer.GetStat(Stats.StatsType.MAGIC).Current * (DPS / 2), mob);

            seconds = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        seconds = 0;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCollider : MonoBehaviour {

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
            other.GetComponent<Monster>().TakeDamage(DPS / 2, Player.Instance);

            seconds = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        seconds = 0;
    }
}
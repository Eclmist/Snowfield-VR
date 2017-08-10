using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : AI {

    [SerializeField]
    protected MonsterData data;


    public override ActorData Data
    {
        get
        {
            return data;
        }

        set
        {
            data = (MonsterData)value;
        }
    }
    public override void Interact(Actor actor)
    {
        Debug.Log("Monsters cant interact");
        throw new NotImplementedException();//Monsters cant interact atm
    }

    public override void Despawn()
    {
        base.Despawn();
        Destroy(gameObject, 10);
    }

    public override void TakeDamage(float damage, Actor attacker, JobType type)
    {
        base.TakeDamage(damage, attacker, type);
        if(StatContainer.GetStat(Stats.StatsType.HEALTH).Current <= 0)
        {
            attacker.GainExperience(type, data.EXPGainedWhenKilled);
        }
    }


}

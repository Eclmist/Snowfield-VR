using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class StatsContainer : MonoBehaviour
{
    private ActiveStats[] allStats;
    private float[] baseStats;
    private bool activated = false;
    private Actor actor;

    [SerializeField]
    private float baseHealth, baseHealthRegen, baseAttack, baseMana, baseManaRegen, baseMagic;


    private ActiveStats health, healthRegen, attack, mana, manaRegen, magic;

    protected virtual void Start()
    {
        actor = GetComponent<Actor>();
        ActivateStats();

        if (actor == null)
        {
            Debug.Log("No actor");
            Destroy(this);
        }
        UpdateVariables();
    }

    protected void ActivateStats()
    {
        activated = true;
        health = new ActiveStats(Stats.StatsType.HEALTH, baseHealth);
        healthRegen = new ActiveStats(Stats.StatsType.HEALTHREGENERATION, baseHealthRegen);
        attack = new ActiveStats(Stats.StatsType.ATTACK, baseAttack);
        mana = new ActiveStats(Stats.StatsType.MANA, baseMana);
        magic = new ActiveStats(Stats.StatsType.MAGIC, baseMagic);
        manaRegen = new ActiveStats(Stats.StatsType.MANAREGENERATION, baseManaRegen);
        allStats = new ActiveStats[] { health, healthRegen, attack, mana, magic, manaRegen };
        baseStats = new float[] { baseHealth, baseHealthRegen, baseAttack, baseMana, baseMagic, baseManaRegen };
    }


    public ActiveStats GetStat(Stats.StatsType type)
    {
        foreach (ActiveStats s in allStats)
        {
            if (s.Type == type)
                return s;
        }
        return null;
    }
    // Update is called once per frame


    void Update()
    {
        HandleStatsInteraction();
    }


    protected void HandleStatsInteraction()
    {
        if (health != null && healthRegen != null && health.Current > 0)
        {
            if (health.Current < health.Max)
            {
                health.Current += healthRegen.Current * Time.deltaTime;
                if (health.Current > health.Max)
                    health.Current = health.Max;
            }

            if (mana != null && manaRegen != null && mana.Current < mana.Max)
            {
                mana.Current += manaRegen.Current * Time.deltaTime;
                if (mana.Current > mana.Max)
                    mana.Current = mana.Max;
            }
        }
    }

    public void ReduceHealth(float amount)
    {
        ActiveStats health = GetStat(Stats.StatsType.HEALTH);
        if (health != null)
        {
            health.Current -= amount;
            if (health.Current < 0)
                health.Current = 0;
        }
        //hitSound.Play();
    }


    public void ReduceMana(float amount)
    {
        ActiveStats mana = GetStat(Stats.StatsType.MANA);
        if (mana != null)
        {
            mana.Current -= amount;
            if (mana.Current < 0)
                mana.Current = 0;
        }
    }


    public void ResetCurrentVariables()
    {
        if (activated)
            foreach (ActiveStats stat in allStats)
            {
                stat.Current = stat.Max;
            }
    }

    public void UpdateVariables()
    {
        if (activated)
        {
            for (int i = 0; i < allStats.Length; i++)
            {
                allStats[i].Max = baseStats[i] + actor.GetBonusStatValueFromJob(allStats[i].Type);
            }
            ResetCurrentVariables();
        }
    }



}

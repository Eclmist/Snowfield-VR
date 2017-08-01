using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class StatsContainer : MonoBehaviour
{
    [SerializeField]
    private List<Stats> baseStats = new List<Stats>();
    [SerializeField]
    private List<ActiveStats> allStats = new List<ActiveStats>();

    private Actor actor;

    

    protected virtual void Start()
    {
        actor = GetComponent<Actor>();
        foreach(Stats stat in baseStats)
        {
            ActiveStats newStat = new ActiveStats(stat.Type, stat.Max);
            allStats.Add(newStat);
        }
        if (actor == null)
        {
            Debug.Log("No actor");
            Destroy(this);
        }

        

        ResetCurrentVariables();
        UpdateVariables();
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
        ActiveStats health = GetStat(Stats.StatsType.HEALTH);
        ActiveStats healthRegeneration = GetStat(Stats.StatsType.HEALTHREGENERATION);
        ActiveStats mana = GetStat(Stats.StatsType.MANA);
        ActiveStats manaRegeneration = GetStat(Stats.StatsType.MANAREGENERATION);

        if (health != null && healthRegeneration != null && health.Current > 0)
        {
            if (health.Current < health.Max)
            {
                health.Current += healthRegeneration.Current * Time.deltaTime;
                if (health.Current > health.Max)
                    health.Current = health.Max;
            }

            if (mana!= null && manaRegeneration != null && mana.Current < mana.Max)
            {
                mana.Current += manaRegeneration.Current * Time.deltaTime;
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
        foreach (ActiveStats stat in allStats)
        {
            stat.Current = stat.Max;
        }
    }

    public void UpdateVariables()
    {
        for (int i = 0; i < allStats.Count; i++)
        {
            allStats[i].Max = baseStats[i].Max + actor.GetBonusStatValueFromJob(allStats[i].Type);
        }
    }

    public void AddStats(Stats _baseState)
    {
        baseStats.Add(_baseState);
        ActiveStats newStat = new ActiveStats(_baseState.Type, _baseState.Max);
        allStats.Add(newStat);
    }

}

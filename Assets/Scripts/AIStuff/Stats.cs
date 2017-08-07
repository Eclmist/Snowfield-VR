using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Stats
{
    public enum StatsType
    {
        HEALTH,
        HEALTHREGENERATION,
        ATTACK,
        MANA,
        MANAREGENERATION,
        MAGIC
    }

    [SerializeField]
    protected float value;

    [SerializeField]
    protected StatsType statsType;
    public float Max
    {
        get
        {
            return value;
        }
        set
        {
            this.value = value;
        }
    }

    public StatsType Type
    {
        get
        {
            return statsType;
        }
    }

    public Stats(StatsType _type, float _maxValue)
    {
        value = _maxValue;
        statsType = _type;
    }

    public Stats(Stats copyObject)
    {
        value = copyObject.Max;
        statsType = copyObject.Type;
    }

}

[System.Serializable]
public class ActiveStats : Stats
{
    [SerializeField]
    protected float currentValue;

    public float Current
    {
        get
        {
            return currentValue;
        }
        set
        {
            currentValue = value;
        }
    }

    public ActiveStats(StatsType _type, float _maxValue) : base(_type, _maxValue) { }

    public float Percentage
    {
        get
        {
            return currentValue / value;
        }
    }
}

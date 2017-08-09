using System.Collections;
using System.Collections.Generic;

public class GameConstants{

    public static GameConstants Instance = new GameConstants();

    private float experienceRate = 1, experienceConstant = 0.5F;//Or whatever value

    private float respawnTimePerLevel = 5f, dropItemDespawnRate = 30f;

    private float dropRate = 0.5f;

    public float ItemDropRate
    {
        get
        {
            return dropRate;
        }
    }
    public float RespawnTimer
    {
        get
        {
            return respawnTimePerLevel;
        }
    }

    public float DroppedDespawnTimer
    {
        get
        {
            return dropItemDespawnRate;
        }
    }

    public float ExpRate
    {
        get
        {
            return experienceRate;
        }
        set
        {
            experienceRate = value;
        }
    }

    public float ExpConstant
    {
        get
        {
            return experienceConstant;
        }
    }
}

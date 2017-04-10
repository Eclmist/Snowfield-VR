using System.Collections;
using System.Collections.Generic;

public class GameConstants{

    public static GameConstants Instance = new GameConstants();

    private float experienceRate = 1, experienceConstant = 0.5F;//Or whatever value

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

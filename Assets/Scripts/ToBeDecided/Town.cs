using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour
{//Can be used to decide the type of adventurers/structures etc

    private float population;
    
    public float Population//used to decide how many requests/day etc
    {
        get
        {
            return population;
        }
    }

    public Town(float _difficulty)
    {
        population = _difficulty;
    }
    
}

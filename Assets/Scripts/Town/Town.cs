using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town
{//Can be used to decide the type of adventurers/structures etc

    private int population;

    private List<Shop> allShops = new List<Shop>();

    public int Population//used to decide how many requests/day etc
    {
        get
        {
            return population;
        }
    }

    public Town(int _difficulty)
    {
        population = _difficulty;
    }
    
    public List<Shop> Shops
    {
        get
        {
            return allShops;
        }
    }
}

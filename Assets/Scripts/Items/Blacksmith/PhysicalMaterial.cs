using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TYPE
{
    IRON,
    COPPER,
    STEEL,
    BRONZE,
    ADAMANTITE,
    PLATINUM
};

[System.Serializable]
public class PhysicalMaterial {

    [SerializeField]private string m_name;
    [SerializeField]private int costMultiplier;
    

    [SerializeField]
    [Range(1, 100)]
    protected float conductivity; // Acts as a multiplier for the heating rate

    public TYPE Type;

    public int CostMultiplier
    {
        get { return this.costMultiplier; }
    }

    public string Name
    {
        get { return this.m_name; }
    }

    public float Conductivity
    {
        get { return this.conductivity; }
    }
    



}

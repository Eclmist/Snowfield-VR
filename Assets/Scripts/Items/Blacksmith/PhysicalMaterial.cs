using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TYPE
{
    IRON,
    COPPER,
    STEEL
};

[System.Serializable]
public class PhysicalMaterial {

    [SerializeField]private int costMultiplier;
    [SerializeField]private string m_name;

    public TYPE Type;

    public int CostMultiplier
    {
        get { return this.costMultiplier; }
    }

    public string Name
    {
        get { return this.m_name; }
    }
    



}

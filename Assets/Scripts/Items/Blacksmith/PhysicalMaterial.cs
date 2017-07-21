using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PhysicalMaterial {

	public enum Type
	{
		IRON,
		COPPER,
		STEEL,
		BRONZE,
		ADAMANTITE,
		PLATINUM
	};

	[SerializeField]private string m_name;
    [SerializeField]private int costMultiplier;
    

    [SerializeField]
    [Range(1, 100)]
    protected float conductivity; // Acts as a multiplier for the heating rate

    public PhysicalMaterial.Type type;

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

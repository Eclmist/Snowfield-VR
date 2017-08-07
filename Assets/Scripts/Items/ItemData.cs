using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ItemData : IStorable, IComparable {

    [SerializeField]
    private int ID;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private int levelUnlocked;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private int maxStackSize = 1;
    [SerializeField]
    private JobType jobType;
    [SerializeField]
    private int cost;

    
    public int ItemID
    {
        get { return this.ID; }
    }

    public GameObject ObjectReference
    {
        get { return this.prefab; }
    }

    public int LevelUnlocked
    {
        get { return this.levelUnlocked; }
        set { this.levelUnlocked = value; }
    }

    public Sprite Icon
    { 
        get { return this.icon; }
    }



    public int MaxStackSize
    {
        get { return this.maxStackSize; }
    }

    public JobType JobType
    {
        get { return this.jobType; }
    }

    public int Cost
    {
        get { return this.cost; }
    }

	public int CompareTo(object obj)
	{
		Weapon currentWeapon = ObjectReference.GetComponent<Weapon>();

		if (!currentWeapon)
			return ID.CompareTo(((ItemData)obj).ID);
		else
		{
			Weapon otherWeapon = ((ItemData)obj).ObjectReference.GetComponent<Weapon>();

			return currentWeapon.WeaponTier.CompareTo(otherWeapon.WeaponTier);
		}
	}
}

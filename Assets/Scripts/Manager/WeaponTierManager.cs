using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTierManager : MonoBehaviour {


    // Weapon Classes are grouped based on Type
    [System.Serializable]
    public class WeaponClass
    {
        [SerializeField]
        private PhysicalMaterial.Type type;
        
        [SerializeField]
        [Range(1,100f)]
        private float successRate;
        
        [SerializeField][Header("Item ID")]
        private List<ItemData> itemList;


        public PhysicalMaterial.Type Type
        {
            get { return this.type; }
        }

        public float SuccessRate
        {
        	get {return this.successRate;}
        }

        public List<ItemData> TierList
        {
            get { return this.itemList; }
        }


    }

    public static WeaponTierManager Instance;


    [SerializeField]
    private List<WeaponClass> weaponClassList = new List<WeaponClass>();

    void Awake()
    {
        Instance = this;
    }

	private void Start()
	{

		List<ItemData> itemDataList = ItemManager.Instance.ItemDataList;

		foreach (ItemData item in itemDataList)
		{
			Weapon weaponScript = item.ObjectReference.GetComponent<Weapon>();

			if (weaponScript)
			{
				foreach (WeaponClass wc in weaponClassList)
				{
					if (wc.Type == weaponScript.GetPhysicalMaterial())
					{
						wc.TierList.Add(item);
						break;
					}
				}
			}
		}

		foreach (WeaponClass wc in weaponClassList)
		{
			wc.TierList.Sort();
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

    public List<WeaponClass> WeaponClassList
    {
        get { return this.weaponClassList; }
    }

    public int GetNumberOfTiersInClass(PhysicalMaterial.Type t)
    {
        foreach(WeaponClass wc in weaponClassList)
        {
            if(t == wc.Type)
            {
                return wc.TierList.Count;
               
            }
        }

        return 0;
    }


    public ItemData GetWeapon(PhysicalMaterial.Type type, int tier)
    {
        foreach(WeaponClass wc in weaponClassList)
        {
            if(wc.Type == type)
            {
                if (wc.TierList.Count < 1)
                    return null;
                else
                    return wc.TierList[tier];
            }
        }

        return null;
    }

    public ItemData GetRandomWeaponInTypeClass(PhysicalMaterial.Type type)
    {
        WeaponClass tempClass = null;

        foreach(WeaponClass wc in weaponClassList)
        {
            if (wc.Type == type)
            {
                tempClass = wc;
                break;
            }      
        }


        if (tempClass != null)
        {
            if(tempClass.TierList.Count > 0)
            {
                return tempClass.TierList[Random.Range(0, tempClass.TierList.Count)];
            }  
        }

        return null;

    }

    public float GetSuccessRateForTier(PhysicalMaterial.Type type)
    {
    	foreach(WeaponClass wc in weaponClassList)
    	{
    		if(wc.Type == type)
    		{

    			return wc.SuccessRate;
    		}
    	}


    	return -1;
    }


}

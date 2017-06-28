using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTierManager : MonoBehaviour {


    // Weapon Classes are grouped based on Type
    [System.Serializable]
    public class WeaponClass
    {
        [SerializeField]
        private TYPE type;

        [SerializeField][Header("Item ID")]
        private List<int> tierList;


        public TYPE Type
        {
            get { return this.type; }
        }

        public List<int> TierList
        {
            get { return this.tierList; }
        }


    }

    public static WeaponTierManager Instance;


    [SerializeField]
    private List<WeaponClass> weaponClassList = new List<WeaponClass>();

    void Awake()
    {
        Instance = this;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public List<WeaponClass> WeaponClassList
    {
        get { return this.weaponClassList; }
    }

    public int GetNumberOfTiersInClass(TYPE t)
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


    public ItemData GetWeapon(TYPE type, int tier)
    {
        foreach(WeaponClass wc in weaponClassList)
        {
            if(wc.Type == type)
            {
                if (wc.TierList.Count < 1)
                    return null;
                else
                    return ItemManager.Instance.GetItemData(wc.TierList[tier]);
            }
        }

        return null;
    }


}

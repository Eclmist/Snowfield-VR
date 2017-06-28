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


    }

    public static WeaponTierManager Instance;
    public List<WeaponClass> weaponClassList = new List<WeaponClass>();

	// Use this for initialization
	void Start () {
		
	}

    void Awake()
    {
        Instance = this;
    }
	
	// Update is called once per frame
	void Update () {
		
	}



}

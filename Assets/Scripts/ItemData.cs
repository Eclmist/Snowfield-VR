using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData {

    [SerializeField]
    private int ID;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private bool isUnlocked;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private int maxStackSize = 1;



    public int ItemID
    {
        get { return this.ID; }
    }

    public GameObject ObjectReference
    {
        get { return this.prefab; }
    }

    public bool IsUnlocked
    {
        get { return this.isUnlocked; }
        set { this.isUnlocked = value; }
    }

    public Sprite Icon
    { 
        get { return this.icon; }
    }



    public int MaxStackSize
    {
        get { return this.maxStackSize; }
    }



}

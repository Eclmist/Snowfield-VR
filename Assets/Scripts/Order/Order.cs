using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Order
{

    private int duration;//i feel like having an actual time here would be better 
    private int goldReward;
    private int id;
    private PhysicalMaterial.Type materialType;
    private float elapsedTime;
    private string aiData;

    public Order(int _duration, int _goldReward, int _id)
    {
        duration = _duration;
        goldReward = _goldReward;
        id = _id;
    }

    public Order(int _duration, int _goldReward, int _id, PhysicalMaterial.Type type)
    {

        duration = _duration;
        goldReward = _goldReward;
        id = _id;
        materialType = type;
    }


    public string Name
    {
        get { return ItemManager.Instance.GetItemData(ItemID).GenericItem._Name; }
    }

    public Sprite Sprite
    {
        get { return ItemManager.Instance.GetItemData(ItemID).Icon; }
    }
    public int Duration
    {
        get { return this.duration; }
    }

    public int GoldReward
    {
        get { return this.goldReward; }
    }

    public int ItemID
    {
        get { return this.id; }
    }

    public PhysicalMaterial.Type MaterialType
    {
        get { return this.materialType; }
    }

    public float ElapsedTime
    {
        get { return elapsedTime; }
        set { elapsedTime = value; }
    }

    public string AIData
    {
        get { return this.aiData; }
        set { this.aiData = value; }
    }



}

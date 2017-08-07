using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CoolDown {

    [SerializeField]
    protected string name;
    [SerializeField]
    protected float coolDown;

    protected float currentCoolDown;

    public string Name
    {
        get
        {
            return name;
        }
    }

    public float MaxCoolDown
    {
        get
        {
            return coolDown;
        }
    }

    public float CurrentCoolDown
    {
        get
        {
            return currentCoolDown;
        }
        set
        {
            currentCoolDown = value;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relation {

    private Actor target;
    private float relationValue;

    public Relation(Actor _target)
    {
        target = _target;
        relationValue = 0;
    }

    public Relation(Actor _target, float _value)
    {
        target = _target;
        relationValue = _value;
    }

    public Actor Target
    {
        get
        {
            return target;
        }
    }

    public float RelationValue
    {
        get
        {
            return relationValue;
        }

        set
        {
            relationValue = value;
        }
    }

    public bool IsHostile
    {
        get
        {
            return relationValue < 0;
        }
    }
}

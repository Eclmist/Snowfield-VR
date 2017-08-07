using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakeDamage(float damage, Actor attacker);

    bool CanBeAttacked
    {
        get;
    }
    Transform transform
    {
        get;
    }

    Collider Collider
    {
        get;
    }
}

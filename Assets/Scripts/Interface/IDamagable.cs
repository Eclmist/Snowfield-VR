using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{

    int Health
    {
        get;
    }

    void TakeDamage(int damage, Actor attacker);

    bool CanBeAttacked
    {
        get;
    }
    Transform transform
    {
        get;
    }
}

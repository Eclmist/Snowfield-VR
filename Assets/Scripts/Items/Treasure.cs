using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Treasure : MonoBehaviour, IDamagable {




    public bool CanBeAttacked
    {
        get
        {
            return true;
        }
    }

    public int Health
    {
        get
        {
            return Player.Instance.Gold;
        }
    }


    public void TakeDamage(float Damage,Actor actor)
    {
        actor.TakeDamage(99999, null);
        GameManager.Instance.AddPlayerGold((int)-Damage);
     
    }
	public new Transform transform
    {
        get
        {
            if (gameObject)
                return base.transform;
            else
                return null;
        }
    }
}

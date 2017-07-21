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
    protected void Awake()
    {
 
    }
    

    public int Health
    {
        get
        {
            return Player.Instance.Gold;
        }
    }


    public void TakeDamage(int Damage,Actor actor)
    {
        actor.TakeDamage(99999, null);
        GameManager.Instance.AddPlayerGold(-Damage - actor.Health);
     
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

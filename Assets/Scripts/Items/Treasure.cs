using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CombatVariable))]
public class Treasure : MonoBehaviour, IDamagable,IHasVariable {

    protected CombatVariable variable;

    [SerializeField]
    protected int maxHealth, healthRegeneration;

    public bool CanBeAttacked
    {
        get
        {
            return true;
        }
    }
    protected void Awake()
    {
        variable = GetComponent<CombatVariable>();
    }
    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }

    public int Health
    {
        get
        {
            return variable.GetCurrentHealth();
        }
    }
    public int HealthRegeneration
    {
        get
        {
            return healthRegeneration;
        }
    }

    public void TakeDamage(int Damage,Actor actor)
    {
        variable.ReduceHealth(Damage);
        if (Health <= 0)
        {
            actor.TakeDamage(9999999, null);
            gameObject.SetActive(false);
        }
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

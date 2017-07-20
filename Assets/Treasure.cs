using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour, IDamagable,IHasVariable {

    protected CombatVariable variable;

    [SerializeField]
    protected int maxHealth, healthRegeneration;

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

            Destroy(gameObject);
        }
    }
	public new Transform transform
    {
        get
        {
            return base.transform;
        }
    }
}

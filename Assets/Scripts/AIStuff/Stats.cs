using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats {

    [SerializeField]
    private int baseHealth;
    [SerializeField]
    private int baseAttack;
    [SerializeField]
    private int baseHealthRegeneration;
    [SerializeField]
    private int baseMovementSpeed;

    public Stats(int _health = 0, int _attack = 0, int _healthRegeneration = 0, int _movementSpeed = 0)
    {
        baseAttack = _attack;
        baseHealthRegeneration = _healthRegeneration;
        baseHealth = _health;
        baseMovementSpeed = _movementSpeed;
    }

    public Stats(Stats copy)
    {
        baseHealth = copy.Health;
        baseAttack = copy.Attack;
        baseHealthRegeneration = copy.HealthRegeneration;
        baseMovementSpeed = copy.MovementSpeed;
    }

    public int Health
    {
        get
        {
            return baseHealth;
        }
    }

    public int Attack
    {
        get
        {
            return baseAttack;
        }
    }

    public int HealthRegeneration
    {
        get
        {
            return baseHealthRegeneration;
        }
    }

    public int MovementSpeed
    {
        get
        {
            return baseMovementSpeed;
        }
    }

}

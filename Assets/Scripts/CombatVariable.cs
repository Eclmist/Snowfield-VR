using UnityEngine;
using System.Collections;

public class CombatVariable : MonoBehaviour
{

    public AudioSource hitSound;

    [SerializeField]
    private int currentHealth;

    private float timeSinceLastDamageTaken;

    private Actor actor;
    // Use this for initialization
    void Start()
    {
        actor = GetComponent<Actor>();
        currentHealth = actor.MaxHealth;
        timeSinceLastDamageTaken = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastDamageTaken += Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (timeSinceLastDamageTaken > 10 && currentHealth < actor.MaxHealth)
        {
            currentHealth += actor.HealthRegeneration;
            if (currentHealth > actor.MaxHealth)
                currentHealth = actor.MaxHealth;
        }
    }

    public void ReduceHealth(int amount)
    {
        timeSinceLastDamageTaken = 0;
        currentHealth -= amount;
        hitSound.Play();
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

}

using UnityEngine;
using System.Collections;

public class CombatVariable : MonoBehaviour
{
    [SerializeField]
    private int currentHealth;

    private float timeSinceLastDamageTaken;

    private IHasVariable iHasVariable;
    // Use this for initialization
    void Start()
    {
        iHasVariable = GetComponent<IHasVariable>();
        currentHealth = iHasVariable.MaxHealth;
        timeSinceLastDamageTaken = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastDamageTaken += Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (currentHealth > 0)
        {
            if (timeSinceLastDamageTaken > 10 && currentHealth < iHasVariable.MaxHealth)
            {
                currentHealth += iHasVariable.HealthRegeneration;
                if (currentHealth > iHasVariable.MaxHealth)
                    currentHealth = iHasVariable.MaxHealth;
            }
        }
    }

    public void ReduceHealth(int amount)
    {
        timeSinceLastDamageTaken = 0;
        currentHealth -= amount;
        //hitSound.Play();
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

}

using UnityEngine;
using System.Collections;

public class FirePowerExplosion : MonoBehaviour {

    public float timeTillDeath;
    public float explosionForce;
    public float explosionSize;

	void Start ()
    {
        StartCoroutine(DestroySelf());
	}
	
	void Update ()
    {
	
	}

    IEnumerator DestroySelf ()
    {
        ExplodeAroundMe();
        yield return new WaitForSeconds(timeTillDeath);
        Destroy(gameObject);
    }

    void ExplodeAroundMe()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {

            // if it's a ragdoll make non-kinematic
            if (enemy.GetComponent<Ragdoll>() != null)
            {
                Ragdoll ragdoll = enemy.GetComponent<Ragdoll>();
                ragdoll.TriggerWarning();
                foreach (Rigidbody rb in ragdoll.myParts)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionSize);
                }
            }

            else if (enemy.GetComponent<Rigidbody>() != null)
            {
                Rigidbody rb = enemy.GetComponent<Rigidbody>();
                rb.AddExplosionForce(explosionForce, transform.position, explosionSize);
            }
        }
    }
}

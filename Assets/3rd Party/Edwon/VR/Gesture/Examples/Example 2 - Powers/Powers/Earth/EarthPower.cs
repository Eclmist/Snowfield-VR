using UnityEngine;
using System.Collections;

public class EarthPower : MonoBehaviour
{
    Rigidbody rb;
    public float explosionForce;
    public float timeTillDeath;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 force = new Vector3(explosionForce, explosionForce, explosionForce);
        //rb.AddRelativeForce(force, ForceMode.Impulse);
        StartCoroutine(DestroySelf());
	}
	
	void FixedUpdate ()
    {

	}

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(timeTillDeath);
        Destroy(gameObject);
    }
}

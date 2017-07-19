using UnityEngine;
using System.Collections;

public class AirPower : MonoBehaviour
{
    public float upForce;
    Rigidbody rb;
    public float timeTillDeath;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
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

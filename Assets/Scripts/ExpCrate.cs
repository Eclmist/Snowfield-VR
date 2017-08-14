using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpCrate : MonoBehaviour
{
    [ReadOnly]
    private const float cutOff = 0.25f;
    [SerializeField]
    private float detectionDistance;
    [SerializeField]
    private float flySpeed;
    [SerializeField]
    private ParticleSystem particleEffect;

    private int crateValue;
    private bool activated = false;
	// Use this for initialization

	
	// Update is called once per frame
	void Update ()
    {

        transform.Rotate(Vector3.up,1,Space.World);

        if (!activated && Vector3.Distance(transform.position, Player.Instance.transform.position) < detectionDistance)
        {

            var temp = particleEffect.emission;
            temp.enabled = false;

            StartCoroutine(FlyToPlayer());
            activated = true;
        }
            
	}

    public void SetAmountToDrop(int amount)
    {
        crateValue = amount;

    }


    private IEnumerator FlyToPlayer()
    {

        while (true)
        {
            if(Vector3.Distance(transform.position,Player.Instance.transform.position) < cutOff)
            {
                ApplyCrateAmount();
                Destroy(gameObject);
            }
            transform.position = Vector3.Lerp(transform.position, Player.Instance.transform.position,  Time.deltaTime * flySpeed);
            yield return new WaitForEndOfFrame();

        }

    }

    private void ApplyCrateAmount()
    {
        Player.Instance.EXPCrates += crateValue;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,detectionDistance);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorceryCast : MonoBehaviour {

    public static SorceryCast Instance;

    private bool canCast = false;
    private bool casted = false;

    private float chargeTimer = 0.0f;

    private ParticleSystem chargeGO;
    private ParticleSystem castGO;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (canCast)
        {
            chargeTimer += Time.deltaTime;

            if (chargeTimer >= 10)
            {
                canCast = false;
            }
        }

        if (casted)
        {
            var em = chargeGO.emission;
            em.enabled = false;
            Destroy(chargeGO, 5);
        }
    }

    public void Charge(Transform flag)
    {
        //Skills.Instance.charge.transform.position = flag.position;
        Skills.Instance.charge.transform.position = Vector3.zero;
        chargeGO = Instantiate(Skills.Instance.charge, flag); //Set parent later

        var em = Skills.Instance.charge.emission;
        em.enabled = true;

        canCast = true;
    }

    public void Cast(Transform flag)
    {
        if (canCast)
        {
            //Skills.Instance.cast.transform.up = flag.transform.up;
            Skills.Instance.cast.transform.position = Vector3.zero;
            castGO = Instantiate(Skills.Instance.cast, flag);

            var em = Skills.Instance.cast.emission;
            em.enabled = true;

            casted = true;
            canCast = false;

            Destroy(castGO, 5);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spell : VR_Interactable
{
    [SerializeField]
    protected GameObject spellPrefab;
    [SerializeField]
    protected GameObject indicator;

    protected bool isHolding = false;
    protected bool isReleased = false;
    protected GameObject spellGO;

    public GameObject SpellPrefab
    {
        get { return this.spellPrefab; }
    }

    public GameObject Indicator
    {
        get { return this.indicator; }
    }

    protected virtual void Cast()
    {
        spellGO = Instantiate(spellPrefab, currentInteractingController.transform);
    }

    public override void OnInteracting(VR_Controller_Custom controller)
    {
        Debug.Log("hitttt");
        transform.position = LinkedController.transform.position;
        transform.rotation = LinkedController.transform.rotation;
    }

    public override void OnTriggerPress(VR_Controller_Custom controller)
    {
        Cast();
    }   

    public override void OnTriggerHold(VR_Controller_Custom controller)
    {
        var em = spellGO.GetComponent<ParticleSystem>().emission;
        em.enabled = true;
    }

    public override void OnTriggerRelease(VR_Controller_Custom controller)
    {
        var em = spellGO.GetComponent<ParticleSystem>().emission;
        em.enabled = false;
    }

    protected void Update()
    {
        //if (Input.Get(KeyCode.C))
        //{
        //    LongCast();
        //}

        //if (Input.GetKeyUp(KeyCode.C))
        //{
        //    isReleased = true;
        //    LongCast();
        //}
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spell : VR_Interactable
{
    [SerializeField]
    protected GameObject spellPrefab;
    [SerializeField]
    protected GameObject indicator;

    protected GameObject spellGO;
    //protected bool isCasted;

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
        //if (!isCasted)
        //{
        //    var em = indicator.GetComponent<ParticleSystem>().emission;
        //    em.enabled = false;

        //    spellGO = Instantiate(spellPrefab, currentInteractingController.transform);

        //    isCasted = true;
        //}
        //else
        //{
        //    var em = spellGO.GetComponent<ParticleSystem>().emission;
            
        //    var emsmoke = spellGO.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().emission;

        //    em.enabled = false;
        //    emsmoke.enabled = false;

        //    Destroy(spellGO, 3);
        //    Destroy(indicator, 3);
        //}
    }

    protected virtual void Hold()
    {
        Debug.Log("Holding");
    }

    protected virtual void Release()
    {

    }

	public virtual void OnUpdateInteraction(VR_Controller_Custom controller)
    {
        transform.position = LinkedController.transform.position;
        transform.rotation = LinkedController.transform.rotation;
    }

	protected override void OnTriggerPress()
    {
        Cast();
    }

   
	protected override void OnTriggerHold()
    {
       
        Hold();
    }

	protected override void OnTriggerRelease()
    {
        Release();
    }
}

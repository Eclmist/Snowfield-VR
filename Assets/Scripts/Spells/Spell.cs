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

    protected ParticleSystem particleRef;
    //protected bool isCasted;

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.A))
        {
            Cast();
        }
    }

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

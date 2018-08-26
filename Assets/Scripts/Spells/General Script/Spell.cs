using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Spell : MonoBehaviour
{
    //[SerializeField]
    //protected GameObject spellPrefab;
    //[SerializeField]
    //protected GameObject indicator;

    //protected GameObject spellGO;

    //public GameObject SpellPrefab
    //{
    //    get { return this.spellPrefab; }
    //}

    //public GameObject Indicator
    //{
    //    get { return this.indicator; }
    //}


    //public override void OnUpdateInteraction(VR_Controller_Custom controller)
    //   {
    //	base.OnUpdateInteraction(controller);
    //       transform.position = LinkedController.transform.position;
    //       transform.rotation = LinkedController.transform.rotation;
    //   }

    //protected override void OnTriggerPress()
    //   {
    //       base.OnTriggerPress();
    //   }


    //protected override void OnTriggerHold()
    //   {
    //	base.OnTriggerHold();
    //}

    //protected override void OnTriggerRelease()
    //   {
    //       base.OnTriggerRelease();
    //   }

    protected SpellHandler handler;

    [SerializeField]
    protected float manaCost, _DPS;

	public float DPS
	{
		get
		{
			return _DPS;
		}
	}
    public virtual void InitializeSpell(SpellHandler _handler)
    {
        handler = _handler;
    }

    public virtual void Update()
    {
        if (handler.LinkedController.Device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            OnTriggerPress();
        }
        if (handler.LinkedController.Device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            OnTriggerHold();
        }
        if (handler.LinkedController.Device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            OnTriggerRelease();
        }
        if (handler.LinkedController.Device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            OnGripPress();
        }
    }

    public virtual void OnGripPress()
    {
        var emindicator = this.gameObject.GetComponent<ParticleSystem>().emission;
        emindicator.enabled = false;
        handler.DecastSpell();
        Destroy(this.gameObject, 0.5f);
    }

    public virtual void OnTriggerPress()
    {

    }

    public virtual void OnTriggerHold()
    {

    }

    public virtual void OnTriggerRelease()
    {

    }

}

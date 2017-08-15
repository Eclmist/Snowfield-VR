using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerSpell : Spell
{
    protected bool hasFlag = false;
    protected bool casted = false;

    protected GameObject flamethrower;
    protected GameObject smoke;

    protected void Start()
    {
        flamethrower = transform.GetChild(0).gameObject;
        smoke = transform.GetChild(0).GetChild(0).gameObject;
    }

    public override void InitializeSpell(SpellHandler handler)
    {
        base.InitializeSpell(handler);
		
    }

    public override void Update()
    {
        base.Update();
		transform.position = handler.transform.position;
		transform.rotation = handler.transform.rotation;
		if (casted)
        {
            handler.Castor.StatContainer.ReduceMana(manaCost * Time.deltaTime);
        }
    }

    public override void OnTriggerHold()
    {
		Debug.Log(handler.Castor.StatContainer);
        if (!casted && (handler.Castor.StatContainer.GetStat(Stats.StatsType.MANA).Current >= 1))
        {
            flamethrower.SetActive(true);

            var em = flamethrower.GetComponent<ParticleSystem>().emission;
            var emsmoke = smoke.GetComponent<ParticleSystem>().emission;

            em.enabled = true;
            emsmoke.enabled = true;

            casted = true;
        }
    }

    public override void OnTriggerRelease()
    {
        if (casted)
        {
            var em = flamethrower.GetComponent<ParticleSystem>().emission;

            var emsmoke = smoke.GetComponent<ParticleSystem>().emission;

            var emindicator = this.gameObject.GetComponent<ParticleSystem>().emission;

            emindicator.enabled = false;
            em.enabled = false;
            emsmoke.enabled = false;

            Destroy(this, 0.5f);

            casted = false;

            handler.DecastSpell();
        }
    }
}
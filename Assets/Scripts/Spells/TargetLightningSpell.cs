using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLightningSpell : Spell
{

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    protected RFX4_ParticleTrail trail1;

    [SerializeField]
    protected RFX4_ParticleTrail trail2;

    [SerializeField]
    protected RFX4_ParticleTrail trail3;

    [SerializeField]
    protected float DPS;

    protected float seconds;

    public override void InitializeSpell(SpellHandler _handler)
    {
        base.InitializeSpell(_handler);

        if (handler.Castor.StatContainer.GetStat(Stats.StatsType.MANA).Current >= manaCost)
        {
            Vector3 playerFor = Player.Instance.transform.forward;
            playerFor.y = 0;
            playerFor.Normalize();

            playerFor = Player.Instance.transform.position + playerFor * offset.z;

            RaycastHit hit;
            Physics.Raycast(playerFor, Vector3.down, out hit);

            transform.position = hit.point;

            float yRot = Player.Instance.transform.rotation.eulerAngles.y;
            var rot = transform.rotation;

            rot.eulerAngles = new Vector3(transform.rotation.x, yRot, transform.rotation.z);
            transform.rotation = rot;

            this.gameObject.SetActive(true);
            handler.Castor.StatContainer.ReduceMana(manaCost);

            handler.DecastSpell();
        }
        else
        {
            //Not Enough mana
            handler.DecastSpell();
            Destroy(this.gameObject, 0.1f);
        }
    }

    public override void Update()
    {
        base.Update();

        seconds += Time.deltaTime;

        if (seconds >= 0.5f)
        {
            if (trail1.Target != null)
            {
                Player.Instance.CastSpell(DPS / 2, trail1.Target.GetComponent<Monster>());
            }

            if (trail2.Target != null)
            {
                Player.Instance.CastSpell(DPS / 2, trail2.Target.GetComponent<Monster>());
            }

            if (trail3.Target != null)
            {
                Player.Instance.CastSpell(DPS / 2, trail3.Target.GetComponent<Monster>());
            }

            seconds = 0;
        }

    }
}

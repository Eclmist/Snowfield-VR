using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TossSpell : Spell
{
    [SerializeField]
    protected GameObject controller;

    public override void InitializeSpell(SpellHandler handler)
    {
        base.InitializeSpell(handler);

        if (handler.Castor.StatContainer.GetStat(Stats.StatsType.MANA).Current >= manaCost)
        {
            transform.position = controller.transform.position;

            float yRot = controller.transform.rotation.eulerAngles.y;
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
}

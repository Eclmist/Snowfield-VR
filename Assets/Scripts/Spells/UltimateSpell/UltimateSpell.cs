using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSpell : Spell{

    [SerializeField]
    private Vector3 offset;

    public override void InitializeSpell(SpellHandler handler)
    {
        base.InitializeSpell(handler);

        transform.position = Player.Instance.transform.position + Player.Instance.transform.forward * offset.z;
        float yRot = Player.Instance.transform.rotation.eulerAngles.y;
        var rot = transform.rotation;
        rot.eulerAngles = new Vector3(transform.rotation.x, yRot, transform.rotation.z);
        transform.rotation = rot;

        transform.parent = null;
    }

    protected void Start()
    {
        if (handler.Castor.StatContainer.GetStat(Stats.StatsType.MANA).Current >= manaCost)
        {
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

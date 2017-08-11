using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonCharge : Spell{

    [SerializeField]
    private Vector3 offset;

    protected void Start()
    {
        if (handler.Castor.StatContainer.GetStat(Stats.StatsType.MANA).Current >= manaCost)
        {
            this.gameObject.SetActive(true);

            transform.position = Player.Instance.transform.position + Player.Instance.transform.forward * offset.z;
            float yRot = Player.Instance.transform.rotation.eulerAngles.y;
            var rot = transform.rotation;
            rot.eulerAngles = new Vector3(transform.rotation.x, yRot, transform.rotation.z);
            transform.rotation = rot;

            handler.Castor.StatContainer.ReduceMana(manaCost);

            handler.DecastSpell();
        }
    }
}

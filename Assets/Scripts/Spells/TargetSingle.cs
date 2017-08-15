using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSingle : Spell {

    protected GameObject monster;
    protected float nearestDistance = 0;

    protected bool readyToLookAt;

    public override void InitializeSpell(SpellHandler handler)
    {
        base.InitializeSpell(handler);

        if (handler.Castor.StatContainer.GetStat(Stats.StatsType.MANA).Current >= manaCost)
        {
            Vector3 target = Player.Instance.transform.position + Player.Instance.transform.forward * 7;

            Collider[] col = Physics.OverlapSphere(target, 7);

            foreach (Collider c in col)
            {
                Monster mob = c.GetComponent<Monster>();

				if (mob)
				{
					float distance = Vector3.Distance(Player.Instance.transform.position, mob.gameObject.transform.position);

					if (distance <= nearestDistance || nearestDistance == 0)
					{
						monster = mob.gameObject;
						nearestDistance = distance;
					}
				}
            }

            if(monster != null && nearestDistance != 0)
            {
				readyToLookAt = true;
			}
			else
			{
				handler.DecastSpell();
				Destroy(this.gameObject, 0.1f);
			}

			if (readyToLookAt)
			{
				//Instantiate and rotate Rotate to monster
				transform.position = Player.Instance.transform.position + Player.Instance.transform.forward * 1;

				float yRot = Player.Instance.transform.rotation.eulerAngles.y;
				var rot = transform.rotation;

				rot.eulerAngles = new Vector3(transform.rotation.x, yRot, transform.rotation.z);
				transform.rotation = rot;

				this.gameObject.SetActive(true);
				handler.Castor.StatContainer.ReduceMana(manaCost);

				handler.DecastSpell();
			}
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
        if (readyToLookAt)
        {
            transform.LookAt(monster.transform);
        }
    }

}

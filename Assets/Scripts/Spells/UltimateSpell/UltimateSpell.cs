using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSpell : Spell{

    [SerializeField]
    private Vector3 offset;

	private float time;

    public override void InitializeSpell(SpellHandler handler)
    {
        base.InitializeSpell(handler);

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
}

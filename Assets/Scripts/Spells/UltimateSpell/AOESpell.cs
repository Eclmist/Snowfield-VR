using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AOESpell : MonoBehaviour
{
	[SerializeField]
	protected float damage;

	//Destroy Particle at DestroyTime given. (in seconds)
	[SerializeField]
	protected float destroyTime;

	//Enable the collider at colliderEnableTime given. (in seconds)
	[SerializeField]
	protected float colliderEnableTime;

	protected BoxCollider col;

	//Cooldown timer for collider to be enable
	protected float cooldown;

	//bool to check if collider is enabled
	protected bool colliderEnabled;

	// Use this for initialization
	void Start()
	{
		Destroy(this.gameObject, destroyTime);

		col = this.gameObject.GetComponent<BoxCollider>();

		transform.parent = null;
	}

	// Update is called once per frame
	void Update()
	{
		if (!colliderEnabled)
		{
			cooldown += Time.deltaTime;
			CheckCooldown();
		}
		else
		{
            CheckCooldown();

            if(cooldown >= 1)
            {
                col.enabled = false;
            }
		}
	}

	void CheckCooldown()
	{
		if (cooldown >= colliderEnableTime && col.enabled == false)
		{
			col.enabled = true;
			colliderEnabled = true;
            cooldown = 0;
        }
        else
        {
            cooldown += Time.deltaTime;
        }
	}

    private void OnTriggerStay(Collider other)
    {
        Monster mob = other.GetComponent<Monster>();

        Player.Instance.CastSpell(Player.Instance.StatContainer.GetStat(Stats.StatsType.MAGIC).Current * damage, mob);
    }
}

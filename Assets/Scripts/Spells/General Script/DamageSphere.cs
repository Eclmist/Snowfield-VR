using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSphere : MonoBehaviour {

    [SerializeField]
    protected float radius;

    [SerializeField]
    protected float damage;
	// Use this for initialization
	void Start () {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider c in hitColliders)
        {
            Monster mob = c.GetComponent<Monster>();

            Player.Instance.CastSpell(Player.Instance.StatContainer.GetStat(Stats.StatsType.MAGIC).Current * damage, mob);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, radius);
    }

}

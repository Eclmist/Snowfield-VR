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

        col.enabled = false;

        transform.parent = null;

		transform.position = new Vector3(Player.Instance.transform.position.x + 2, Player.Instance.transform.position.y, Player.Instance.transform.position.z);
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
            col.size.Set(col.size.x + (1 * Time.deltaTime), col.size.y, col.size.z + (1 * Time.deltaTime));
        }
    }

    void CheckCooldown()
    {
        if (cooldown >= colliderEnableTime)
        {
            col.enabled = true;
            colliderEnabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Monster mob = other.GetComponent<Monster>();

        if (mob)
            mob.TakeDamage(damage, Player.Instance);
    }
}

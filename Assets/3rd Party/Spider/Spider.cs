using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour{

    public float distanceToReact;
    private Animator anim;

    // Use this for initialization
    void Start () {

        anim = GetComponent<Animator>();

	}
    // Update is called once per frame
    void Update () {

        anim.SetBool("isAttacking", (Vector3.Distance(Player.Instance.transform.position, transform.position) < distanceToReact ));
 
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, distanceToReact);
    }
}

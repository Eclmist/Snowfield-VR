using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour {
    [SerializeField]
    private float speed = 10F;
    [SerializeField]
    protected Vector3 offset = Vector3.zero;
	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 localOffset = Vector3.zero;
        localOffset += Player.Instance.transform.right * offset.x;
        localOffset += Player.Instance.transform.up * offset.y;
        localOffset += Player.Instance.transform.forward * offset.z;

        Vector3 target = Player.Instance.transform.position;
        transform.LookAt(target);
        transform.position = Vector3.Lerp(transform.position, target + localOffset, Time.deltaTime * speed);

	}
}

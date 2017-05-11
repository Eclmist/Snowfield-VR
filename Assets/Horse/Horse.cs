using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : MonoBehaviour {

    [SerializeField]
    protected float speed;

	void Update () {
        transform.position += Vector3.forward * Time.deltaTime * speed;
	}
}

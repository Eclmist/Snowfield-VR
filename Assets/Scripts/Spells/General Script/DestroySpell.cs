using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySpell : MonoBehaviour {

    [SerializeField]
    protected float destroyTime;

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, destroyTime);
	}
}

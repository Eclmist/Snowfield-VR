using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSpell : MonoBehaviour {

    [SerializeField]
    protected float rotateSpeed;

	void Update () {
        transform.Rotate(0, 0, Time.deltaTime * rotateSpeed);
    }
}

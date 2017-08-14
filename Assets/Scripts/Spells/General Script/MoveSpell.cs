using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpell : MonoBehaviour {

    [SerializeField]
    protected float movementSpeed;

    // Update is called once per frame
    void Update () {
        transform.position += transform.forward * Time.deltaTime * movementSpeed;
    }
}

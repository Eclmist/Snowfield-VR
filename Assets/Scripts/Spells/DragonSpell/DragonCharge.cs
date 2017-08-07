using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonCharge : Spell {

    [SerializeField]
    protected float zOffset;

    public override void OnUpdateInteraction(VR_Controller_Custom controller)
    {
        transform.position = new Vector3(LinkedController.transform.position.x, LinkedController.transform.position.y, LinkedController.transform.position.z + zOffset);
        transform.rotation = LinkedController.transform.rotation;
    }
}

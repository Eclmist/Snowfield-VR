using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonCharge : Spell {

    [SerializeField]
    private Vector3 offset;

	public override void OnUpdateInteraction(VR_Controller_Custom controller)
	{
		//transform.position = new Vector3(LinkedController.transform.position.x, LinkedController.transform.position.y, LinkedController.transform.position.z + zOffset);
		//transform.rotation = LinkedController.transform.rotation;

		
	}


	protected override void Start()
	{
		transform.position = Player.Instance.transform.position + transform.forward * offset.z;
		float yRot = Player.Instance.transform.rotation.eulerAngles.y;
		var rot = transform.rotation;
		rot.eulerAngles = new Vector3(transform.rotation.x,yRot,transform.rotation.z);
		transform.rotation = rot;
	}




}

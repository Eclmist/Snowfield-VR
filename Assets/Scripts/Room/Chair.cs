using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Opening_Room
{
	[RequireComponent(typeof(Rigidbody))]
	public class Chair : VR_Interactable_Object
	{
		[SerializeField] private AudioSource rollingSound;

		private Vector3 positionalOffset;

		private float maxVol;
		private float maxPitch;

		protected override void Start()
		{
			base.Start();

			maxVol = rollingSound.volume;
			maxPitch = rollingSound.pitch;

		}

		protected void Update()
		{
			Vector3 v = rigidBody.velocity;
			v.y = 0;

			rollingSound.pitch = Mathf.Lerp(0.7F, maxPitch, v.magnitude);
			rollingSound.volume = Mathf.Lerp(0, maxVol, v.magnitude);

			v.Scale(new Vector3(0.01F, 0.01F, 0.01F));

			if (v.magnitude > 0)
			{
				if (currentInteractingController)
				{
					currentInteractingController.Vibrate(Mathf.Min(v.magnitude, 0.01F));
				}
			}

		}

		public override void OnTriggerPress(VR_Controller_Custom ctrl)
		{
			base.OnTriggerPress(ctrl);
			positionalOffset = transform.position - ctrl.transform.position;
		}

		public override void OnFixedUpdateInteraction(VR_Controller_Custom referenceCheck)
		{
			base.OnFixedUpdateInteraction();

			Vector3 PositionDelta = referenceCheck.transform.position - transform.position + positionalOffset;
			PositionDelta.y = 0;

			Vector3 targetPoint = targetPositionPoint.transform.position;
			targetPoint.y = transform.position.y;


			float currentForce = 10;

			rigidBody.velocity =
				PositionDelta.magnitude * 5 > currentForce ?
				(PositionDelta).normalized * currentForce : PositionDelta * 5;


				//// Rotation ----------------------------------------------

				////Quaternion targetRotation = referenceCheck.transform.rotation * rotationOffset;

				//Quaternion RotationDelta = targetPositionPoint.transform.rotation * Quaternion.Inverse(this.transform.rotation);
				//RotationDelta.eulerAngles = new Vector3(0, 1, 0);
				//float angle;
				//Vector3 axis;
				//RotationDelta.ToAngleAxis(out angle, out axis);

				//if (angle > 180)
				//	angle -= 360;

				//float angularVelocityNumber = .2f;

				//// -------------------------------------------------------
				//rigidBody.angularVelocity = axis * angle * angularVelocityNumber;

			
		}
	}
}
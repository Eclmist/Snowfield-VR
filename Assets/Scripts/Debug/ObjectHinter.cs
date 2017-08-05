using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ObjectHinter : MonoBehaviour
{
	public static bool hinterEnabled = true;
	private Camera cam;

	private List<VR_Interactable_Object> hintedObj;

	protected void Start ()
	{
		hintedObj = new List<VR_Interactable_Object>();
		cam = GetComponent<Camera>();
	}
	
	protected void Update ()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			hinterEnabled = !hinterEnabled;

			if (!hinterEnabled)
			{
				foreach (VR_Interactable_Object obj in hintedObj)
				{
					obj.StopHint();
				}

				hintedObj.Clear();
			}
		}

		if (hinterEnabled)
		{
			if (Input.GetMouseButtonDown(0))
			{
				Ray mouseRay = cam.ScreenPointToRay(Input.mousePosition);

				RaycastHit hit;

				if (Physics.Raycast(mouseRay, out hit, LayerMask.GetMask("Interactable")))
				{
					VR_Interactable_Object obj = hit.collider.GetComponent<VR_Interactable_Object>();
					if (obj)
					{
						obj.HintObject();
						hintedObj.Add(obj);
					}
				}
			}
		}
	}
}

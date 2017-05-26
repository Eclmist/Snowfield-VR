using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgedBlade : MonoBehaviour
{

	[SerializeField] private GameObject modelObject;

	[SerializeField] [Range(0,1)] [Tooltip("At what angle should impact direction snap to axis? 0 = always snap, 1 = exactly parallel")]
	private float dotThreshold = 0.3F;

	[SerializeField] private float squeezeAmount = 0.1F;
	[SerializeField] private float sideExtensionAmount = 0.05F;
	[SerializeField] private float forwardExtensionAmount = 0.1F;

	[SerializeField] private float minThickness = 0.2F;
	[SerializeField] private float minThicknessSide = 0.4F;

	[Header("Vertex Manipulation")]
	[SerializeField] private float distanceThreshold = 0.05F;
	[SerializeField] private float randomJitter = 0.01F;
	[SerializeField] private float deformationAmount = 0.01F;

	private MorphVertex vertexMorphScript;

	private MeshFilter modelFilter;
	private MeshCollider meshCollider;
	private Mesh currentMesh;
	protected void Start()
	{
		if (modelObject == null)
		{
			if (transform.childCount == 0)
			{
				Destroy(this);
			}
			else
			{
				modelObject = transform.GetChild(0).gameObject;
			}
		}

		modelFilter = modelObject.GetComponent<MeshFilter>();
		meshCollider = modelObject.GetComponent<MeshCollider>();

		vertexMorphScript = GetComponentInChildren<MorphVertex>();

		ResetTransform();
	}

	public bool Impact(Vector3 direction)
	{
		Vector3 localScale = transform.localScale;
		bool hit = false;
		//Impact came from top
		if (Mathf.Abs(Vector3.Dot(direction, transform.up)) > dotThreshold)
		{
			if (vertexMorphScript != null)
			{
				vertexMorphScript.Morph();
			}

			if (localScale.y > minThickness)
			{
				localScale.y -= squeezeAmount;
				localScale.x += sideExtensionAmount;
				localScale.z += forwardExtensionAmount;
				hit = true;
			}

		}
		else if (Mathf.Abs(Vector3.Dot(direction, transform.right)) > dotThreshold)
		{
			if (vertexMorphScript != null)
			{
				vertexMorphScript.Morph();
			}

			if (localScale.x > minThicknessSide)
			{
				localScale.x -= squeezeAmount;
				localScale.y += sideExtensionAmount;
				localScale.z += forwardExtensionAmount;
				hit = true;
			}

		}

		transform.localScale = localScale;

		return hit;
	}

	public void Impact(Vector3 direction, Vector3 position)
	{
		if (Impact(direction))
		{
			currentMesh = modelFilter.mesh;

			Vector3[] verts = currentMesh.vertices;

			for (int i = 0; i < verts.Length; i++)
			{
				Vector3 currentVert = modelObject.transform.TransformPoint(verts[i]);

				if (Vector3.Distance(position, currentVert) < distanceThreshold)
				{
					Vector3 dirToImpactPoint = currentVert - position;
					float distanceToImpact = dirToImpactPoint.magnitude;
					verts[i] += dirToImpactPoint.normalized * deformationAmount * (1 - (distanceToImpact / distanceThreshold));

					verts[i] += Random.insideUnitSphere * randomJitter;
				}

				currentMesh.vertices = verts;
			}


			currentMesh.RecalculateBounds();
			modelFilter.mesh = currentMesh;
		}
	}

	private void ResetTransform(bool useParentTransform = false)
	{

		modelObject.transform.parent = null;

		currentMesh = modelFilter.mesh;

		Vector3[] verts = currentMesh.vertices;

		for (int i = 0; i < verts.Length; i++)
		{
			Vector3 realWorldSpaceTransform = modelObject.transform.TransformVector(verts[i]);
			verts[i] = realWorldSpaceTransform;
		}
		currentMesh.vertices = verts;

		currentMesh.RecalculateBounds();
		modelFilter.mesh = currentMesh;

		//if (useParentTransform)
		//{
		transform.localScale = new Vector3(1, 1, 1);
		transform.localRotation = Quaternion.identity;
		//}
		//else
		//{
		//	//modelObject.transform.localPosition = Vector3.zero;

		modelObject.transform.localScale = new Vector3(1, 1, 1);
		modelObject.transform.localRotation = Quaternion.identity;
		//}

		modelObject.transform.parent = transform;

		ResetCollider();
	}

	private void ResetCollider()
	{
		meshCollider.convex = true;
		meshCollider.sharedMesh = currentMesh;
	}

	//Debug
	public void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray screenToWord = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(screenToWord, out hit))
			{
				Impact(screenToWord.direction, hit.point);
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			Hammer hammer = contact.otherCollider.GetComponent<Hammer>();

			if (hammer != null)
			{
				Impact(-collision.relativeVelocity, contact.point);
			}
		}

	}

}

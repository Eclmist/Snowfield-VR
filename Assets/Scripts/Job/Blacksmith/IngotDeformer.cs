using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ingot))]
public class IngotDeformer : MonoBehaviour
{

	[SerializeField] private bool enableScaling = false;

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

	private MeshFilter modelFilter;
	private Mesh currentMesh;
	private MeshCollider collider;

	protected void Start()
	{
		modelFilter = GetComponent<MeshFilter>();
		collider = GetComponent<MeshCollider>();
	}


	public void Impact(Vector3 direction, Vector3 position)
	{
		currentMesh = modelFilter.mesh;

		Vector3[] verts = currentMesh.vertices;

		for (int i = 0; i < verts.Length; i++)
		{
			Vector3 currentVert = transform.TransformPoint(verts[i]);

			float distanceToImpact = Vector3.Distance(position, currentVert);

			if (distanceToImpact < distanceThreshold)
			{
				verts[i] += direction.normalized * deformationAmount * (1 - (distanceToImpact / distanceThreshold));

				verts[i] += Random.insideUnitSphere * randomJitter;
			}

			currentMesh.vertices = verts;
		}


		currentMesh.RecalculateBounds();
		currentMesh.RecalculateNormals();
		currentMesh.RecalculateTangents();
		modelFilter.mesh = currentMesh;

		collider.sharedMesh = currentMesh;
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
				if (hit.collider.gameObject == gameObject)
				{
					Impact(screenToWord.direction, hit.point);
					Debug.Log("MouseClick");
				}
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cardinal_Tiles : MonoBehaviour
{


	[SerializeField] private Transform followTarget;
	[SerializeField] private GameObject[] tiles;
	[SerializeField] private float[] randomOffset;

	[SerializeField] [Range(0, 2)] private float heightMultiplier = 0.5F;
	[SerializeField] [Range(0, 10)] private float radius = 3;
	[SerializeField] [Range(0, 10)] private float speed = 2;

	private Vector3[] startingPosition;


	// Use this for initialization
	protected void Start ()
	{
		if (tiles.Length == 0)
			Destroy(this);

		startingPosition = new Vector3[tiles.Length];

		for (int i = 0; i < tiles.Length; i++)
		{
			startingPosition[i] = tiles[i].transform.position;

			tiles[i].GetComponent<Renderer>().material.SetFloat("_RandomAmount", Random.Range(-5, 5));
		}
	}

	// Update is called once per frame
	protected void Update ()
	{
		Vector3 center = followTarget.position;
		center.y = 0;

		for (int i = 0; i < tiles.Length; i++)
		{
			float distance = ((startingPosition[i] - center).magnitude);
			if (distance < radius) distance = 0;
				
			Vector3 targetPosition = new Vector3(startingPosition[i].x,
				startingPosition[i].y - distance * heightMultiplier,
				startingPosition[i].z);

			tiles[i].transform.position = Vector3.Slerp(tiles[i].transform.position, targetPosition, Time.deltaTime * speed);

		}
	}
}

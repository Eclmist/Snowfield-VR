using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ReflectionProbe))]
public class ReflectionProbeRefresh : MonoBehaviour
{

	ReflectionProbe[] probes;

	float timeSinceLastRefresh = 0;
	[SerializeField] private float refreshEveryNSeconds = 10;

	// Use this for initialization
	void Start ()
	{
		probes = GetComponents<ReflectionProbe>();

		foreach (ReflectionProbe p in probes)
		{
			p.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.ViaScripting;
		}

	}

	// Update is called once per frame
	void Update ()
	{
		if (timeSinceLastRefresh > refreshEveryNSeconds)
		{
			timeSinceLastRefresh = 0;

			foreach (ReflectionProbe p in probes)
			{
				p.RenderProbe();
			}
		}
	}
}

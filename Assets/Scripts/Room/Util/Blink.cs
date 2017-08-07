using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Opening_Room
{

	[RequireComponent(typeof(Renderer))]
	public class Blink : MonoBehaviour
	{
		[SerializeField] [Range(0,1)] private float blinkSpeed;

		private Renderer ren;

		protected void Start()
		{
			ren = GetComponent<Renderer>();
			StartCoroutine(BlinkCoroutine());
		}

		private IEnumerator BlinkCoroutine()
		{
			while (true)
			{
				ren.enabled = !ren.enabled;

				yield return new WaitForSeconds(1.05F - blinkSpeed);
			}
		}
	}
}
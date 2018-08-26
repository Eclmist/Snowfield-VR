using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public class WindowLightGroup
//{
//	public Renderer window;
//	public Light light;
//}

public class LightManager : MonoBehaviour
{

	[SerializeField] private Renderer[] windows;
	[SerializeField][Range(0,1)] private float changeRate = 0.5F;


	private GameClock clock;

	private List<Renderer> shuffleBag = new List<Renderer>();

	protected void Start ()
	{
		if (!GameManager.Instance)
			Destroy(this);

		clock = GameManager.Instance.GameClock;
		
	}

	private bool wasDayOnPreviousFrame;

	protected void Update ()
	{
		if (clock.IsDay)
		{
			if (!wasDayOnPreviousFrame)
			{
				wasDayOnPreviousFrame = true;
				OnDayChange();
			}

			ChangeLight(false);

		}
		else
		{
			if (wasDayOnPreviousFrame)
			{
				wasDayOnPreviousFrame = false;
				OnDayChange();
			}

			ChangeLight(true);

		}
	}

	private void OnDayChange()
	{
		shuffleBag = new List<Renderer>(windows);
	}

	private void ChangeLight(bool active)
	{
		if (shuffleBag.Count > 0)
		{
			if (Random.Range(0, 10) <= changeRate)
			{
				int index = Random.Range(0, shuffleBag.Count);
				Renderer current = shuffleBag[index];
				shuffleBag.RemoveAt(index);

				current.enabled = false;
			}
		}
	}
}

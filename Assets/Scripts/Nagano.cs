using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

[System.Serializable]
public struct ReferenceEventObjects
{
	public GameObject furnace;
}

[System.Serializable]
public class LevelEvent
{
    public UnityEvent invokableEventUpdate;
    [Range(0, 100)] public float startDelay;
    public bool enabled = true;
}


public class Nagano : MonoBehaviour
{


	[SerializeField] private ReferenceEventObjects referenceEventObjects;
	[SerializeField] private AudioClip notificationSound;
	private Valve.VR.InteractionSystem.Player player;
	private static bool currentEventCompleted = false;
	[SerializeField] private List<LevelEvent> events;
	private LevelEvent currentEvent;

	private float timer = 0;

	// Use this for initialization
	private void Start()
	{
		player = Valve.VR.InteractionSystem.Player.instance;
		currentEvent = null;
	}

	// Update is called once per frame
	private void Update()
	{
		timer += Time.deltaTime;

		while (currentEvent == null)
		{
			if (events.Count <= 0) //No more events, can kill level blueprint already
			{
				enabled = false;
				return;
			}

			currentEvent = events[0];
			events.RemoveAt(0);

			if (!currentEvent.enabled)
				currentEvent = null;
		}

		if (currentEvent.startDelay < timer)
		{

			currentEvent.invokableEventUpdate.Invoke();

			if (currentEventCompleted)
			{
				timer = 0;
				currentEvent = null;
				currentEventCompleted = false;
			}
		}
	}

	public static void CompleteCurrentEvent()
	{
		currentEventCompleted = true;
	}

	private void ShowControllerHints(Valve.VR.EVRButtonId btn, string text, bool glowbtn = true)
	{
		foreach (Hand hand in player.hands)
		{
			ControllerButtonHints.ShowTextHint(hand, btn, text, glowbtn);
			AudioSource.PlayClipAtPoint(notificationSound, hand.transform.position);
		}
	}

	private void HideAllControllerHints()
	{
		foreach (Hand hand in player.hands)
		{
			ControllerButtonHints.HideAllTextHints(hand);
		}
	}


	//----------- Level Events -----------------------------------------------------------------------------------------------------------//


	#region Blacksmith Tutorial Sequence

	private bool blacksmithCoroutineRunning = false;
	private bool enteredBlacksmithArea = false;

	public void EnterBlackSmithArea()
	{
		enteredBlacksmithArea = true;
	}

	public void BlacksmithTutorialEvent()
	{

		if (!blacksmithCoroutineRunning)
		{
			blacksmithCoroutineRunning = true;
			StartCoroutine(HeadToBlackSmithAreaRoutine());
		}

	}

	// The starting of the blacksmith tutorial sequence
	private IEnumerator HeadToBlackSmithAreaRoutine()
	{
		ShowControllerHints(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad, "Head next door");

		while (!enteredBlacksmithArea)
		{

			yield return null;
		}

		HideAllControllerHints();
		StartCoroutine(PickUpIngotRoutine());
	}


	private IEnumerator PickUpIngotRoutine()
	{
		blacksmithCoroutineRunning = true;
		ShowControllerHints(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger, "Grab the ingot");

		while (Ingot.pickedUpIngot == false)
		{

			yield return null;
		}

		HideAllControllerHints();
		StartCoroutine(HeatUpIngotRoutine());


	}

	private IEnumerator HeatUpIngotRoutine()
	{
		ShowControllerHints(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger, "Place ingot in furnace");

		while (!Ingot.isHotEnough)
		{

			yield return null;
		}

		HideAllControllerHints();
		StartCoroutine(PlaceOnAnvilRoutine());

	}


	private IEnumerator PlaceOnAnvilRoutine()
	{
		ShowControllerHints(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger, "Place ingot on anvil");

		while (!Ingot.isPlacedOnAnvil)
		{

			yield return null;
		}

		HideAllControllerHints();
		StartCoroutine(HammerIngotRoutine());
	}

	private IEnumerator HammerIngotRoutine()
	{

		ShowControllerHints(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger, "Hammer it");
		while (!FakeItem.isForming)
		{

			yield return null;
		}


		MessageManager.Instance.SendMail("Blacksmithing", "Woah! I heard that you can make really cool weapons. I'll pay you a visit!\n\nFrom:\n???", null);

		HideAllControllerHints();
		StartCoroutine(ReadMailRoutine());

	}

	private IEnumerator ReadMailRoutine()
	{
		ShowControllerHints(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu, "Check your mail");

		yield return new WaitForSeconds(6f);

		HideAllControllerHints();
		CompleteCurrentEvent();
	}


	#endregion

	#region Selling Tutorial Sequence

	private bool sellingRoutineStarted = false;
	private int startingNumberOfOrders;

	public void SellingTutorialEvent()
	{
		if(OrderBoard.Instance.CurrentNumberOfOrders > 0 && !sellingRoutineStarted)
		{
			sellingRoutineStarted = true;
			startingNumberOfOrders = OrderBoard.Instance.CurrentNumberOfOrders;
			StartCoroutine(SellingRoutine());
		}
	}

	private IEnumerator SellingRoutine()
	{
		bool soldFirstOrder = false;
		ShowControllerHints(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger, "touch order slip with sword");

		while (!soldFirstOrder)
		{
			if (OrderBoard.Instance.CurrentNumberOfOrders <= 0)
				soldFirstOrder = true;

			yield return null;
		}

		HideAllControllerHints();
		MessageManager.Instance.SendMail("Thank You!","That's a fine looking sword! I shall recommend it to all my friends.\n\nFrom:\n???",null);
		CompleteCurrentEvent();
	}



	#endregion

	#region Post-tutorial Sequence




	#endregion







}




using System;
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
    [SerializeField][ReadOnly]private int eventIndex;
	
    public int EventIndex
    {
        get { return this.eventIndex; }
        set { this.eventIndex = value; }
    }
}


public class Nagano : MonoBehaviour,ICanSerialize
{

    
	[SerializeField] private ReferenceEventObjects referenceEventObjects;
	[SerializeField] private AudioClip notificationSound;
	private Valve.VR.InteractionSystem.Player player;
	private static bool currentEventCompleted = false;
	[SerializeField] private List<LevelEvent> events;
	[Header("CURRENT RUNNING EVENT")][SerializeField]private LevelEvent currentEvent;
    private int previousIndex = -1;

	private float timer = 0;

    #region ISerializable
    public void Save()
    {
        SerializeManager.Save(SerializedFileName, currentEvent.EventIndex);
    }
    #endregion


    private void Awake()
	{

        if(events.Count > 0)
        {
            for (int i = 0; i < events.Count; i++)
                events[i].EventIndex = i;
        }

        object o = SerializeManager.Load(SerializedFileName);

        if(o!=null)
            previousIndex = (int)o;


		if (enabled)
			AIManager.canSpawnAI = false;

        
	}

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
			if (events.Count <= 0)
			{
				enabled = false;
				return;
			}

			currentEvent = events[0];
			events.RemoveAt(0);
            SaveManager.Instance.Save();

            if (!currentEvent.enabled || currentEvent.EventIndex < previousIndex)
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
			AudioSource.PlayClipAtPoint(notificationSound, hand.transform.position, 0.1F);
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

    #region Welcome event

    private bool openMailCoroutineRunning = false;

	public void WelcomeEvent()
	{
		if(GameManager.firstGame)
		{
            if(!openMailCoroutineRunning)
            {
              
				Teleport.instance.CancelTeleportHint();
                openMailCoroutineRunning = true;
				StartCoroutine(OpenMailRoutine());
            }
            
		}
			
	}

    private IEnumerator OpenMailRoutine()
    {

		MessageManager.Instance.SendMail("Welcome", "You have been assigned the role of a merchant in this wonderful world. Here you can craft weapons and sell them to players. Have fun!\n\nFrom:\n???", null);
		ShowControllerHints(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu, "Open inbox");

		while (!MessageManager.Instance.knowHowToUseMail)
        {
				
			yield return null;
        }

        HideAllControllerHints();
        CompleteCurrentEvent();
		Debug.Log(previousIndex);
    }


	#endregion

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

		while (enteredBlacksmithArea == false)
		{

			yield return null;
		}

		HideAllControllerHints();
		StartCoroutine(PickUpIngotRoutine());
	}


	private IEnumerator PickUpIngotRoutine()
	{
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


		MessageManager.Instance.SendMail("Blacksmithing", "Woah! I heard that you can make really cool weapons. I'll pay you a visit!\n\nFrom:\nXiaotian", null);
        AIManager.Instance.InstantiateNewAdventurerAI(AIManager.Instance.CreateNewAdventurerAI(Color.white, "Xiaotian", 1.2f));
        HideAllControllerHints();
		StartCoroutine(ReadMailRoutine());

	}

	private IEnumerator ReadMailRoutine()
	{
		ShowControllerHints(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu, "Check your mail");

		yield return new WaitForSeconds(4f);

		HideAllControllerHints();
		CompleteCurrentEvent();
	}


	#endregion

	#region Selling Tutorial Sequence

	private bool sellingRoutineStarted = false;
	private int startingNumberOfOrders;

    public string SerializedFileName
    {
        get
        {
            return "NaganoCurrentEvent";
        }
    }

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
		MessageManager.Instance.SendMail("Thank You!","That's a fine looking sword! I shall recommend it to all my friends.\n\nFrom:\nXiaotian",null);
        AIManager.Instance.StartSpawningAllAdventurerAIs();
        CompleteCurrentEvent();
	}



	#endregion

	#region Post-tutorial Sequence

	public void TownUnderAttackEvent()
	{
		if (GameManager.nightApproaching && GameManager.Instance.GameClock.Day == 1)
		{
			MessageManager.Instance.SendMail("Defend The Town", "All of our guards got banned from hacking and we are really short-handed right now. If those monsters get to the heart of the town, somebody's gotta pay for damages. I'm just saying...\n\nFrom:\nMayor", null);

			CompleteCurrentEvent();
		}
			
	}


	#endregion







}




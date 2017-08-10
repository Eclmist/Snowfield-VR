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

    [SerializeField] private AudioClip notificationSound;
    private Valve.VR.InteractionSystem.Player player;
    private static bool currentEventCompleted = false;
    [SerializeField] private List<LevelEvent> events;
    public  LevelEvent currentEvent;

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
            StartCoroutine(PickUpIngotRoutine());
    }

    // The starting of the blacksmith tutorial sequence
    private IEnumerator HeadToBlackSmithAreaRoutine()
    {
        while (!enteredBlacksmithArea)
        {
            ShowControllerHints(Valve.VR.EVRButtonId.k_EButton_DPad_Up, "Head next door");
            yield return null;
        }

        StartCoroutine(PickUpIngotRoutine());
    }


    private IEnumerator PickUpIngotRoutine()
    {
        blacksmithCoroutineRunning = true;

        while (!Ingot.pickedUpIngot)
        {
            ShowControllerHints(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger, "Grab the ingot");
            yield return null;
        }

        StartCoroutine(HeatUpIngotRoutine());

        //HideAllControllerHints();
    }

    private IEnumerator HeatUpIngotRoutine()
    {
        while (!Ingot.isHotEnough)
        {
            ShowControllerHints(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger, "Place ingot in furnace");
            yield return null;
        }

        StartCoroutine(PlaceOnAnvilRoutine());

        //HideAllControllerHints();
    }


    private IEnumerator PlaceOnAnvilRoutine()
    {
        while (!Ingot.isPlacedOnAnvil)
        {
            ShowControllerHints(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger, "Place ingot on anvil");
            yield return null;
        }

        StartCoroutine(HammerIngotRoutine());
    }

    private IEnumerator HammerIngotRoutine()
    {
        while (!FakeItem.isForming)
        {
            ShowControllerHints(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger, "Hammer it");
            yield return null;
        }


        MessageManager.Instance.SendMail("Blacksmithing", "Woah! I heard that you can make really cool weapons. I'll pay you a visit!\n\nFrom:\n???", null);
        StartCoroutine(ReadMailRoutine());

    }

    private IEnumerator ReadMailRoutine()
    {
        while(!FakeItem.isForming)
        {
            ShowControllerHints(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu, "Check your mail");
            yield return null;
        }

        CompleteCurrentEvent();
    }


    #endregion

    #region Selling Tutorial Sequence

    public void SellingTutorialEvent()
    {

    }



    #endregion









}




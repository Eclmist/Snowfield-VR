using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
using Valve.VR.InteractionSystem;

namespace Opening_Room
{
	[System.Serializable]
	public struct SequenceObjectGroup
	{
		public GameObject message;
		public TeleportPoint computerTeleportationPoint;
		public Lamp lamp;
		public Keyboard keyboard;

		public VideoPlayer tv;
		public VideoClip messageVideo;

		public GameObject BGM;
		public GameObject StaticLoop;
		public GameObject Music;

		public GameObject GlitchLeft;
		public GameObject GlitchMiddle;
		public GameObject GlitchRight;

	}

	[System.Serializable]
	public class SequenceEvent
	{
		public UnityEvent invokableEventUpdate;
		[Range(0,100)] public float startDelay;
	}

	public class Room : MonoBehaviour
	{

		[SerializeField] private SequenceObjectGroup sequenceObjects;

		[SerializeField] private List<SequenceEvent> events;

		private float timer = 0;

		public SequenceEvent currentEvent; // TODO: Make private

		private static bool currentEventCompleted = false;

		private Valve.VR.InteractionSystem.Player player;

		protected void Start()
		{
			player = Valve.VR.InteractionSystem.Player.instance;
			currentEvent = null;
		}

		protected void Update()
		{
			// Increment timer
			timer += Time.deltaTime;

			// Try to play next event
			if (currentEvent == null)
			{
				if (events.Count <= 0) //No more events, can kill level blueprint already
				{
					enabled = false;
					return;
				}

				currentEvent = events[0];
				events.RemoveAt(0);
			}

			if (currentEvent.startDelay < timer)
			{
				// play event
				currentEvent.invokableEventUpdate.Invoke();

				// remove event once completed
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

		public void HintTeleportEventUpdate()
		{
			if (!ControllerButtonHints.IsButtonHintActive(player.rightHand, Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
			{
				CompleteCurrentEvent();
			}
		}

		private bool lampCoroutineRunning;

		public void TurnOnLampUpdate()
		{
			if (!sequenceObjects.lamp)
			{
				CompleteCurrentEvent();
				return;

			}

			if (sequenceObjects.lamp.GetTurnedOn() == false)
			{
				foreach (Hand hand in player.hands)
				{
					ControllerButtonHints.ShowTextHint(hand, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger, "Turn on the lamp");
					sequenceObjects.lamp.HintObject();
				}
			}

			if (!lampCoroutineRunning)
				StartCoroutine(LampUpdate());
		}

		private IEnumerator LampUpdate()
		{
			lampCoroutineRunning = true;

			while (!sequenceObjects.lamp.GetTurnedOn())
			{
				yield return new WaitForFixedUpdate();
			}

			foreach (Hand hand in player.hands)
				ControllerButtonHints.HideAllTextHints(hand);



			CompleteCurrentEvent();
		}

		private bool messageSequenceStarted = false;

		public void MessageEventUpdate()
		{
			if (!messageSequenceStarted)
			{
				StartCoroutine(MessageSequence());
				messageSequenceStarted = true;
			}


		}

		// Hurray for hard code
		private IEnumerator MessageSequence()
		{
			sequenceObjects.BGM.SetActive(true);
			yield return new WaitForSeconds(3);

			sequenceObjects.message.SetActive(true);
			sequenceObjects.computerTeleportationPoint.enabled = true;

			yield return new WaitForSeconds(0.5F);
			sequenceObjects.lamp.TurnOn(false);


			yield return StartCoroutine(WaitForKeyboard("Open Message"));

			sequenceObjects.Music.SetActive(true);

			sequenceObjects.message.SetActive(false);
			sequenceObjects.tv.enabled = true;
			sequenceObjects.tv.clip = sequenceObjects.messageVideo;
			sequenceObjects.tv.isLooping = false;

			while (sequenceObjects.tv.time < 5)
			{
				yield return null;
			}

			sequenceObjects.tv.Pause();

			yield return StartCoroutine(WaitForKeyboard("Yes"));

			//sequenceObjects.tv.Play();

			//while (sequenceObjects.tv.time < 10)
			//{
			//	yield return null;
			//}

			//sequenceObjects.tv.Pause();

			//yield return StartCoroutine(WaitForKeyboard());

			//sequenceObjects.tv.Play();
			// Make crazy shit happen here

			sequenceObjects.StaticLoop.SetActive(true);
			sequenceObjects.GlitchMiddle.SetActive(true);

			yield return new WaitForSeconds(0.4F);

			sequenceObjects.GlitchRight.SetActive(true);
			yield return new WaitForSeconds(0.3F);
			sequenceObjects.GlitchLeft.SetActive(true);



		}

		private IEnumerator WaitForKeyboard(string hoverhint)
		{
			float startTime = Time.time;
			bool hinting = false;

			sequenceObjects.keyboard.SetHoverHint(hoverhint);

			while (!sequenceObjects.keyboard.GetKey())
			{

				if (!hinting && Time.time - startTime > 7)
				{
					sequenceObjects.keyboard.HintObject();
					hinting = true;
				}
				yield return null;
			}

			Debug.Log("Keyboard Pressed");
		}
	}
}

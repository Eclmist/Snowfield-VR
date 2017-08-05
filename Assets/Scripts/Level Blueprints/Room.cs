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
		public AudioClip notificationSound;

		public GameObject message;
		public TeleportPoint computerTeleportationPoint;
		public Lamp lamp;
		public Keyboard keyboard;
		public Bed bed;

		public VideoPlayer tv;
		public MeshRenderer tvMeshRen;

		public VideoClip messageVideo;

		public GameObject BGM;
		public GameObject StaticLoop;
		public GameObject Music;

		public Renderer GlitchLeft;
		public Renderer GlitchMiddle;
		public Renderer GlitchRight;

		public Renderer windowLight;
		public ReflectionProbe reflectionProbe;

		public AudioSource ambient;

		public OutlineRenderer outlineRen;
		public GlitchCamera glitchScript;
	}

	[System.Serializable]
	public class SequenceEvent
	{
		public UnityEvent invokableEventUpdate;
		[Range(0,100)] public float startDelay;
		public bool enabled = true;
	}

	public class Room : MonoBehaviour
	{

		[SerializeField] private SequenceObjectGroup sequenceObjects;

		[SerializeField] private List<SequenceEvent> events;

		private float timer = 0;

		[Space(20)]
		[Header("Debug READONLY")]
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
			// hint will be triggered by teleport.cs

			if (Teleport.playerKnowsHowToTeleport)
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
				ShowControllerHints(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger, "Turn on the lamp");
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
			sequenceObjects.outlineRen.enabled = true;

			sequenceObjects.computerTeleportationPoint.enabled = true;

			yield return new WaitForSeconds(0.5F);

			yield return StartCoroutine(WaitForKeyboard("Open message"));

			sequenceObjects.lamp.TurnOn(false);
			sequenceObjects.lamp.enabled = false;

			sequenceObjects.Music.SetActive(true);

			sequenceObjects.message.SetActive(false);
			sequenceObjects.tvMeshRen.enabled = true;
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

			sequenceObjects.outlineRen.enabled = false;

			sequenceObjects.StaticLoop.SetActive(true);
			sequenceObjects.GlitchMiddle.enabled = true;
			sequenceObjects.GlitchMiddle.GetComponent<AudioSource>().Play();
			yield return new WaitForSeconds(0.3F);

			sequenceObjects.GlitchRight.enabled = true;
			sequenceObjects.GlitchRight.GetComponent<AudioSource>().Play();

			yield return new WaitForSeconds(0.4F);
			sequenceObjects.GlitchLeft.enabled = true;
			sequenceObjects.GlitchLeft.GetComponent<AudioSource>().Play();

			float glitchVal = 0;
			sequenceObjects.glitchScript.enabled = true;

			while (glitchVal < 1)
			{
				glitchVal += Time.deltaTime;
				sequenceObjects.glitchScript.SetGlitchAmount(glitchVal);
			}

		}

		private IEnumerator WaitForKeyboard(string hoverhint)
		{
			float startTime = Time.time;
			bool hinting = false;

			ShowControllerHints(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger, hoverhint);

			while (!sequenceObjects.keyboard.GetKey())
			{

				if (!hinting && Time.time - startTime > 10)
				{
					sequenceObjects.keyboard.HintObject();
					hinting = true;
				}
				yield return null;
			}

			HideAllControllerHints();
		}

		bool interactWithObjectEventFlag = false;
		public void InteractWithObjectsEvent()
		{
			if (!interactWithObjectEventFlag)
			{
				interactWithObjectEventFlag = true;
				VR_Interactable_Object.playerKnowsHowToInteractWithObjects = false;

				ShowControllerHints(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger, "Clean your room");

				StartCoroutine(InteractWithObjectCoroutine());
			}
		}

		private IEnumerator InteractWithObjectCoroutine()
		{
			while (!VR_Interactable_Object.playerKnowsHowToInteractWithObjects)
			{
				yield return null;
			}

			yield return StartCoroutine(EndEventAfterDelay(10));

			HideAllControllerHints();
		}

		bool sleeping = false;

		public void SleepEvent()
		{
			if (!sleeping)
			{
				ShowControllerHints(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger, "Go back to sleep");
				Bed.canSleep = true;
				sequenceObjects.bed.HintObject();
				sleeping = true;
				StartCoroutine(SleepEventCoroutine());

			}
		}

		private IEnumerator SleepEventCoroutine()
		{
			sleeping = true;

			while (!Bed.sleeping)
			{
				yield return null;
			}

			sequenceObjects.outlineRen.enabled = false;

			AudioFadeOut.FadeOut(sequenceObjects.ambient, 2);
			Bed.canSleep = false;
			HideAllControllerHints();

			while (!Bed.eyeClosed)
			{
				yield return null;
			}

			AudioFadeOut.FadeIn(sequenceObjects.ambient, 2);


			//Eye closed
			sequenceObjects.windowLight.material.SetColor("_EmissionColor", Color.black);
			sequenceObjects.windowLight.UpdateGIMaterials();
			sequenceObjects.reflectionProbe.intensity = 2.1F;
			CompleteCurrentEvent();
		}



		bool eventScheduledToEnd = false;

		private IEnumerator EndEventAfterDelay(float time)
		{
			if (!eventScheduledToEnd)
			{
				eventScheduledToEnd = true;
				yield return new WaitForSeconds(time);
				CompleteCurrentEvent();
				eventScheduledToEnd = false;
			}
			//Debug.Log("delayed event ended" + time);
		}

		private void HideAllControllerHints()
		{
			foreach (Hand hand in player.hands)
			{
				ControllerButtonHints.HideAllTextHints(hand);
			}
		}

		private void ShowControllerHints(Valve.VR.EVRButtonId btn, string text, bool glowbtn = true)
		{

			foreach (Hand hand in player.hands)
			{
				ControllerButtonHints.ShowTextHint(hand, btn, text, glowbtn);

				AudioSource.PlayClipAtPoint(sequenceObjects.notificationSound, hand.transform.position);
			}



		}
	}
}

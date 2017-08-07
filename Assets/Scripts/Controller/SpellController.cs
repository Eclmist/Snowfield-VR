using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edwon.VR.Gesture;
using Edwon.VR;

public class SpellController : MonoBehaviour {

    protected VRGestureRig rig;

    protected Transform leftHand;
    protected Transform rightHand;

	protected VR_Controller_Custom vrController;

    protected Spell currentSpell;

    void Start()
	{
        rig = FindObjectOfType<VRGestureRig>();
        if (rig == null)
        {
            Debug.Log("there is no VRGestureRig in the scene, please add one");
        }

        rightHand = rig.handRight;
        leftHand = rig.handLeft;

        vrController = GetComponent<VR_Controller_Custom>();
    }

	void OnEnable()
	{
		GestureRecognizer.GestureDetectedEvent += OnGestureDetected;
	}

	void OnDisable()
	{
		GestureRecognizer.GestureDetectedEvent -= OnGestureDetected;
	}

	void OnGestureDetected(string gestureName, double confidence, Handedness hand, bool isDouble = false)
	{

        if ((hand == Handedness.Left && vrController.Handle == VR_Controller_Custom.Controller_Handle.LEFT) || (hand == Handedness.Right && vrController.Handle == VR_Controller_Custom.Controller_Handle.RIGHT))
        {
            Spell spell = SpellManager.Instance.GetSpell(gestureName);
            Spell spellInstance = Instantiate(spell, vrController.transform).GetComponent<Spell>();
			vrController.SetInteraction(spellInstance);
			spellInstance.LinkedController = vrController;

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edwon.VR.Gesture;
using Edwon.VR;

public class SpellController : VR_Interactable_Object
{

    protected VRGestureRig rig;

    protected Transform leftHand;
    protected Transform rightHand;

    protected Spell currentSpell;

    protected override void Start()
    {
        rig = FindObjectOfType<VRGestureRig>();
        if (rig == null)
        {
            Debug.Log("there is no VRGestureRig in the scene, please add one");
        }

        rightHand = rig.handRight;
        leftHand = rig.handLeft;

        currentInteractingController = GetComponent<VR_Controller_Custom>();
        if (!currentInteractingController)
        {
            Debug.Log("No Controller");
            Destroy(this);
        }
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

        if ((hand == Handedness.Left && currentInteractingController.Handle == VR_Controller_Custom.Controller_Handle.LEFT) || (hand == Handedness.Right && currentInteractingController.Handle == VR_Controller_Custom.Controller_Handle.RIGHT))
        {
            Spell spell = SpellManager.Instance.GetSpell(gestureName);

            Spell spellInstance = Instantiate(spell, currentInteractingController.transform).GetComponent<Spell>();
            currentInteractingController.SetInteraction(this);
        }
    }
}

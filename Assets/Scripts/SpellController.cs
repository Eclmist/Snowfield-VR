using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edwon.VR.Gesture;
using Edwon.VR;

public class SpellController : MonoBehaviour {

	//protected VRGestureRig rig;

	//protected Transform leftHand;
	//protected Transform rightHand;

    [SerializeField]
	protected VR_Controller_Custom vrController;

    protected Spell currentSpell;

    void Start()
	{
		//rig = FindObjectOfType<VRGestureRig> ();
		//if (rig == null) {
		//	Debug.Log ("there is no VRGestureRig in the scene, please add one");
		//}

		//rightHand = rig.handRight;
		//leftHand = rig.handLeft;
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
        //switch (gestureName)
        //{
        //case "Circle1":
        //	{
        //		// DO SOMETHING REACTIN TO CIRCLE GESTURE
        //		SorceryCast.Instance.Charge(rightHand);
        //	}
        //	break;
        //case "Push1":
        //	{
        //		// DO SOMETHING REACTING TO TRIANGLE GESTURE
        //		SorceryCast.Instance.Cast(rightHand);
        //	}
        //	break;
        //}

        //Spell spell = SpellManager.Instance.GetSpell(gestureName);
        //Instantiate(spell, vrController.transform);
        //vrController.SetInteraction(spell);
        //spell.LinkedController = vrController;

        Spell spell = SpellManager.Instance.GetSpell(gestureName);
        Instantiate(spell, vrController.transform);
        vrController.SetInteraction(spell);
        spell.LinkedController = vrController;


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Spell spell = SpellManager.Instance.GetSpell("Circle");
            Instantiate(spell, vrController.transform);
            vrController.SetInteraction(spell);
            spell.LinkedController = vrController;
            currentSpell = spell;
        }

        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    Spell spell = SpellManager.Instance.GetSpell("Complex");
        //    Instantiate(spell, vrController.transform);
        //    vrController.SetInteraction(spell);
        //    spell.LinkedController = vrController;
        //}

        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    Spell spell = SpellManager.Instance.GetSpell("Bolt");
        //    Instantiate(spell, vrController.transform);
        //    vrController.SetInteraction(spell);
        //    spell.LinkedController = vrController;
        //}
    }
}

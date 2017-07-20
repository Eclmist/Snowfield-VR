using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spell : VR_Interactable
{

    private GameObject spellPrefab;


    public GameObject SpellPrefab
    {
        get { return this.spellPrefab; }
    }



    protected virtual void Cast()
    {
        Debug.Log("fire!fire!");
    }


    public override void OnInteracting(VR_Controller_Custom controller)
    {
        transform.position = LinkedController.transform.position;
        transform.rotation = LinkedController.transform.rotation;
    }

    public override void OnTriggerPress(VR_Controller_Custom controller)
    {
        Cast();
    }






}

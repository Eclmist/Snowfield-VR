using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericItem : MonoBehaviour,IInteractable {

    [SerializeField]    protected string name;
    [SerializeField]    protected float weight;
    [SerializeField]    protected AudioClip sound;
    [SerializeField]    protected AudioSource audioSource;

    public string Name
    {
        get { return this.name; }
        set { this.name = value; }
    }

    public float Weight
    {
        get { return this.weight; }
        set { this.weight = value; }
    }

    public void Interact()
    {
        


    }



	
}

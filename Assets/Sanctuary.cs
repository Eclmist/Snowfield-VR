using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sanctuary : MonoBehaviour
{

    [SerializeField]
    protected LayerMask protectionMask;

    protected List<Actor> objectsProtected = new List<Actor>();

    protected void Update()
    {
        foreach(Actor actor in objectsProtected)
        {
            actor.InSanctuary = true;
        }
    }
    protected void OnTriggerEnter(Collider col)
    {
        if ((protectionMask.value & 1 << col.gameObject.layer) != 0)
        {
            Actor actor = col.GetComponentInParent<Actor>();
            if (actor)
            {
                objectsProtected.Add(actor);
            }
            else if (col == Player.Instance.Collider)
            {
                objectsProtected.Add(Player.Instance);
            }
        }
    }

    protected void OnTriggerExit(Collider col)
    {
        if ((protectionMask.value & 1 << col.gameObject.layer) != 0)
        {
            Actor actor = col.GetComponentInParent<Actor>();
            if (actor && objectsProtected.Contains(actor))
            {
                actor.InSanctuary = false;
                objectsProtected.Remove(actor);
            }
            else if (col == Player.Instance.Collider && objectsProtected.Contains(Player.Instance))
            {
                
                Player.Instance.InSanctuary = false;
                objectsProtected.Remove(Player.Instance);
            }
        }
    }
}

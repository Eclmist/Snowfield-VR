using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeItem : MonoBehaviour {

    public ItemData trueForm;
    public static bool isForming = false;

    public void RevealFinalForm()
    {
        isForming = true;
		Instantiate(trueForm.ObjectReference, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}

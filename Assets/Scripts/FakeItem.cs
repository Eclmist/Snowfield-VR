using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeItem : MonoBehaviour {

    public ItemData trueForm;

    public void RevealFinalForm()
    {
		Debug.Log("real one");
		Instantiate(trueForm.ObjectReference, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}

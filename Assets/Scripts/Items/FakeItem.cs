using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeItem : MonoBehaviour {

    public ItemData trueForm;

    public void RevealFinalForm()
    {
        Instantiate(trueForm.ObjectReference,transform);
        Destroy(this.gameObject);
    }
}

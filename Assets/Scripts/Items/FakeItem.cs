using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeItem : MonoBehaviour {

    public ItemData trueForm;
    public static bool isForming = false;

    public void RevealFinalForm()
    {
        isForming = true;
	    StartCoroutine(FadeOut());
    }

	IEnumerator FadeOut()
	{
		Renderer ren = GetComponent<Renderer>();
		if (ren)
		{
			Material mat = ren.material;
			float reductionSpeed = Time.deltaTime * 2;
			Color currentColor = mat.GetColor("_EmissionColor");

			while (currentColor.maxColorComponent > 0)
			{
				currentColor.r -= reductionSpeed;
				currentColor.g -= reductionSpeed;
				currentColor.b -= reductionSpeed;

				mat.SetColor("_EmissionColor", currentColor);
				yield return null;
			}
		}

		Instantiate(trueForm.ObjectReference, transform.position, transform.rotation);
		Destroy(this.gameObject);

	}
}

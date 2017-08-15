using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public struct TrailerItem
{
    public Camera cam1, cam2, cam3, cam4, cam5;
    public GameObject mouse, mail, cupboard;
    public VideoPlayer glitchL, glitchC, glitchR, glitchTV;
    public MeshRenderer TV;
}


public class Trailer : MonoBehaviour {
    [SerializeField]
    private TrailerItem trailerItem;

    private Animator[] camAnim = new Animator[5];
	// Use this for initialization
	void Start ()
    {
        camAnim[0] = trailerItem.cam1.GetComponent<Animator>();
        camAnim[1] = trailerItem.cam2.GetComponent<Animator>();
        camAnim[2] = trailerItem.cam3.GetComponent<Animator>();
        camAnim[3] = trailerItem.cam4.GetComponent<Animator>();
        camAnim[4] = trailerItem.cam5.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.F1))
            {
                trailerItem.cam1.gameObject.SetActive(true);
                camAnim[0].SetBool("Transit", true);
            }
            else
            {
                trailerItem.cam1.gameObject.SetActive(false);
                camAnim[0].SetBool("Transit", false);
            }

            if (Input.GetKey(KeyCode.F2))
            {
                trailerItem.cam2.gameObject.SetActive(true);
                camAnim[1].SetBool("Transit", true);
            }
            else
            {
                trailerItem.cam2.gameObject.SetActive(false);
                camAnim[1].SetBool("Transit", false);
            }

            if (Input.GetKey(KeyCode.F3))
            {
                trailerItem.mouse.SetActive(true);
                trailerItem.mail.SetActive(true);
                trailerItem.mouse.GetComponent<Animator>().SetBool("Transit", true);
                trailerItem.cam3.gameObject.SetActive(true);
                camAnim[2].SetBool("Transit", true);
            }
            else
            {
                trailerItem.mail.SetActive(false);
                trailerItem.cam3.gameObject.SetActive(false);
                camAnim[2].SetBool("Transit", false);
            }

            if (Input.GetKey(KeyCode.F4))
            {
                trailerItem.cam4.gameObject.SetActive(true);
                StartCoroutine(WaitForCam3(0.75f));
                StartCoroutine(WaitForGlitch(0.25f));
                //camAnim[3].SetBool("Transit", true);
            }
            else
            {
                if(!Input.GetKey(KeyCode.F5))
                {
                    trailerItem.glitchL.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    trailerItem.glitchR.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    trailerItem.glitchC.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    trailerItem.mouse.SetActive(true);
                }
                trailerItem.cam4.gameObject.SetActive(false);
                camAnim[3].SetBool("Transit", false);
                trailerItem.TV.enabled = true;

            }

            if (Input.GetKey(KeyCode.F5))
            {
                trailerItem.cam5.gameObject.SetActive(true);
                camAnim[4].SetBool("Transit", true);
                StartCoroutine(WaitForCupboard(0.2f));
                //camAnim[3].SetBool("Transit", true);
            }
            else
            {
                trailerItem.glitchL.gameObject.GetComponent<MeshRenderer>().enabled = false;
                trailerItem.glitchR.gameObject.GetComponent<MeshRenderer>().enabled = false;
                trailerItem.glitchC.gameObject.GetComponent<MeshRenderer>().enabled = false;
                trailerItem.cam5.gameObject.SetActive(false);
                camAnim[3].SetBool("Transit", false);
                trailerItem.cupboard.GetComponent<Animator>().enabled = false;
            }
        }
    }

    IEnumerator WaitForCam3(float i)
    {
        yield return new WaitForSeconds(i);
        camAnim[3].SetBool("Transit", true);
    }

    IEnumerator WaitForGlitch(float i)
    {
        trailerItem.TV.enabled = false;
        trailerItem.glitchTV.Play();
        yield return new WaitForSeconds(i);
        trailerItem.mouse.SetActive(false);
        trailerItem.glitchC.gameObject.GetComponent<MeshRenderer>().enabled = true;
        trailerItem.glitchC.Play();
        yield return new WaitForSeconds(i);
        trailerItem.glitchL.gameObject.GetComponent<MeshRenderer>().enabled = true;
        trailerItem.glitchL.Play(); 
        yield return new WaitForSeconds(i);
        trailerItem.glitchR.gameObject.GetComponent<MeshRenderer>().enabled = true;
        trailerItem.glitchR.Play(); 
    }

    IEnumerator WaitForCupboard(float i)
    {
        yield return new WaitForSeconds(i);
        trailerItem.cupboard.GetComponent<Animator>().enabled = true;
    }
}

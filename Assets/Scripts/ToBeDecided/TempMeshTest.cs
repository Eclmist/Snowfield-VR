using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMeshTest : MonoBehaviour
{

    public Mesh toTestFor;
    public GameObject containsMesh;

    private Comparism currentTest;
    private float lastProgress = 0;
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            MeshFilter mf = containsMesh.GetComponent<MeshFilter>();
            if (!mf)
            {
                Debug.Log("No Mesh Filter on target Gameobject");
                return;
            }

            Mesh m = mf.mesh;
            if (!m)
            {
                Debug.Log("No Mesh on target Gameobject");
                return;
            }

            Comparism newComparism = MeshComparer.Instance.StartCompare(m, toTestFor);
            if (newComparism == null)
            {
                Debug.Log("A comparism is already in progress!");
                return;
            }

            currentTest = newComparism;
            Debug.Log("A comparism has started!");

            lastProgress = 0;
        }

        if (currentTest == null)
            return;

        if (currentTest.progress != lastProgress)
        {
            //Debug.Log("Comparism " + currentTest.UID + " progress: " + currentTest.progress * 100 + "%");
            lastProgress = currentTest.progress;
        }

        if (currentTest.progress == 1)
        {
            Debug.Log("Comparism " + currentTest.UID + " result: " + currentTest.comparismResult * 100 + "% matching original mesh.");
            currentTest = null;
        }
    }
}

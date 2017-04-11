using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class MorphVertex : MonoBehaviour
{
    [SerializeField] private Mesh[] morphPhases;
    [SerializeField] [Range(1, 30)] private int stepsPerPhase;
    [SerializeField] private int startingPhase = 1;

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    private int currentPhase;
    private int currentStep;
    private int totalPhases;

    private Mesh activeMesh;

    protected void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();

        totalPhases = morphPhases.Length;

        if (totalPhases <= 1)
        {
            Debug.LogError("At least 2 mesh needs to be specified in MorphVertex.morphPhases. The script will not work.");
            Destroy(this);
            return;
        }

        int vertexCountCheck = morphPhases[0].vertices.Length;

        for (int i = 1; i < morphPhases.Length; i++)
        {
            if (vertexCountCheck != morphPhases[i].vertices.Length)
            {
                Debug.LogError("Mesh specified do not have the same number of vertices. The script will not work.");
                Destroy(this);
                return;
            }
        }

        startingPhase =
            startingPhase < 0 ? 0 : startingPhase > totalPhases ? totalPhases : startingPhase;

        currentPhase = startingPhase;
        currentStep = 0;

        Morph(0);

    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Morph(1);
        }
    }

    private void RecalculatePhase(int step)
    {
        int tempStep = currentStep + step;

        while (tempStep >= stepsPerPhase)
        {
            tempStep -= stepsPerPhase;
            currentPhase++;

            if (currentPhase >= totalPhases)
            {
                currentPhase = totalPhases;
                currentStep = stepsPerPhase;

                return;
            }
        }

        currentStep = tempStep;
    }

    private void UpdateMesh()
    {
        Debug.Assert(currentPhase + 1 < totalPhases);

        Mesh lhs = morphPhases[currentPhase];
        Mesh rhs = morphPhases[currentPhase + 1];

        activeMesh = lhs;

        for (int i = 0; i < activeMesh.vertices.Length; i++)
        {
            activeMesh.vertices[i] = Vector3.Lerp(activeMesh.vertices[i], rhs.vertices[i], currentStep);
        }

        meshFilter.mesh = activeMesh; 
    }

    private void RecalculateColliders()
    {
        meshCollider.convex = true;
        meshCollider.sharedMesh = activeMesh;
    }

    public void Morph(int step = 1)
    {
        RecalculatePhase(step);
        UpdateMesh();
        RecalculateColliders();
    }
}

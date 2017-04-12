using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Comparism
{
    public int UID;
    public float comparismResult;
    public float progress;

    public Comparism(int UID)
    {
        this.UID = UID;
        comparismResult = 0;
        progress = 0;
    }
}

public class MeshComparer : MonoBehaviour
{
    public static MeshComparer Instance;

    private static float progress;                          // 0 if no progress, 1 is completed.
    private static float comparerSkinWidth;
    private static int comparismUID;
    private static Dictionary<int, Comparism> UID_CompareResult_Pair;

    private static float maxAllowedThreadTime;

    protected void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There are multiple instances of Mesh Comparer in the scene!");
            Destroy(this);
        }
        else
        {
            Instance = this;
            progress = 0;
            comparerSkinWidth = 0.1F;
            comparismUID = 0;
            UID_CompareResult_Pair = new Dictionary<int, Comparism>();
            maxAllowedThreadTime = 0.005F;
        }
    }

    public static bool ComparismInProgress(int currentUID)
    {
        Comparism latestComparism;

        if (UID_CompareResult_Pair.TryGetValue(currentUID, out latestComparism))
        {
            return latestComparism.progress != 1;
        }
        else
        {
            return false;
        }
    }

    // Returns UID if comparism starts
    // Returns -1 if a comparism is already in progress
    public int StartCompare(Mesh comparer, Mesh target)
    {
        if (ComparismInProgress(comparismUID))
            return -1;
        else
        {
            comparismUID++;
            StartCoroutine(Compare(comparer, target));
            return comparismUID;
        }
    }

    private IEnumerator Compare(Mesh comparer, Mesh target)
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        Comparism currentComparism = new Comparism(comparismUID);
        AddToLimitedDictionary(comparismUID, currentComparism);

        // Create 2 Mesh Colliders, one for inner one for outer
        MeshCollider innerMeshCollider = new MeshCollider();
        MeshCollider outerMeshCollider = new MeshCollider();

        Mesh innerMesh = Object.Instantiate(target);
        Mesh outerMesh = Object.Instantiate(target);

        float innerModifier = 1 - comparerSkinWidth;
        float outerModifier = 1 + comparerSkinWidth;

        Vector3[] innerVertices = innerMesh.vertices;
        Vector3[] outerVertices = outerMesh.vertices;

        Debug.Assert(innerVertices.Length == outerVertices.Length);

        float currentThreadStartTime = Time.realtimeSinceStartup;

        for (int i = 0; i < innerVertices.Length; i++)
        {
            innerVertices[i] *= innerModifier;
            outerVertices[i] *= outerModifier;

            if (Time.realtimeSinceStartup - currentThreadStartTime > maxAllowedThreadTime)
            {
                currentThreadStartTime = Time.realtimeSinceStartup;
                yield return wait;
            }
        }

        innerMesh.vertices = innerVertices;
        outerMesh.vertices = outerVertices;

        // assign mesh to collider
        innerMeshCollider.sharedMesh = innerMesh;
        outerMeshCollider.sharedMesh = outerMesh;

        int totalVertices = comparer.vertexCount;
        int totalAccurateVertices = 0;

        currentThreadStartTime = Time.realtimeSinceStartup;

        foreach (Vector3 vertex in comparer.vertices)
        {
            // Check if its within outer mesh limit
            if (outerMeshCollider.OverlapPoint(vertex))
            {
                // Check if outside inner mesh limit
                if (!innerMeshCollider.OverlapPoint(vertex))
                {
                    //Vertex accurate to skin
                    totalAccurateVertices++;
                }
            }

            if (Time.realtimeSinceStartup - currentThreadStartTime > maxAllowedThreadTime)
            {
                currentThreadStartTime = Time.realtimeSinceStartup;
                yield return wait;
            }
        }

        currentComparism.progress = 1;
        currentComparism.comparismResult = (float)totalAccurateVertices / totalVertices;
    }

    private void AddToLimitedDictionary(int UID, Comparism comparism)
    {

    }

    public float GetLatestComparismResult()
    {
        return GetComparismResult(comparismUID);
    }

    public float GetComparismResult(int UID)
    {
        Comparism comparism;

        if (!UID_CompareResult_Pair.TryGetValue(UID, out comparism))
        {
            return -1;
        }
        else
        {
            return comparism.comparismResult;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comparism
{
    public int UID;
    public float comparismResult;
    public float progress;

    public Comparism(int UID)
    {
        this.UID = UID;
        comparismResult = 0;
        progress = 0;               // 0 if no progress, 1 is completed.
    }
}

public class MeshComparer : MonoBehaviour
{
    public static MeshComparer Instance;

    private static float comparerSkinWidth;
    private static int comparismUID;
    private static int minUID;
    private static int dictionaryLength;
    private static Dictionary<int, Comparism> UID_CompareResult_Pair;

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
            comparerSkinWidth = 0.2F;
            comparismUID = 0;
            minUID = comparismUID + 1;
            dictionaryLength = 10;
            UID_CompareResult_Pair = new Dictionary<int, Comparism>();
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

    // Returns Comparism if comparism starts
    // Returns null if a comparism is already in progress
    public Comparism StartCompare(Mesh comparer, Mesh target)
    {
        if (ComparismInProgress(comparismUID))
            return null;
        else
        {
            comparismUID++;
            Comparism currentComparism = new Comparism(comparismUID);
            AddToLimitedDictionary(comparismUID, currentComparism);
            StartCoroutine(Compare(comparer, target, currentComparism));
            return currentComparism;
        }
    }

    private IEnumerator Compare(Mesh comparer, Mesh target, Comparism currentComparism)
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        // Create 2 Mesh Colliders, one for inner one for outer
        MeshCollider innerMeshCollider = gameObject.AddComponent<MeshCollider>();
        MeshCollider outerMeshCollider = gameObject.AddComponent<MeshCollider>();

        Mesh innerMesh = Instantiate(target);
        Mesh outerMesh = Instantiate(target);

        float innerModifier = 1 - comparerSkinWidth;
        float outerModifier = 1 + comparerSkinWidth;

        Vector3[] innerVertices = innerMesh.vertices;
        Vector3[] outerVertices = outerMesh.vertices;

        Debug.Assert(innerVertices.Length == outerVertices.Length);

        float currentThreadStartTime = Time.realtimeSinceStartup;

        for (int i = 0; i < innerVertices.Length; i++)
        {
            //innerVertices[i] = transform.TransformPoint(innerVertices[i]) * innerModifier;
            //outerVertices[i] = transform.TransformPoint(outerVertices[i]) * outerModifier;
            innerVertices[i] = (innerVertices[i] + target.bounds.center) * innerModifier - target.bounds.center;
            outerVertices[i] = (outerVertices[i] + target.bounds.center) * outerModifier - target.bounds.center;

            currentComparism.progress = (float)i / innerVertices.Length * 0.05F;
            yield return wait;
        }

        innerMesh.vertices = innerVertices;
        outerMesh.vertices = outerVertices;

        // assign mesh to collider
        innerMeshCollider.sharedMesh = innerMesh;
        outerMeshCollider.sharedMesh = outerMesh;

        int totalVertices = comparer.vertexCount;
        int totalAccurateVertices = 0;

        currentThreadStartTime = Time.realtimeSinceStartup;

        for (int i = 0; i < totalVertices; i++)
        {
            // Check if its within outer mesh limit
            if (outerMeshCollider.OverlapPointSimple(transform.TransformPoint(comparer.vertices[i])))
            {
                //Check if outside inner mesh limit
                if (!innerMeshCollider.OverlapPointSimple(transform.TransformPoint(comparer.vertices[i])))
                {
                    //Vertex accurate to skin
                    totalAccurateVertices++;
                }
            }

            currentComparism.progress = 0.05F + (float)i / totalVertices * 0.95F;
            yield return wait;
        }

        currentComparism.progress = 1;
        currentComparism.comparismResult = (float)totalAccurateVertices / totalVertices;

        //Cleanup
        //Destroy(innerMeshCollider);
        //Destroy(outerMeshCollider);
    }

    private static void AddToLimitedDictionary(int UID, Comparism comparism)
    {
        if (UID - minUID >= dictionaryLength)
        {
            UID_CompareResult_Pair.Remove(minUID);
            minUID++;
        }

        UID_CompareResult_Pair.Add(UID, comparism);
    }

    // Returns -1 if UID does not exist
    public static float GetComparismResult(int UID)
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

    // Returns -1 if UID does not exist
    public static float GetComparismProgress(int UID)
    {
        Comparism comparism;

        if (!UID_CompareResult_Pair.TryGetValue(UID, out comparism))
        {
            return -1;
        }
        else
        {
            return comparism.progress;
        }
    }

    // UID starts at 1. Returns 0 if no comparism has been done before.
    public static int GetLatestComparismUID()
    {
        return comparismUID;
    }
}

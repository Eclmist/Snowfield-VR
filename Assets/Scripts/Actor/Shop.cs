using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Spot where AIs go to")]
    private Transform location;
    private List<Vector3> points = new List<Vector3>();
    private int currentIndex = 0;
    [SerializeField]
    private LayerMask pointsIgnoreLayer;

    private bool isOccupied = false;

    public Vector3 Location
    {
        get
        {
            return location.position;
        }

    }

    public Vector3 GetNextLocation()
    {
        currentIndex++;
        if (currentIndex >= points.Count)
        {
            currentIndex = 0;
        }
        return points[currentIndex];
    }

    [SerializeField]
    private Actor shopOwner;

    public Actor Owner
    {
        get
        {
            return shopOwner;
        }
    }

    private void Awake()
    {

        if (!location)
        {
            Debug.Log("Location for " + gameObject.name + ": store has not been set! Hence it wont be considered a shop");
            Destroy(this);
        }
        for (int i = 0; i <= 180; i += 60)
        {
            Vector3 newVec = FindPoint(location.position, 2, i);
            if (Physics.OverlapSphere(newVec,1,~pointsIgnoreLayer).Length == 0)
            points.Add(newVec);
        }
    }

    private Vector3 FindPoint(Vector3 c, float r, int i)
    {
        return c + Quaternion.AngleAxis(i, location.up) * (location.right * r);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(location.position, .1f);
        foreach (Vector3 x in points)
        {
            Gizmos.DrawSphere(x, .1f);
        }

    }
}

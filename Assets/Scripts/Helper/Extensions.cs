using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static bool OverlapPointSimple(this Collider collider, Vector3 point)
    {
        Vector3 center;
        Vector3 direction;
        Ray ray;
        RaycastHit hitInfo;
        bool hit;

        // Use collider bounds to get the center of the collider. May be inaccurate
        // for some colliders (i.e. MeshCollider with a 'plane' mesh)
        center = collider.bounds.center;

        // Cast a ray from point to center
        direction = center - point;
        ray = new Ray(point, direction);
        hit = collider.Raycast(ray, out hitInfo, direction.magnitude);

        // If we hit the collider, point is outside. So we return !hit
        return !hit;

    }

    public static float DistanceToClosestPoint(this Collider collider, Vector3 point)
    {
        Vector3 closestPoint = collider.ClosestPoint(point);

        return Vector3.Distance(point, closestPoint);
    }
}

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

	// SUPER EXPENSIVE, USE AT OWN RISK
    public static float DistanceToClosestPointNonConvex(this MeshCollider collider, Vector3 point)
    {
		Mesh colliderMesh = collider.sharedMesh;

		int[] triangle = colliderMesh.triangles;
		Vector3[] vertex = colliderMesh.vertices;
		Vector3[] normal = colliderMesh.normals;

		float shortestDistance = float.MaxValue;

		// foreach triangle
		for (int i = 0; i < triangle.Length; i += 3)
		{
			Vector3 v1 = vertex[triangle[i]];
			Vector3 v2 = vertex[triangle[i + 1]];
			Vector3 v3 = vertex[triangle[i + 2]];

			Vector3 n = (normal[triangle[i]] + normal[triangle[i + 1]] + normal[triangle[i + 2]]).normalized;

			float distance = Mathf.Abs(Vector3.Dot(n, v1) - Vector3.Dot(n, point));

			if (distance < shortestDistance)
				shortestDistance = distance;
		}

		return shortestDistance;
	}
}

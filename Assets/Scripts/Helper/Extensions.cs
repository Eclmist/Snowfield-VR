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
    public static bool OverlapPoint(this Collider collider, Vector3 point)
    {
        Vector3 temp;
        Vector3 start = new Vector3(0, 100, 0); // This is defined to be some arbitrary point far away from the collider.
        Vector3 direction = point - start; // This is the direction from start to goal.
        direction.Normalize();
        int itterations = 0; // If we know how many times the raycast has hit faces on its way to the target and back, we can tell through logic whether or not it is inside.
        temp = start;


        while (temp != point) // Try to reach the point starting from the far off point.  This will pass through faces to reach its objective.
        {
            RaycastHit hit;
            if (Physics.Linecast(temp, point, out hit)) // Progressively move the point forward, stopping everytime we see a new plane in the way.
            {
                itterations++;
                temp = hit.point + (direction / 100.0f); // Move the Point to hit.point and push it forward just a touch to move it through the skin of the mesh (if you don't push it, it will read that same point indefinately).
            }
            else
            {
                temp = point; // If there is no obstruction to our goal, then we can reach it in one step.
            }
        }
        while (temp != start) // Try to return to where we came from, this will make sure we see all the back faces too.
        {
            RaycastHit hit;
            if (Physics.Linecast(temp, start, out hit))
            {
                itterations++;
                temp = hit.point + (-direction / 100.0f);
            }
            else
            {
                temp = start;
            }
        }
        return (itterations % 2 == 1);
    }
}

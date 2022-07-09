using UnityEngine;

namespace OVAWORKT
{
    public static class PhysicsUtility
    {
        public static bool WithinRange(this Vector3 pointA, Vector3 pointB, float minRange) => (pointA - pointB).sqrMagnitude <= (minRange * minRange);

        public static bool TagIsAllowed(this Collider collider, params string[] allowedTags)
        {
            for (int i = 0; i < allowedTags.Length; i++)
                if (collider.CompareTag(allowedTags[i]))
                    return true;

            return false;
        }

        public static bool WithinBounds(this Bounds bounds, Bounds other)
        {
            Vector3[] corners = new Vector3[8];
            corners[0] = other.min;
            corners[1] = new Vector3(other.min.x, other.min.y, other.max.z);
            corners[2] = new Vector3(other.max.x, other.min.y, other.max.z);
            corners[3] = new Vector3(other.max.x, other.min.y, other.min.z);

            corners[4] = new Vector3(other.min.x, other.max.y, other.min.z);
            corners[5] = new Vector3(other.min.x, other.max.y, other.max.z);
            corners[6] = other.max;
            corners[7] = new Vector3(other.max.x, other.max.y, other.min.z);

            for (int i = 0; i < corners.Length; i++)
                if (!bounds.Contains(corners[i]))
                    return false;

            return true;
        }
    }
}
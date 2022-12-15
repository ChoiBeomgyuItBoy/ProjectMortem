using UnityEngine;

namespace Mortem.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private const float waypointRadius = 0.2f;

        private void OnDrawGizmos()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(GetWaypoint(i), waypointRadius);
                Gizmos.color = Color.white;
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextIndex(i)));
            }
        }

        public int GetNextIndex(int index)
        {
            if(index == transform.childCount - 1) return 0;

            return index + 1;
        }

        public Vector3 GetWaypoint(int index)
        {
            return transform.GetChild(index).position;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [SerializeField] private float waypointSize = 0.5f;
    [SerializeField] private Transform[] waypoints;

    private void Awake()
    {
        // Populate the waypoints array based on the child transforms
        waypoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
    }
        private void OnDrawGizmos()
    {

        foreach (Transform t in waypoints) // Use the waypoints array
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(t.position, waypointSize);
        }
        Gizmos.color = Color.white;
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
        // Connect the last waypoint to the first
        if (waypoints.Length > 0)
        {
            Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);
        }
    }
    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        if (currentWaypoint == null)
        {
            return waypoints[0]; // Start at the first waypoint
        }

        int currentIndex = System.Array.IndexOf(waypoints, currentWaypoint);
        if (currentIndex < waypoints.Length - 1)
        {
            return waypoints[currentIndex + 1]; // Next waypoint
        }
        else
        {
            return null;
        }

    }
 
    public int Count => waypoints.Length;


}



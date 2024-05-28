using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public Transform[] nearbyPoints;
    public int myLayer;
    public static List<Vector3> lastWaypoints = new List<Vector3>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pacman"))
        {
            UpdateWaypoints(transform.position);
        }
    }

    private void UpdateWaypoints(Vector3 waypointPosition)
    {
        if (lastWaypoints.Count >= 2)
        {
            lastWaypoints.RemoveAt(1); // Elimina el waypoint más antiguo si hay más de dos
        }
        lastWaypoints.Insert(0, waypointPosition); // Agrega el nuevo waypoint al principio de la lista
    }
    public List<Vector3> GetLastWaypoints()
    {
        return lastWaypoints;
    }
}

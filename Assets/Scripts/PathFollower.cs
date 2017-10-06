using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathFollower : MonoBehaviour {

    public GameObject path;

    public float speed = 1;

    protected List<Transform> waypoints;
    public int currentWaypoint = 0;
    protected Vector3 shift = Vector3.zero;

    public bool Wait { get;  set; }

    public void init()
    {
        waypoints = new List<Transform>();
        foreach (Transform waypoint in path.transform)
        {
            waypoint.position += shift;
            waypoints.Add(waypoint.transform);
        }
        
    }

    public void move()
    {
        if (!Wait && currentWaypoint < waypoints.Count)
        {
            if (transform.position != waypoints[currentWaypoint].position)
            {
                transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, speed * Time.deltaTime);
            }
            else
            {
                currentWaypoint++;
            }
        }
    }

    public void rotate()
    {
        if (!Wait && currentWaypoint < waypoints.Count)
        {
            if (transform.position != waypoints[currentWaypoint].position)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, waypoints[currentWaypoint].rotation, speed * 2 * Time.deltaTime);
            }
        }
    }

    public abstract void Go();

    public virtual void replaceCamera()
    {
        return;
    }
}


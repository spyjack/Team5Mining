using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class VehicleMovement : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> targetQueue = new List<Vector3>();

    [SerializeField]
    private float speed = 200f;

    [SerializeField]
    private float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    [SerializeField]
    Seeker seeker;

    [SerializeField]
    Rigidbody2D rigidbody2D;

    [SerializeField]
    WorkerBase worker = null;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdatePath", 0f, 1f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            targetQueue.Add(mousePos);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rigidbody2D.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rigidbody2D.AddForce(force);

        float distance = Vector2.Distance(rigidbody2D.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && targetQueue.Count > 0)
        seeker.StartPath(rigidbody2D.position, targetQueue[0], OnPathComplete);
    }

    void OnPathComplete(Path _path)
    {
        if (!_path.error)
        {
            path = _path;
            currentWaypoint = 0;
        }
        targetQueue.RemoveAt(0);
    }
}

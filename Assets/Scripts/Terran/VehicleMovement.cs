using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class VehicleMovement : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> targetQueue = new List<Vector3>();

    [SerializeField]
    private float speed = 0f;

    [SerializeField]
    private float maxSpeed = 200f;

    [SerializeField]
    private float stoppingDistance = 3f;

    [SerializeField]
    private float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    [SerializeField]
    Seeker seeker;

    [SerializeField]
    Rigidbody2D rb = null;

    [SerializeField]
    VehicleClass vehicleMain = null;

    // Start is called before the first frame update
    void Start()
    {
        if (vehicleMain == null)
        {
            Debug.LogError(gameObject.name + " Missing Vehicle Main");
        }

        if (rb == null)
        {
            Debug.LogWarning(gameObject.name + " Missing RigidBody2D");
            rb = GetComponent<Rigidbody2D>();
        }

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
            if (Vector2.Distance(rb.position, targetQueue[0]) < stoppingDistance)
            {
                targetQueue.RemoveAt(0);
                if (targetQueue.Count <= 0)
                {
                    speed = 0;
                }
            }
            return;
        }else
        {
            reachedEndOfPath = false;
        }

        if (speed < maxSpeed)
        {
            speed += vehicleMain.Acceleration;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.velocity = force;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && targetQueue.Count > 0)
        seeker.StartPath(rb.position, targetQueue[0], OnPathComplete);
    }

    void OnPathComplete(Path _path)
    {
        if (!_path.error)
        {
            path = _path;
            currentWaypoint = 0;
        }
        
    }
}

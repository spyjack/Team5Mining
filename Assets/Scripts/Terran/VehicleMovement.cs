using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class VehicleMovement : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> targetQueue = new List<Vector3>();

    [SerializeField]
    private List<GameObject> targetPositions = new List<GameObject>();

    [SerializeField]
    private GameObject targetPrefab = null;

    [SerializeField]
    private float speed = 0f;

    [SerializeField]
    private float maxSpeed = 200f;

    [SerializeField]
    private float stoppingDistance = 1f;

    [SerializeField]
    private float nextWaypointDistance = 3f;

    [SerializeField]
    private float drillRange = 3f;

    Path path;
    Path path2;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    [SerializeField]
    Seeker seeker = null;

    [SerializeField]
    Rigidbody2D rb = null;

    [SerializeField]
    VehicleClass vehicleMain = null;

    [SerializeField]
    Vector2 drillPosition = new Vector2();

    [SerializeField]
    Transform drillBit = null;

    [SerializeField]
    Vector2 direction = new Vector2();

    [SerializeField]
    bool minerMode = false;

    public Vector2 DrillPoint
    {
        get { return drillPosition; }
    }

    public float Speed
    {
        get { return speed; }
    }

    public bool IsMining
    {
        get { return minerMode; }
    }

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

        ToggleMinerMode(true);

        InvokeRepeating("UpdatePath", 0f, 1f);
        AddTargetPoint(this.transform.position);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && vehicleMain.Inventory.UsedCapacity < vehicleMain.Inventory.Capacity && vehicleMain.Selected)
        {
            if (vehicleMain.PartExists(PartType.Drill))
                ToggleMinerMode(minerMode);
        }

        if (vehicleMain.Inventory.UsedCapacity >= vehicleMain.Inventory.Capacity && minerMode)
            ToggleMinerMode(true);

        if (Input.GetMouseButtonDown(1) && vehicleMain.Selected && vehicleMain.HasPart(PartType.Wheels))
        {
            AddTargetPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }else if ((Input.GetMouseButtonDown(2) && vehicleMain.Selected) || vehicleMain.Fuel <= 0)
        {
            ClearPath();
        }

        if (targetPositions.Count > 0)
        {
            if (!vehicleMain.Selected && targetPositions[0].activeInHierarchy)
            {
                foreach (GameObject item in targetPositions)
                {
                    item.SetActive(false);
                }
            }else if (vehicleMain.Selected && !targetPositions[0].activeInHierarchy)
            {
                foreach (GameObject item in targetPositions)
                {
                    item.SetActive(true);
                }
            }
        }

        if (direction != Vector2.zero)
        {
            drillPosition = rb.position + direction * drillRange;
            drillBit.position = drillPosition;
        }
        Debug.DrawLine(rb.position, drillBit.position, Color.blue);

        if (!vehicleMain.HasPart(PartType.Wheels) || vehicleMain.Crew[0] == null)
        {
            speed = 0;
            ClearPath();
        }
        if (vehicleMain.Inventory.GetFuelAmount() <= 0 && (speed > 0 || minerMode))
        {
            speed = 0;
            ClearPath();
            ToggleMinerMode(true);
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
            StopCoroutine(ConsumeGas());
            if (0 < targetQueue.Count)
            {
                if (Vector2.Distance(rb.position, targetQueue[0]) <= stoppingDistance)
                {
                        RemoveTargetPoint(0);
                        if (targetQueue.Count <= 0)
                        {
                            print("Removing spped");
                            speed = 0;
                        }
                }
            }

            return;
        }else
        {
            reachedEndOfPath = false;
        }

        if (speed < maxSpeed && !reachedEndOfPath && Vector2.Distance(rb.position, targetQueue[0]) > stoppingDistance)
        {
            speed += vehicleMain.Acceleration;
        }

        direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;


        
        

        rb.velocity = force;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
            //Debug.DrawLine(rb.position, GetPointInDirection(rb.position, path.vectorPath[currentWaypoint], 20), Color.magenta, 1f);
            //print(path.vectorPath[currentWaypoint]);
        }
    }

    public void ClearPath()
    {
        ClearTargets();
        path.vectorPath.Clear();
        speed = 0;
        rb.velocity = Vector3.zero;
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && targetQueue.Count > 0)
        {
            seeker.StartPath(rb.position, targetQueue[0], OnPathComplete);

        }
    }

    void OnPathComplete(Path _path)
    {
        if (!_path.error)
        {
            path = _path;
            currentWaypoint = 0;
            StartCoroutine(ConsumeGas());
        }
        
    }

    IEnumerator ConsumeGas()
    {
        if (speed > 0)
        {
            vehicleMain.UseFuel(-2);
            vehicleMain.VehicleGraphics.RefreshUiInv();
        }

        yield return new WaitForSeconds(0.5f);
    }



    void AddTargetPoint(Vector3 newTarget)
    {
        if (minerMode)
            newTarget.z = 1;
        targetQueue.Add(newTarget);
        newTarget.z = -2;
        targetPositions.Add(Instantiate(targetPrefab, newTarget, Quaternion.identity));
        for (int i = 0; i < targetPositions.Count; i++)
        {
            SpriteRenderer sprite = targetPositions[i].transform.GetComponent<SpriteRenderer>();
            Color spriteColor = sprite.color;
            spriteColor.a = Mathf.Max(0.1f, Mathf.Abs(i - (float)targetPositions.Count) / (float)targetPositions.Count);
            sprite.color = spriteColor;
        }
    }

    void RemoveTargetPoint(Vector2 target) //removes a target at a coordinate with a 0.1 inclusion zone, may remove multiple
    {
        for (int i = 0; i < targetQueue.Count; i++)
        {
            if(Vector2.Distance(target, targetQueue[i]) <= 0.1f)
            {
                Debug.Log("Removing Coordinate " + targetQueue[i] + " at index " + i);
                targetQueue.RemoveAt(i);
                Destroy(targetPositions[i]);
                targetPositions.RemoveAt(i);
            }
        }
    }

    void RemoveTargetPoint(int index) //removes a target at an index
    {

        Debug.Log("Removing Coordinate " + targetQueue[index] + " at index " + index);

        targetQueue.RemoveAt(index);
        Destroy(targetPositions[index]);
        targetPositions.RemoveAt(index);
        for (int i = 0; i < targetPositions.Count; i++)
        {
            SpriteRenderer sprite = targetPositions[i].transform.GetComponent<SpriteRenderer>();
            Color spriteColor = sprite.color;
            spriteColor.a = Mathf.Max(0.1f, Mathf.Abs(i - (float)targetPositions.Count) / (float)targetPositions.Count);
            sprite.color = spriteColor;
        }
    }

    void ClearTargets()
    {
        targetQueue.Clear();
        path.vectorPath.Clear();
        speed = 0;
        for (int i = targetPositions.Count-1; i >= 0; i--)
        {
            Destroy(targetPositions[i]);
        }
        targetPositions.Clear();
    }

    void ToggleMinerMode(bool isActive)
    {
        if (isActive)
        {
            seeker.graphMask = GraphMask.FromGraphName("Cleared Paths");
            minerMode = false;
            GetComponentInChildren<ShipDrilling>().drillCollider.enabled = false;
            FindObjectOfType<PlayerController>().recentMinerToggle = true;
        }
        else
        {
            seeker.graphMask = GraphMask.FromGraphName("Mining Paths");
            GetComponentInChildren<ShipDrilling>().drillCollider.enabled = true;
            minerMode = true;
        }
    }

    public void CheckDrillRange(PartDrill _drill)
    {
        if (_drill == null)
        {
            drillRange = 0;
            return;
        }

        if (_drill.Range > drillRange || _drill.Range < drillRange)
            drillRange = _drill.Range;
    }
}

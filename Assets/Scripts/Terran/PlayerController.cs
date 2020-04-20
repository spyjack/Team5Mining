using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int money = 100;

    [SerializeField]
    private List<WorkerBase> workerInventory = new List<WorkerBase>();

    [SerializeField]
    private List<PartBase> partsInventory = new List<PartBase>();

    [SerializeField]
    private float deepestDepth = 0;

    [SerializeField]
    private List<Transform> playerShips = new List<Transform>();

    [SerializeField]
    AstarPath astar = null;

    public bool dirtyNav = true;
    // Start is called before the first frame update
    void Start()
    {
        GetAllShips();
        StartCoroutine(CheckDepths());
        StartCoroutine(UpdateNavmesh());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CheckDepths()
    {
        while(true)
        {
            
            float lowest = deepestDepth;
            foreach (Transform vehicle in playerShips)
            {
                if (vehicle.position.y * 4 < lowest)
                {
                    lowest = vehicle.position.y * 4;
                }
            }
            //print("Checking Depths " + lowest);
            deepestDepth = lowest;
            yield return new WaitForSeconds(0.25f * playerShips.Count);
        }
    }

    IEnumerator UpdateNavmesh()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f * playerShips.Count);
            if (dirtyNav)
            {
                astar.Scan(astar.graphs[0]);
                dirtyNav = false;
            }
        }
    }

    void GetAllShips()
    {
        foreach (VehicleClass vehicle in GameObject.FindObjectsOfType<VehicleClass>())
        {
            playerShips.Add(vehicle.GetComponent<Transform>());
        }
    }
}

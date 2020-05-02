using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private double money = 100;

    [SerializeField]
    private List<WorkerBase> workerInventory = new List<WorkerBase>();

    [SerializeField]
    private List<PartBase> partsInventory = new List<PartBase>();

    [SerializeField]
    GameObject depthLabel = null;

    [SerializeField]
    Image depthTextBreak = null;

    [SerializeField]
    List<Text> depthTextsList = new List<Text>();

    [SerializeField]
    private List<DepthMarker> depthMarkers = new List<DepthMarker>();

    private int nextDepth = 0;

    [SerializeField]
    private float deepestDepth = 0;

    [SerializeField]
    private List<Transform> playerShips = new List<Transform>();


    [SerializeField]
    AstarPath astar = null;

    public bool dirtyNav = true;
    public bool recentMinerToggle = true;

    [SerializeField]
    BoxCollider2D camBounds = null;

    [SerializeField]
    Text moneyText = null;

    [SerializeField]
    Text depthGoalText = null;

    [SerializeField]
    Text depthCurrentText = null;

    [SerializeField]
    Text shipLogsText = null;

    [SerializeField]
    InventoryBarConnector partsInventoryBar = null;

    [SerializeField]
    InventoryBarConnector workersInventoryBar = null;

    [SerializeField]
    ShipEditor shipEditor = null;

    [SerializeField]
    GameObject StoreUIObj = null;

    [SerializeField]
    GameObject EditorUIObj = null;

    public double Money
    {
        get { return money; }
        set { money = value; }
    }

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
        moneyText.text = "Funds: $" + money;
        shipLogsText.text = "Ship Logs: \n";
        foreach (Transform ship in playerShips)
        {
            VehicleClass vehicle = ship.GetComponent<VehicleClass>();
            shipLogsText.text += ship.gameObject.name + ": Inventory: " + vehicle.Inventory.UsedCapacity + "/" + vehicle.Inventory.Capacity;
            shipLogsText.text += " Gas: " + vehicle.Inventory.GetResourceAmount(ResourceId.Fuel); 
            shipLogsText.text += "\n";
        }

        checkForSelection();
    }

    public void AddWorker(WorkerBase _worker)
    {
        workerInventory.Add(_worker);
        workersInventoryBar.AddItem(_worker);
    }

    public float GetMaxWorkers()
    {
        return workersInventoryBar.MaxItems;
    }

    public float GetWorkersCount()
    {
        return workersInventoryBar.ItemCount;
    }

    public void AddPart(PartBase _part)
    {
        partsInventory.Add(_part);
        partsInventoryBar.AddItem(_part);
    }

    public void RemovePart(PartBase _part)
    {
        partsInventory.Remove(_part);
        partsInventoryBar.RemoveItem(_part);
    }

    public float GetMaxParts()
    {
        return partsInventoryBar.MaxItems;
    }

    public float GetPartsCount()
    {
        return partsInventoryBar.ItemCount;
    }

    public void AddShip(Transform _ship)
    {
        playerShips.Add(_ship);
    }

    void checkForSelection()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            //print("casting");
            if (hit.collider != null && hit.collider.gameObject.GetComponent<VehicleClass>() != null && !StoreUIObj.activeInHierarchy && !EditorUIObj.activeInHierarchy)
            {
                VehicleClass ship = hit.collider.gameObject.GetComponent<VehicleClass>();
                if (ship.Selected)
                {
                    ship.Selected = false;
                }
                else
                {
                    ship.Selected = true;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeselectAllShips();
        }
    }

    public void DeselectAllShips()
    {
        foreach (Transform _vehicle in playerShips)
        {
            _vehicle.GetComponent<VehicleClass>().Selected = false;
        }
    }

    public void CancelSelectedTargets()
    {
        foreach (Transform _vehicle in playerShips)
        {
            VehicleClass _vehicleClass = _vehicle.GetComponent<VehicleClass>();
            if (_vehicleClass.Selected)
            {
                _vehicleClass.MovementScript.ClearPath();
            }
        }
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
            depthCurrentText.text = "Depth: " + deepestDepth.ToString("F2") + "m";
            depthGoalText.text = "Goal: " + depthMarkers[nextDepth].depthLevel + "m";
            if (deepestDepth <= depthMarkers[nextDepth].depthLevel)
            {
                
                depthTextsList[1].text = depthMarkers[nextDepth].depthName;
                depthTextsList[2].text = depthMarkers[nextDepth].depthLevel*-1 + " m";
                //offset is half of the increased amount
                Vector2 newOffset = camBounds.offset;
                Vector2 newSize = camBounds.size;
                float newYSize = Mathf.Abs(depthMarkers[nextDepth + 1].depthLevel * 0.25f);
                newSize.y += newYSize;
                newOffset.y -= (newYSize * 0.5f);
                camBounds.size = newSize;
                camBounds.offset = newOffset;
                StartCoroutine(DisplayDepthText());
                nextDepth++;
            }
            yield return new WaitForSeconds(0.25f * playerShips.Count);
        }
    }

    IEnumerator UpdateNavmesh()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f * playerShips.Count);
            if (dirtyNav && recentMinerToggle)
            {
                astar.Scan(astar.graphs[0]);
                dirtyNav = false;
                recentMinerToggle = false;
            }
        }
    }

    IEnumerator DisplayDepthText()
    {
        depthLabel.SetActive(true);
        Color labelColor = new Color(1, 1, 1, 0);
        while (labelColor.a < 1)
        {
            labelColor.a += 0.1f;
            depthTextBreak.color = labelColor;
            foreach (Text text in depthTextsList)
            {
                text.color = labelColor;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        while (labelColor.a > 0)
        {
            labelColor.a -= 0.25f;
            depthTextBreak.color = labelColor;
            foreach (Text text in depthTextsList)
            {
                text.color = labelColor;
            }
            yield return new WaitForSeconds(0.1f);
        }
        depthLabel.SetActive(false);
        yield break;
    }

    void GetAllShips()
    {
        foreach (VehicleClass vehicle in GameObject.FindObjectsOfType<VehicleClass>())
        {
            playerShips.Add(vehicle.GetComponent<Transform>());
        }
    }
}

[System.Serializable]
public struct DepthMarker
{
    
    public float depthLevel;
    
    public string depthName;
}
 


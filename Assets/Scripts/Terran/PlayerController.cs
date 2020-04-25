using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int money = 100;

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

    [SerializeField]
    BoxCollider2D camBounds = null;

    [SerializeField]
    Text moneyText = null;

    [SerializeField]
    Text depthGoalsText = null;

    [SerializeField]
    Text shipLogsText = null;

    public int Money
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
        moneyText.text = money + "$";
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
    }

    public void AddPart(PartBase _part)
    {
        partsInventory.Add(_part);
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
            print("casting");
            if (hit.collider != null && hit.collider.gameObject.GetComponent<VehicleClass>() != null)
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
            foreach (Transform _vehicle in playerShips)
            {
                _vehicle.GetComponent<VehicleClass>().Selected = false;
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
            depthGoalsText.text = deepestDepth.ToString() + " m \n Goal: " + depthMarkers[nextDepth].depthLevel;
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
            if (dirtyNav)
            {
                astar.Scan(astar.graphs[0]);
                dirtyNav = false;
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
 


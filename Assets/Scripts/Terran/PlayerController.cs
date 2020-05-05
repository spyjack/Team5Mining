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
    private float eventTimer = 60 * 2;

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
    ShopController storeMain = null;

    [SerializeField]
    GameObject StoreUIObj = null;

    [SerializeField]
    GameObject EditorUIObj = null;

    [SerializeField]
    GameObject pauseMenu = null;

    [SerializeField]
    GameObject confirmQuit = null;

    [SerializeField]
    AmbienceController ambience = null;

    public double Money
    {
        get { return money; }
        set { money = value; }
    }

    public InventoryBarConnector PartsInventory
    {
        get { return partsInventoryBar; }
    }

    public InventoryBarConnector WorkersInventory
    {
        get { return workersInventoryBar; }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetAllShips();
        StartCoroutine(CheckDepths());
        StartCoroutine(UpdateNavmesh());
        StartCoroutine(EventTimer());
        ResumeGame();
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

        if (pauseMenu.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame();
        }else if (!pauseMenu.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        if (Camera.main.transform.position.y > 10)
        {
            ambience.FadeInAmbience(AmbienceTrack.Surface);
        }
        else if (Camera.main.transform.position.y <= 10 && Camera.main.transform.position.y > depthMarkers[2].depthLevel * 0.25f)
        {
            ambience.FadeInAmbience(AmbienceTrack.UpperLayers);
        }
        else if (Camera.main.transform.position.y < depthMarkers[2].depthLevel * 0.25f)
        {
            ambience.FadeInAmbience(AmbienceTrack.DarkCaves);
        }
    }

    public void AddWorker(WorkerBase _worker)
    {
        if (_worker == null)
            return;

        workersInventoryBar.AddItem(_worker);
        for (int i = 0; i < workerInventory.Count; i++)
        {
            if (workerInventory[i] == null)
            {
                workerInventory[i] = _worker;
                return;
            }
        }
        workerInventory.Add(_worker);
    }

    public void RemoveWorker(WorkerBase _worker)
    {
        workerInventory.Remove(_worker);
        workersInventoryBar.RemoveItem(_worker);
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
        if (_part == null)
            return;

        partsInventoryBar.AddItem(_part);
        for (int i = 0; i < partsInventory.Count; i++)
        {
            if (partsInventory[i] == null)
            {
                partsInventory[i] = _part;
                return;
            }
        }
        partsInventory.Add(_part);
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
        if (Input.GetMouseButtonDown(0))
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
        else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
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

    IEnumerator EventTimer()
    {
        while(true)
        {
            foreach (Transform ship in playerShips)
            {
                ship.GetComponent<VehicleClass>().CheckRations();
            }
            storeMain.Restock();
            yield return new WaitForSeconds(eventTimer);
        }
        
    }

    IEnumerator CheckDepths()
    {
        while(true)
        {
            if (nextDepth >= depthMarkers.Count)
            {
                yield break;
            }

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
            if (deepestDepth <= depthMarkers[nextDepth].depthLevel && nextDepth < depthMarkers.Count-1)
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
            }else if (nextDepth >= depthMarkers.Count - 1)
            {
                StartCoroutine(DisplayDepthText());
                depthTextsList[1].text = "The Mantle";
                depthTextsList[2].text = "<b>You have Won!</b> However, you are free to keep playing!";
                depthCurrentText.text = "Depth: " + deepestDepth.ToString("F2") + "m";
                depthGoalText.text = "Goals Completed";
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
        yield return new WaitForSeconds(3f);
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

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        confirmQuit.SetActive(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void QuitGameChecked()
    {
        confirmQuit.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

[System.Serializable]
public struct DepthMarker
{
    
    public float depthLevel;
    
    public string depthName;
}
 


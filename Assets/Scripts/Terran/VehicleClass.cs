using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleClass : MonoBehaviour
{
    [SerializeField]
    private ResourceInventoryClass resourceInventory = null;

    [SerializeField]
    private VehicleMovement vehicleMover = null;

    [SerializeField]
    private VehicleDisplayClass vehicleGraphics = null;

    [SerializeField]
    private GameObject selectionSprite = null;

    [Header("Vehicle Stats")]
    [SerializeField]
    private string shipName = "S.S. Unnamed";

    [SerializeField]
    private int health = 10;
    [SerializeField]
    private int healthMax = 10;

    [SerializeField]
    private float inventoryCapacity = 5000;

    [SerializeField]
    private float baseWeight = 50;
    [SerializeField]
    private float weight = 100;

    [SerializeField]
    private float fuel = 60;
    [SerializeField]
    private int fuelMax = 60;

    [SerializeField]
    private float acceleration = 60;

    [SerializeField]
    private float drillSpeed = 1;

    [SerializeField]
    private bool isSelected = false;

    [SerializeField]
    private int cost = 10;

    [SerializeField]
    private int tradeRadius = 2;

    [Header("Crew Stats")]
    [SerializeField]
    WorkerBase[] crew = new WorkerBase[4]; //0 - Captain, 1 - Engineer, 2 - Operator, 3 - Spare Hands
    //bool hasRations = false;
    [SerializeField]
    int crewCount;

    [Header("Vehicle Parts")]
    [SerializeField]
    private PartBody vehicleBase = null;

    [SerializeField]
    private PartDrill drill = null;

    [SerializeField]
    private PartEngine engine = null;

    [SerializeField]
    private PartUpgrade upgrade = null;

    [SerializeField]
    private PartCabin cabin = null;

    [SerializeField]
    private PartWheel wheels = null;

    public string ShipName
    {
        get { return shipName; }
        set { shipName = value; }
    }

    public float Acceleration
    {
        get { return acceleration; }
    }

    public float DrillSpeed
    {
        get { return drillSpeed; }
    }
    public float DrillTier
    {
        get { return drill.Strength; }
    }
    public float DrillEfficiency
    {
        get { return drill.Efficiency; }
    }
    public float DrillFuelEfficiency
    {
        get { return drill.FuelEfficiency; }
    }

    public bool Selected
    {
        get { return isSelected; }
        set { isSelected = value; }
    }

    public float Fuel
    {
        get { return fuel; }
    }

    public int Cost
    {
        get { return cost; }
        set { cost = value; }
    }

    public int CrewCount
    {
        get { return crewCount; }
    }

    public int CrewCountMax
    {
        get
        {
            int maxCrew = 1;
            if (cabin != null)
                maxCrew += cabin.CrewCapacity;

            return maxCrew;
        }
    }

    public WorkerBase[] Crew
    {
        get { return crew; }
    }

    public ResourceInventoryClass Inventory
    {
        get { return resourceInventory; }
    }

    public VehicleMovement MovementScript
    {
        get { return vehicleMover; }
    }

    public VehicleDisplayClass VehicleGraphics
    {
        get { return vehicleGraphics; }
    }
    // Start is called before the first frame update
    void Start()
    {
        RecalculateStats();
        resourceInventory = new ResourceInventoryClass(inventoryCapacity);
        Refuel(fuelMax);
    }

    // Update is called once per frame
    void Update()
    {
        fuel = resourceInventory.GetResourceAmount(ResourceId.Fuel);

        if (selectionSprite != null)
        {
            if (isSelected && !selectionSprite.activeSelf)
            {
                selectionSprite.SetActive(true);
                vehicleGraphics.SetUI(true);
            }
            else if (!isSelected && selectionSprite.activeSelf)
            {
                selectionSprite.SetActive(false);
                vehicleGraphics.SetUI(false);
                vehicleGraphics.DisableTrade();
            }
        }
    }

    float CalculateDrillSpeed()
    {
        return (1 / drill.Strength);
    }

    float CalculateAcceleration()//Calculate acceleration, based on engine power + pilot skills over weight
    {
        if (engine == null) //If there isn't an engine
        {
            Debug.LogWarning(shipName + " doesn't have an engine!");
            return 0f;
        }
        float accelerationPoints = 10; //Base acceleration of 10

        if (crew[0] != null)
        {
            int modifier = 0;
            if (crew[3] != null)
            {
                modifier = crew[3].Motorskills;
            }
            accelerationPoints *= (crew[0].Motorskills + modifier);
        }

        accelerationPoints += engine.Power;

        return accelerationPoints / weight;
    }

    float CalculateWeight()//Calculate weight of the entire ship, includes all parts, base weight, and crew
    {
        float totalWeight = baseWeight;
      
        if (drill != null)
            totalWeight += drill.Weight;
        if (engine != null)
            totalWeight += engine.Weight;
        if (upgrade != null)
            totalWeight += upgrade.Weight;
        if (cabin != null)
            totalWeight += cabin.Weight;
        if (wheels != null)
            totalWeight += wheels.Weight;

        crewCount = 0;

        for (int i = 0; i < crew.Length; i++)
        {
            if (crew[i] == null)
            {
                print("Worker " + i + " is null");
            }
            else if (crew[i] != null)
            {
                crewCount++;
                totalWeight++;
            }
        }

        return totalWeight;
    }

    public void ScanForTrade()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, tradeRadius, transform.forward, tradeRadius, LayerMask.GetMask("Vehicle"));
        if (hits.Length > 1)
        {
            List<VehicleClass> nearbyvehicles = new List<VehicleClass>();
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.gameObject.GetComponent<VehicleClass>() != this)
                    nearbyvehicles.Add(hits[i].transform.gameObject.GetComponent<VehicleClass>());
            }
            vehicleGraphics.EnableTrade(nearbyvehicles.ToArray());
        }
    }

    public void CreateSelf(VehicleClass _vehicle)
    {
        shipName = _vehicle.ShipName;
        _vehicle.GetPart(out vehicleBase);
        _vehicle.GetPart(out cabin);
        _vehicle.GetPart(out drill);
        _vehicle.GetPart(out engine);
        _vehicle.GetPart(out wheels);
        _vehicle.GetPart(out upgrade);
        crew = new WorkerBase[4];
        crew[0] = _vehicle.crew[0];
        cost = _vehicle.cost;
    }

    public int GetCost(float _multiplier)
    {
        int partCost = 0;
        if (vehicleBase != null)
            partCost += vehicleBase.Cost;
        if (cabin != null)
            partCost += cabin.Cost;
        if (drill != null)
            partCost += drill.Cost;
        if (engine != null)
            partCost += engine.Cost;
        if (upgrade != null)
            partCost += upgrade.Cost;
        if (wheels != null)
            partCost += wheels.Cost;
        return Mathf.RoundToInt(partCost * _multiplier);
    }
    
    public void UseFuel(float fuelModifier)
    {
        float crewModifier = 1;
        int spareHandsAddition = 0;
        if (crew[3] != null)
            spareHandsAddition = Mathf.RoundToInt((float)crew[3].Engineering * 0.5f);
        if (crew[1] != null)
        {
            float skillLevel = crew[1].Engineering + spareHandsAddition;
            if (skillLevel <= 5 && skillLevel > 1)
            {
                float x1 = 8.5132f * Mathf.Pow(10, -4) * Mathf.Pow(skillLevel, 3);
                float x2 = -2.554f * Mathf.Pow(10, -3) * Mathf.Pow(skillLevel, 2);
                float x3 = -1.3607f * Mathf.Pow(10, -1) * Mathf.Pow(skillLevel, 1) + 1.1378f;
                //print("x1: " + x1 + ", x2: " + x2 + ", x3" + x3);
                crewModifier = x1 + x2 + x3;
            }
            else if (skillLevel > 5 && skillLevel <= 10)
            {
                float x1 = -5.3285f * Mathf.Pow(10, -4) * Mathf.Pow(skillLevel, 3);
                float x2 = 1.8209f * Mathf.Pow(10, -2) * Mathf.Pow(skillLevel, 2);
                float x3 = -2.3988f * Mathf.Pow(10, -1) * Mathf.Pow(skillLevel, 1) + 1.3108f;
                //print("x1: " + x1 + ", x2: " + x2 + ", x3" + x3);
                crewModifier = x1 + x2 + x3;
            }else if (skillLevel > 10)
            {
                float x1 = -4.94f * Mathf.Pow(10, -5) * Mathf.Pow(skillLevel, 3);
                float x2 = 3.705f * Mathf.Pow(10, -3) * Mathf.Pow(skillLevel, 2);
                float x3 = -9.4844f * Mathf.Pow(10, -2) * Mathf.Pow(skillLevel, 1) + 0.82734f;
                //print("x1: " + x1 + ", x2: " + x2 + ", x3" + x3);
                crewModifier = x1 + x2 + x3;
            }
            print("Crew Fuel Consumption Multiplier is: " + crewModifier);
        }
        resourceInventory.RemoveResource(ResourceId.Fuel, fuelModifier * crewModifier);
        vehicleGraphics.PlayExhaust();
    }

    public void Refuel(float fuelAmount)
    {
        resourceInventory.AddResource(ResourceId.Fuel, fuelAmount);
    }

    public void RecalculateStats()
    {
        if (vehicleBase != null)
        {
            if (inventoryCapacity != vehicleBase.Capacity)
            {
                inventoryCapacity = vehicleBase.Capacity;
                resourceInventory = new ResourceInventoryClass(inventoryCapacity);
            }
            healthMax = vehicleBase.Health;
            baseWeight = vehicleBase.Weight;
        }
        weight = CalculateWeight();
        acceleration = CalculateAcceleration();
        if (drill != null)
            drillSpeed = CalculateDrillSpeed();

        if (vehicleMover != null)
            vehicleMover.CheckDrillRange(drill);

        if (vehicleGraphics != null)
            vehicleGraphics.RefreshUI();
    }

    public void AssignWorker(WorkStation _station, WorkerBase _worker)
    {
        if (crewCount < CrewCountMax || _worker == null)
        {
            switch (_station)
            {
                case WorkStation.None:
                    return;
                case WorkStation.Cabin:
                    crew[0] = _worker;
                    break;
                case WorkStation.Engine:
                    if (cabin != null)
                    {
                        crew[1] = _worker;
                    }
                    break;
                case WorkStation.Drill:
                    if (cabin != null)
                    {
                        crew[2] = _worker;
                    }
                    break;
                case WorkStation.Spare:
                    if (cabin != null)
                    {
                        crew[3] = _worker;
                    }
                    break;
            }
        }
        if (_worker != null)
        {
            _worker.Station = _station;
        }else
        {
            crewCount--;
            print("Crew is" + CrewCount + "/" + CrewCountMax);
        }
        RecalculateStats();
    }

    public void AssignWorker(int _stationIndex, WorkerBase _worker)
    {
        if (crewCount < CrewCountMax || _worker == null)
        {
            switch (_stationIndex)
            {
                case 0:
                    crew[0] = _worker;
                    if (_worker != null)
                        _worker.Station = WorkStation.Cabin;
                    break;
                case 1:
                    if (cabin != null)
                    {
                        crew[1] = _worker;
                        if (_worker != null)
                            _worker.Station = WorkStation.Engine;
                    }
                    break;
                case 2:
                    if (cabin != null)
                    {
                        crew[2] = _worker;
                        if (_worker != null)
                            _worker.Station = WorkStation.Drill;
                    }
                    break;
                case 3:
                    if (cabin != null)
                    {
                        crew[3] = _worker;
                        if (_worker != null)
                            _worker.Station = WorkStation.Spare;
                    }
                    break;
            }
        }
        if (_worker == null)
        {
            crewCount--;
            print("Crew is" + CrewCount + "/" + CrewCountMax);
        }
        RecalculateStats();
    }

    void KillWorker(WorkerBase _worker)
    {
        int station = 0;
        for (station = 0; station < crew.Length; station++)
        {
            if(crew[station] == _worker)
            {
                AssignWorker(station, null);
                break;
            }
        }
        if (station == 0 && crewCount > 0)
        {
            for (station = crew.Length-1; station >= 0; station--)
            {
                if (crew[station] != null)
                {
                    AssignWorker(WorkStation.Cabin, crew[station]);
                    AssignWorker(station, null);
                    return;
                }
            }
        }
    }

    void ModifyCrewCount(WorkerBase _worker, int _crewPosition)
    {
        if (crew[_crewPosition] == null && _worker != null)
        {
            crewCount++;
        }
        else if (_worker == null)
        {
            crewCount--;
        }
    }

    public bool CanAssignWorker(WorkStation _station)
    {
        switch (_station)
        {
            case WorkStation.None:
                return false;
            case WorkStation.Cabin:
                return true;
            case WorkStation.Engine:
                if (cabin != null && crewCount < CrewCountMax)
                {
                    return true;
                }
                break;
            case WorkStation.Drill:
                if (cabin != null && crewCount < CrewCountMax)
                {
                    return true;
                }
                break;
            case WorkStation.Spare:
                if (cabin != null && crewCount < CrewCountMax)
                {
                    return true;
                }
                break;
        }
        return false;
    }

    public WorkerBase GetWorker(WorkStation _station)
    {
        switch (_station)
        {
            case WorkStation.None:
                Debug.LogWarning("Returning Null Station");
                return null;
            case WorkStation.Cabin:
                return crew[0];
            case WorkStation.Engine:
                return crew[1];
            case WorkStation.Drill:
                return crew[2];
            case WorkStation.Spare:
                return crew[3];
        }
        return null;
    }

    public void InstallPart(PartBody _part)
    {
        vehicleBase = _part;
        inventoryCapacity = vehicleBase.Capacity;
        healthMax = vehicleBase.Health;
        baseWeight = vehicleBase.Weight;
    }

    public void InstallPart(PartDrill _part)
    {
        drill = _part;
        RecalculateStats();
    }

    public void InstallPart(PartCabin _part)
    {
        cabin = _part;
        RecalculateStats();
    }

    public void InstallPart(PartEngine _part)
    {
        engine = _part;
        RecalculateStats();
    }

    public void InstallPart(PartWheel _part)
    {
        wheels = _part;
        RecalculateStats();
    }

    public void InstallPart(PartUpgrade _part)
    {
        upgrade = _part;
        RecalculateStats();
    }

    public void GetPart(out PartBody _part)
    {
        _part = vehicleBase;
    }

    public void GetPart(out PartCabin _part)
    {
        _part = cabin;
    }

    public void GetPart(out PartDrill _part)
    {
        _part = drill;
    }

    public void GetPart(out PartEngine _part)
    {
        _part = engine;
    }

    public void GetPart(out PartWheel _part)
    {
        _part = wheels;
    }

    public void GetPart(out PartUpgrade _part)
    {
        _part = upgrade;
    }

    public bool HasPart(PartType _partType)
    {
        switch (_partType)
        {
            case PartType.Drill:
                if (drill != null)
                    return true;
                break;
            case PartType.Cabin:
                if (cabin != null)
                    return true;
                break;
            case PartType.Engine:
                if (engine != null)
                    return true;
                break;
            case PartType.Wheels:
                if (wheels != null)
                    return true;
                break;
            case PartType.Upgrade:
                if (upgrade != null)
                    return true;
                break;
        }
        return false;
    }

    public bool PartExists(PartType _part)
    {
        switch (_part)
        {
            case PartType.Drill:
                if (drill != null)
                    return true;
                break;
            case PartType.Cabin:
                if (cabin != null)
                    return true;
                break;
            case PartType.Engine:
                if (engine != null)
                    return true;
                break;
            case PartType.Wheels:
                if (wheels != null)
                    return true;
                break;
            case PartType.Upgrade:
                if (upgrade != null)
                    return true;
                break;
        }
        return false;
    }

    public void CheckRations()
    {
        if (Vector2.Distance(FindObjectOfType<ShopController>().transform.position, this.transform.position) > 5)
        {
            foreach (WorkerBase worker in crew)
            {
                if (worker != null)
                {
                    if (worker.Gender != Gender.Other && resourceInventory.GetResourceAmount(ResourceId.Rations) > 0) // If not a mutant or robot
                    {
                        worker.Eat();
                        resourceInventory.RemoveResource(ResourceId.Rations, 1);
                    }
                    else if (worker.Gender != Gender.Other && resourceInventory.GetResourceAmount(ResourceId.Rations) <= 0) //If no rations
                    {
                        worker.Starve();
                        if (worker.Dead)
                        {
                            KillWorker(worker);
                        }
                    }
                }
            }
        }
    }
}

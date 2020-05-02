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

    [Header("Crew Stats")]
    [SerializeField]
    WorkerBase[] crew = new WorkerBase[3]; //0 - Captain, 1 - Operator, 2 - Engineer
    //bool hasRations = false;

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

    public ResourceInventoryClass Inventory
    {
        get { return resourceInventory; }
    }

    public VehicleMovement MovementScript
    {
        get { return vehicleMover; }
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
            }
            else if (!isSelected && selectionSprite.activeSelf)
            {
                selectionSprite.SetActive(false);
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
            accelerationPoints *= crew[0].Motorskills;
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
        for (int i = 0; i < crew.Length; i++)
        {
            if (crew[i] == null)
            {
                print("Worker " + i + " is null");
            }
            else if (crew[i] != null)
            {
                totalWeight++;
            }
        }

        return totalWeight;
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
        resourceInventory.RemoveResource(ResourceId.Fuel, fuelModifier);
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
}
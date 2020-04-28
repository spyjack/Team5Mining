﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleClass : MonoBehaviour
{
    [SerializeField]
    private ResourceInventoryClass resourceInventory = null;

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
    // Start is called before the first frame update
    void Start()
    {
        RecalculateStats();
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

        if (Input.GetKeyDown(KeyCode.Insert))
        {
            int num = Random.Range(0, 6);
            int randAmount = Random.Range(10, 60);
            switch (num)
            {
                case 0:
                    resourceInventory.AddResource(ResourceId.Fuel, randAmount);
                    print("Added " + randAmount + " " + ResourceId.Fuel + " to " + name);
                    break;
                case 1:
                    resourceInventory.AddResource(ResourceId.Dirt, randAmount);
                    print("Added " + randAmount + " " + ResourceId.Dirt + " to " + name);
                    break;
                case 2:
                    resourceInventory.AddResource(ResourceId.Stone, randAmount);
                    print("Added " + randAmount + " " + ResourceId.Stone + " to " + name);
                    break;
                case 3:
                    resourceInventory.AddResource(ResourceId.Iron, randAmount);
                    print("Added " + randAmount + " " + ResourceId.Iron + " to " + name);
                    break;
                case 4:
                    resourceInventory.AddResource(ResourceId.Copper, randAmount);
                    print("Added " + randAmount + " " + ResourceId.Copper + " to " + name);
                    break;
                case 5:
                    resourceInventory.AddResource(ResourceId.Gold, randAmount);
                    print("Added " + randAmount + " " + ResourceId.Gold + " to " + name);
                    break;
            }

        }
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
            inventoryCapacity = vehicleBase.Capacity;
            healthMax = vehicleBase.Health;
            baseWeight = vehicleBase.Weight;
        }
        resourceInventory = new ResourceInventoryClass(inventoryCapacity);
        weight = CalculateWeight();
        acceleration = CalculateAcceleration();
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
}

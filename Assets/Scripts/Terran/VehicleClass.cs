using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleClass : MonoBehaviour
{
    [SerializeField]
    private ResourceInventoryClass resourceInventory = null;

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

    [Header("Crew Stats")]
    [SerializeField]
    WorkerBase[] crew = new WorkerBase[3]; //0 - Captain, 1 - Operator, 2 - Engineer
    bool hasRations = false;

    [Header("Vehicle Parts")]
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
    // Start is called before the first frame update
    void Start()
    {
        resourceInventory = new ResourceInventoryClass(inventoryCapacity);
        weight = CalculateWeight();
        acceleration = CalculateAcceleration();
        Refuel(fuelMax);
    }

    // Update is called once per frame
    void Update()
    {
        checkForSelection();
        fuel = resourceInventory.GetResourceAmount(ResourceId.Fuel);

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

    void checkForSelection()
    {
        if (Input.GetMouseButtonDown(1))
        { 
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.GetComponent<VehicleClass>() != null)
            {
                VehicleClass ship = hit.collider.gameObject.GetComponent<VehicleClass>();
                if (ship.Selected)
                {
                    ship.Selected = false;
                }else
                {
                    ship.Selected = true;
                }
            }
        }else if(Input.GetKeyDown(KeyCode.Escape))
        {
            Selected = false;
        }
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
        weight = CalculateWeight();
        acceleration = CalculateAcceleration();
    }
}

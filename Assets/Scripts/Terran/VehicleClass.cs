using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleClass : MonoBehaviour
{

    [Header("Vehicle Stats")]
    [SerializeField]
    private string shipName = "S.S. Unnamed";

    [SerializeField]
    private int health = 10;
    [SerializeField]
    private int healthMax = 10;

    [SerializeField]
    private float baseWeight = 50;
    [SerializeField]
    private float weight = 100;

    [SerializeField]
    private int fuel = 60;
    [SerializeField]
    private int fuelMax = 60;

    [SerializeField]
    private float acceleration = 60;

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
    // Start is called before the first frame update
    void Start()
    {
        weight = CalculateWeight();
        acceleration = CalculateAcceleration();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void RecalculateStats()
    {
        weight = CalculateWeight();
        acceleration = CalculateAcceleration();
    }
}

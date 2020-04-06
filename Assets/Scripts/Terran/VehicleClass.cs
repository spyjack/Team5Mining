using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleClass : MonoBehaviour
{

    [Header("Vehicle Stats")]
    [SerializeField]
    private int health = 10;
    [SerializeField]
    private int healthMax = 10;

    [SerializeField]
    private int fuel = 60;
    [SerializeField]
    private int fuelMax = 60;

    [Header("Crew Stats")]
    [SerializeField]
    WorkerBase[] crew = new WorkerBase[3]; 
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

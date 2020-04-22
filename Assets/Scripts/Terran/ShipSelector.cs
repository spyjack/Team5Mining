using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipSelector : MonoBehaviour
{
    [SerializeField]
    VehicleClass vehicleIndex = null;

    [SerializeField]
    Text tabText = null;

    [SerializeField]
    string tabName = "Unnamed";

    public VehicleClass Vehicle
    {
        get { return vehicleIndex; }
        set { vehicleIndex = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        tabName = vehicleIndex.ShipName;
        tabText.text = tabName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

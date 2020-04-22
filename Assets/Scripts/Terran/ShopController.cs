using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [SerializeField]
    PlayerController player = null;

    [SerializeField]
    List<VehicleClass> dockedShips = new List<VehicleClass>();

    [Header("Resources Store Variables")]
    [SerializeField]
    VehicleClass selectedVehicle = null;

    [Header("Resources Store UI")]

    [SerializeField]
    private List<GameObject> shipSellectors = new List<GameObject>();

    [SerializeField]
    private Text currentCapacityText = null;

    [SerializeField]
    private Text maxCapacityText = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Vehicle")
        {
            DockShip(other.transform.GetComponent<VehicleClass>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Vehicle")
        {
            UndockShip(other.transform.GetComponent<VehicleClass>());
        }
    }

    public void SelectShipTab()
    {

    }

    void AddShipSelector(VehicleClass vehicle)
    {

    }

    void DockShip(VehicleClass _vehicle)
    {
        dockedShips.Add(_vehicle);
        foreach (GameObject sellector in shipSellectors)
        {
            ShipSellector shipSellect = sellector.GetComponent<ShipSellector>();
            if (shipSellect.Vehicle == _vehicle)
            {
                sellector.SetActive(true);
            }
        }
    }

    void UndockShip(VehicleClass _vehicle)
    {
        dockedShips.Remove(_vehicle);
        foreach (GameObject sellector in shipSellectors)
        {
            ShipSellector shipSellect = sellector.GetComponent<ShipSellector>();
            if (shipSellect.Vehicle == _vehicle)
            {
                sellector.SetActive(false);
            }
        }
    }
}

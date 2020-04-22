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

    [SerializeField]
    GameObject shipSelectorPrefab = null;

    [Header("Resources Store UI")]

    [SerializeField]
    private List<GameObject> shipSelectors = new List<GameObject>();

    [SerializeField]
    private Text currentCapacityText = null;

    [SerializeField]
    private Text maxCapacityText = null;

    [SerializeField]
    private GameObject dockedShipsGroup = null;
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

    void AddShipSelector(VehicleClass _vehicle)
    {
        GameObject newSelector = Instantiate(shipSelectorPrefab, dockedShipsGroup.transform);
        newSelector.GetComponent<ShipSelector>().Vehicle = _vehicle;
        shipSelectors.Add(newSelector);
    }

    void DockShip(VehicleClass _vehicle)
    {
        dockedShips.Add(_vehicle);
        foreach (GameObject sellector in shipSelectors)
        {
            ShipSelector shipSellect = sellector.GetComponent<ShipSelector>();
            if (shipSellect.Vehicle == _vehicle)
            {
                sellector.SetActive(true);
                return;
            }
        }
        AddShipSelector(_vehicle);
    }

    void UndockShip(VehicleClass _vehicle)
    {
        dockedShips.Remove(_vehicle);
        foreach (GameObject sellector in shipSelectors)
        {
            ShipSelector shipSellect = sellector.GetComponent<ShipSelector>();
            if (shipSellect.Vehicle == _vehicle)
            {
                sellector.SetActive(false);
            }
        }
    }
}

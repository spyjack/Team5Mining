using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ShipUIConnector : MonoBehaviour
{
    [SerializeField]
    Text shipName = null;

    [SerializeField]
    Text workerListTitle = null;

    [SerializeField]
    List<GenericUIIcon> equippedWorkerIcons = new List<GenericUIIcon>();

    [SerializeField]
    List<GenericUIIcon> installedPartsIcons = new List<GenericUIIcon>();

    [SerializeField]
    Text invCapacityText = null;

    [SerializeField]
    Text invFuelText = null;

    [SerializeField]
    Text invRationsText = null;

    [SerializeField]
    Text miningModeText = null;

    [Header("Trading Window")]
    [SerializeField]
    GameObject tradeMenu = null;

    [SerializeField]
    GameObject selfInvMenu = null;

    [SerializeField]
    GameObject disableTradeButton = null;

    [SerializeField]
    Dropdown tradeOptionsDropdown = null;

    [SerializeField]
    Text tradingShipNameText = null;

    [SerializeField]
    VehicleClass[] vehiclesToTrade = new VehicleClass[1];

    public VehicleClass referenceVehicle = null;
    public VehicleClass vehicleToTransfer = null;

    List<VehicleResourceListConnector> referenceVehicleInvList = new List<VehicleResourceListConnector>();
    List<VehicleResourceListConnector> vehicleToTransferInvList = new List<VehicleResourceListConnector>();

    [SerializeField]
    GameObject vehicleToTransferInvHolder = null;
    [SerializeField]
    GameObject referenceVehicleInvHolder = null;

    [SerializeField]
    GameObject resourceInvPrefab = null;

    int dropdownChosen = 0;

    private void Start()
    {
        tradeOptionsDropdown.onValueChanged.AddListener(delegate {
            GetDropdownValue(tradeOptionsDropdown);
        });

        DisableTrade();
    }

    private void Update()
    {
        if (tradeMenu.activeSelf && Vector2.Distance(referenceVehicle.transform.position, vehicleToTransfer.transform.position) > 5)
        {
            DisableTrade();
        }
    }

    public void RefreshUI(VehicleClass vehicle)
    {
        shipName.text = vehicle.ShipName;

        UpdateParts(vehicle);

        UpdateWorkers(vehicle);

        RefreshInventoryUI(vehicle);
    }

    public void RefreshInventoryUI(VehicleClass vehicle)
    {
        invCapacityText.text = "Capacity Used: " + vehicle.Inventory.UsedCapacity.ToString("F2") + "/" + vehicle.Inventory.Capacity;
        invFuelText.text = "Fuel Remaining: " + vehicle.Inventory.GetFuelAmount();
        invRationsText.text = "Rations Remaining: " + vehicle.Inventory.GetResourceAmount(ResourceId.Rations);
        if (vehicle.MovementScript.IsMining && miningModeText.text == "Mining Mode (M): Disabled")
        {
            miningModeText.text = "Mining Mode (M): Active";
        } else if (!vehicle.MovementScript.IsMining && miningModeText.text == "Mining Mode (M): Active")
        {
            miningModeText.text = "Mining Mode (M): Disabled";
        }
    }

    public int GetDropdownValue(Dropdown dropdown)
    {
        print("dropdown changed");
        dropdownChosen = dropdown.value;
        vehicleToTransfer = vehiclesToTrade[dropdown.value];
        RefreshTradeInventory();
        return dropdown.value;
    }

    public void RefreshTradeInventory()
    {
        tradingShipNameText.text = vehicleToTransfer.ShipName;

        RefreshTradeItems(vehicleToTransferInvList, vehicleToTransfer);
        RefreshTradeItems(referenceVehicleInvList, referenceVehicle);
    }

    public void RefreshTradeItems(List<VehicleResourceListConnector> vehicleInvList, VehicleClass vehicle)
    {
        foreach (ResourceId resourceId in Enum.GetValues(typeof(ResourceId)))
        {
            bool addResource = true;
            foreach (VehicleResourceListConnector connector in vehicleInvList)
            {
                if (connector.resource == resourceId)
                {
                    addResource = false;
                    RefreshResource(resourceId, vehicle, connector);
                    break;
                }   
            }
            if (addResource)
                AddNewResource(resourceId, vehicleInvList, vehicle);
        }

    }

    void AddNewResource(ResourceId _resource, List<VehicleResourceListConnector> vehicleInvList, VehicleClass vehicle)
    {
        Transform spawnPosition = null;
        if (vehicle == vehicleToTransfer)
        {
            spawnPosition = vehicleToTransferInvHolder.transform;
        }
        else { spawnPosition = referenceVehicleInvHolder.transform; }

        if (vehicle.Inventory.CheckForResource(_resource) || _resource == ResourceId.Fuel || _resource == ResourceId.Rations)
        {
            VehicleResourceListConnector connector = Instantiate(resourceInvPrefab, spawnPosition).GetComponent<VehicleResourceListConnector>();
            connector.resource = _resource;
            connector.shipMain = this;
            connector.vehicleInv = vehicle;
            vehicleInvList.Add(connector);
        }
        //RefreshResource(_resource, vehicle, vehicleInvList[vehicleInvList.Count-1]);
    }

    public void RefreshResources(ResourceId _resource)
    {
        foreach (VehicleResourceListConnector connector in vehicleToTransferInvList)
        {
            if (connector.resource == _resource)
            {
                RefreshResource(_resource, vehicleToTransfer, connector);
                break;
            }
        }
        foreach (VehicleResourceListConnector connector in referenceVehicleInvList)
        {
            if (connector.resource == _resource)
            {
                RefreshResource(_resource, referenceVehicle, connector);
                break;
            }
        }
    }

    void RefreshResource(ResourceId _resource, VehicleClass _vehicle, VehicleResourceListConnector _connector)
    {
        if (_vehicle.Inventory.CheckForResource(_resource) || _resource == ResourceId.Fuel || _resource == ResourceId.Rations)
        {
            _connector.gameObject.SetActive(true);

            _connector.resourceName.text = _vehicle.Inventory.GetResourceName(_resource);
            _connector.resourceQuantity.text = _vehicle.Inventory.GetResourceAmount(_resource).ToString("F2");
            _connector.resourceCapacity.text = _vehicle.Inventory.GetResourceCapacity(_resource).ToString("F2");
            _connector.shipMain = this;
        }
        else if (!_vehicle.Inventory.CheckForResource(_resource))
        {
            _connector.gameObject.SetActive(false);
        }
    }

    public void EnableTrade(VehicleClass baseVehicle, VehicleClass[] tradingVehicles)
    {
        referenceVehicle = baseVehicle;
        vehiclesToTrade = tradingVehicles;
        tradeMenu.SetActive(true);
        disableTradeButton.SetActive(true);
        selfInvMenu.SetActive(true);
        PopulateDropdown(tradeOptionsDropdown, tradingVehicles);
        vehicleToTransfer = vehiclesToTrade[0];
        RefreshTradeInventory();
        RefreshTradeInventory();
    }

    public void EnableTrade()
    {
        referenceVehicle.ScanForTrade();
    }

    public void DisableTrade()
    {
        tradeMenu.SetActive(false);
        selfInvMenu.SetActive(false);
        disableTradeButton.SetActive(false);
    }

    public void PopulateDropdown(Dropdown dropdown, VehicleClass[] optionsArray)
    {
        List<string> options = new List<string>();
        foreach (VehicleClass option in optionsArray)
        {
            options.Add(option.ShipName); // Or whatever you want for a label
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }

    public void TransferResources(ResourceId _resource, float amount, VehicleClass vehicleComingFrom)
    {
        VehicleClass sendToVehicle = vehicleToTransfer;
        if (vehicleComingFrom == vehicleToTransfer)
        {
            print("sending to original vehicle");
            sendToVehicle = referenceVehicle;
        }


        if (vehicleComingFrom.Inventory.GetResourceAmount(_resource) >= amount && sendToVehicle.Inventory.Capacity - sendToVehicle.Inventory.UsedCapacity >= amount)
        {
            if (vehicleComingFrom.Inventory.GetResourceAmount(_resource) > 0)
            {
                vehicleComingFrom.Inventory.RemoveResource(_resource, amount);
            }
            sendToVehicle.Inventory.AddResource(_resource, amount);
        }
    }

    void UpdateParts(VehicleClass vehicle)
    {
        vehicle.GetPart(out PartCabin _cabin);
        vehicle.GetPart(out PartDrill _drill);
        vehicle.GetPart(out PartWheel _wheels);
        vehicle.GetPart(out PartEngine _engine);
        vehicle.GetPart(out PartUpgrade _upgrade);

        if (_cabin != null && !installedPartsIcons[1].gameObject.activeSelf)
        {
            installedPartsIcons[1].gameObject.SetActive(true);
            installedPartsIcons[1].icon.sprite = _cabin.Icon;
        }
        else if (_cabin == null && installedPartsIcons[1].gameObject.activeSelf) { installedPartsIcons[1].gameObject.SetActive(false); }

        if (_drill != null && !installedPartsIcons[0].gameObject.activeSelf)
        {
            installedPartsIcons[0].gameObject.SetActive(true);
            installedPartsIcons[0].icon.sprite = _drill.Icon;
        }
        else if (_drill == null && installedPartsIcons[0].gameObject.activeSelf) { installedPartsIcons[0].gameObject.SetActive(false); }

        if (_engine != null && !installedPartsIcons[2].gameObject.activeSelf)
        {
            installedPartsIcons[2].gameObject.SetActive(true);
            installedPartsIcons[2].icon.sprite = _engine.Icon;
        }
        else if (_engine == null && installedPartsIcons[2].gameObject.activeSelf) { installedPartsIcons[2].gameObject.SetActive(false); }

        if (_wheels != null && !installedPartsIcons[3].gameObject.activeSelf)
        {
            installedPartsIcons[3].gameObject.SetActive(true);
            installedPartsIcons[3].icon.sprite = _wheels.Icon;
        }
        else if (_wheels == null && installedPartsIcons[3].gameObject.activeSelf) { installedPartsIcons[3].gameObject.SetActive(false); }
    }

    void UpdateWorkers(VehicleClass vehicle)
    {
        for (int cr = 0; cr < vehicle.Crew.Length; cr++)
        {
            if (vehicle.Crew[cr] != null && (equippedWorkerIcons[cr].icon != vehicle.Crew[cr].Portrait || !equippedWorkerIcons[cr].gameObject.activeSelf))
            {
                equippedWorkerIcons[cr].icon.sprite = vehicle.Crew[cr].Portrait;
                equippedWorkerIcons[cr].gameObject.SetActive(true);
            }
            else if (vehicle.Crew[cr] == null && equippedWorkerIcons[cr].gameObject.activeSelf)
            {
                equippedWorkerIcons[cr].gameObject.SetActive(false);
            }
        }
    }
}

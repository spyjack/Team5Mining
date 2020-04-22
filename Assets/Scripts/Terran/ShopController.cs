﻿using System;
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

    [Header("Store Values")]
    [SerializeField]
    List<ResourceSaleItem> resourceSaleItems = new List<ResourceSaleItem>();

    [Header("Resources Store Variables")]
    [SerializeField]
    VehicleClass selectedVehicle = null;

    [SerializeField]
    int selectedVehicleIndex = 0;

    [SerializeField]
    GameObject shipSelectorPrefab = null;

    [SerializeField]
    GameObject resourceListPrefab = null;

    [Header("Resources Store UI")]

    [SerializeField]
    private List<ShipSelector> shipSelectors = new List<ShipSelector>();

    [SerializeField]
    private List<ResourceListConnector> resourceListItems = new List<ResourceListConnector>();

    [SerializeField]
    private Text currentCapacityText = null;

    [SerializeField]
    private Text maxCapacityText = null;

    [SerializeField]
    private GameObject dockedShipsGroup = null;

    [SerializeField]
    private GameObject resourceListGroup = null;

    public VehicleClass SelectedShip
    {
        get { return selectedVehicle; }
    }
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

    public void SelectShipTab(VehicleClass _vehicle)
    {
        for (int i = 0; i < shipSelectors.Count; i++)
        {
            if (shipSelectors[i].Vehicle == _vehicle)
            {
                selectedVehicleIndex = i;
                break;
            }
        }
        selectedVehicle = _vehicle;
        UpdateCapacity();
        CreateResourceList();
    }

    public void SellResource(ResourceId _resource, float _amount)
    {
        if (selectedVehicle.Inventory.CheckForResource(_resource))
        {
            _amount = Mathf.Min(selectedVehicle.Inventory.GetResourceAmount(_resource), _amount);
            player.Money += (int)CalculateValue(_resource, _amount);
            selectedVehicle.Inventory.RemoveResource(_resource, _amount);
            UpdateCapacity();
        }
    }

    void AddShipSelector(VehicleClass _vehicle)
    {
        GameObject newSelector = Instantiate(shipSelectorPrefab, dockedShipsGroup.transform);
        ShipSelector selector = newSelector.GetComponent<ShipSelector>();
        selector.Vehicle = _vehicle;
        selector.Shop = this;
        shipSelectors.Add(selector);
    }

    void DockShip(VehicleClass _vehicle)
    {
        dockedShips.Add(_vehicle);
        foreach (ShipSelector sellector in shipSelectors)
        {
            if (sellector.Vehicle == _vehicle)
            {
                sellector.gameObject.SetActive(true);
                return;
            }
        }
        AddShipSelector(_vehicle);
    }

    void UndockShip(VehicleClass _vehicle)
    {
        dockedShips.Remove(_vehicle);
        foreach (ShipSelector selector in shipSelectors)
        {
            if (selectedVehicle == _vehicle)
                selectedVehicle = null;

            if (selector.Vehicle == _vehicle)
            {
                selector.gameObject.SetActive(false);
                DisableResourceList();
            }
        }
    }

    void CreateResourceList()
    {
        foreach (ResourceId resourceId in Enum.GetValues(typeof(ResourceId)))
        {
            bool addResource = true;
            foreach (ResourceListConnector connector in resourceListItems)
            {
                if (connector.resourceId == resourceId)
                {
                    addResource = false;
                    RefreshResource(resourceId, connector);
                    break;
                }
            }
            if (addResource)
                AddNewResource(resourceId);
        }
    }

    void DisableResourceList()
    {
        foreach (ResourceListConnector connector in resourceListItems)
        {
            connector.gameObject.SetActive(false);
        }
    }

    public void RefreshResource(ResourceId _resource, ResourceListConnector _connector)
    {
        if (selectedVehicle.Inventory.CheckForResource(_resource) || _resource == ResourceId.Fuel || _resource == ResourceId.Rations)
        {
            _connector.gameObject.SetActive(true);            

            _connector.resourceName.text = selectedVehicle.Inventory.GetResourceName(_resource);
            _connector.resourceQuantity.text = selectedVehicle.Inventory.GetResourceAmount(_resource).ToString();
            _connector.resourceCapacity.text = selectedVehicle.Inventory.GetResourceCapacity(_resource).ToString();
            _connector.resourceValue.text = "$" + CalculateValue(_resource);
            _connector.shopMain = this;

            if (_resource == ResourceId.Fuel || _resource == ResourceId.Rations)
            {
                _connector.buyOneButton.SetActive(true);
            }
        }
        else if (!selectedVehicle.Inventory.CheckForResource(_resource))
        {
            _connector.gameObject.SetActive(false);
        }
    }

    void AddNewResource(ResourceId _resource)
    {
        if (selectedVehicle.Inventory.CheckForResource(_resource) || _resource == ResourceId.Fuel || _resource == ResourceId.Rations)
        {
            GameObject newListObject = Instantiate(resourceListPrefab, resourceListGroup.transform);
            ResourceListConnector connector = newListObject.GetComponent<ResourceListConnector>();
            resourceListItems.Add(connector);

            connector.resourceId = _resource;
            connector.resourceName.text = selectedVehicle.Inventory.GetResourceName(_resource);
            connector.resourceQuantity.text = selectedVehicle.Inventory.GetResourceAmount(_resource).ToString();
            connector.resourceCapacity.text = selectedVehicle.Inventory.GetResourceCapacity(_resource).ToString();
            connector.resourceValue.text = "$" + CalculateValue(_resource);
            connector.shopMain = this;

            if (_resource == ResourceId.Fuel || _resource == ResourceId.Rations)
            {
                connector.buyOneButton.SetActive(true);
            }
        }
    }

    void UpdateCapacity()
    {
        currentCapacityText.text = selectedVehicle.Inventory.UsedCapacity.ToString();
        maxCapacityText.text = selectedVehicle.Inventory.Capacity.ToString();
    }

    float CalculateValue(ResourceId _resource)
    {
        for (int i = 0; i < resourceSaleItems.Count; i++)
        {
            if (resourceSaleItems[i].resource == _resource)
            {
                return (resourceSaleItems[i].salePricerPerUnit * selectedVehicle.Inventory.GetResourceAmount(_resource)) * resourceSaleItems[i].saleModifier;
            }
        }
        return 0f;
    }

    float CalculateValue(ResourceId _resource, float _amount)
    {
        for (int i = 0; i < resourceSaleItems.Count; i++)
        {
            if (resourceSaleItems[i].resource == _resource)
            {
                return (resourceSaleItems[i].salePricerPerUnit * _amount) * resourceSaleItems[i].saleModifier;
            }
        }
        return 0f;
    }
}

[System.Serializable]
public struct ResourceSaleItem
{
    public ResourceId resource;
    public float salePricerPerUnit;
    public float buyPricerPerUnit;
    public float saleModifier;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleResourceListConnector : MonoBehaviour
{
    public ResourceId resource;

    public Text resourceName = null;

    public Text resourceQuantity = null;

    public Text resourceCapacity = null;

    public ShipUIConnector shipMain = null;

    public VehicleClass vehicleInv = null;

    public void TransferAllClicked()
    {
        shipMain.TransferResources(resource, vehicleInv.Inventory.GetResourceAmount(resource), vehicleInv);
        shipMain.RefreshResources(resource);
    }

    public void TransferClicked()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            shipMain.TransferResources(resource, 10, vehicleInv);
            shipMain.RefreshResources(resource);
        }
        else
        {
            shipMain.TransferResources(resource, 1, vehicleInv);
            shipMain.RefreshResources(resource);
        }
        
    }
}

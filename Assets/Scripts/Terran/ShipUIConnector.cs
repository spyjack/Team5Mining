using System.Collections;
using System.Collections.Generic;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartItemConnector : MonoBehaviour
{
    public Text partName = null;

    public Image partIcon = null;

    public GameObject purchasedOverlay = null;

    public Button selectItemButton = null;

    public Button purchaseItemButton = null;

    public ShopController shopMain = null;

    public bool isPurchased = false;

    [Header("Part")]
    public PartBase partItem = null;

    private void Start()
    {
        SetUpButton();
        partName.text = partItem.PartName;
        partIcon.sprite = partItem.Icon;
    }

    void SetUpButton()
    {
        if (partItem is PartCabin)
        {
            selectItemButton.onClick.AddListener(OnCabinItemClick);
        }
        else if (partItem is PartDrill)
        {
            selectItemButton.onClick.AddListener(OnDrillItemClick);
        }
        else if (partItem is PartEngine)
        {
            selectItemButton.onClick.AddListener(OnEngineItemClick);
        }
        else if (partItem is PartWheel)
        {
            selectItemButton.onClick.AddListener(OnWheelItemClick);
        }
    }

    public void OnCabinItemClick()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            shopMain.PurchasePart(this);
        }
        shopMain.ClearPartAttributes();
        shopMain.SelectPart(this);
        PartCabin _cabin = (PartCabin)partItem;
        shopMain.AddPartAttribute("Cabin Type:", _cabin.PartName);
        shopMain.AddPartAttribute("Crew Capacity:", _cabin.CrewCapacity.ToString());
        shopMain.AddPartAttribute("Fuel Efficiency:", _cabin.FuelEfficiency.ToString());
        shopMain.AddPartAttribute("Weight:", _cabin.Weight.ToString());
    }

    public void OnDrillItemClick()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            shopMain.PurchasePart(this);
        }
        shopMain.ClearPartAttributes();
        shopMain.SelectPart(this);
        PartDrill _drill = (PartDrill)partItem;
        shopMain.AddPartAttribute("Drill Type:", _drill.PartName);
        shopMain.AddPartAttribute("Drill Power:", _drill.Strength.ToString());
        shopMain.AddPartAttribute("Drill Efficieny:", _drill.Efficiency.ToString());
        shopMain.AddPartAttribute("Fuel Efficiency:", _drill.FuelEfficiency.ToString());
        shopMain.AddPartAttribute("Weight:", _drill.Weight.ToString());
    }

    public void OnWheelItemClick()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            shopMain.PurchasePart(this);
        }
        shopMain.ClearPartAttributes();
        shopMain.SelectPart(this);
        PartWheel _wheel = (PartWheel)partItem;
        shopMain.AddPartAttribute("Wheel Type:", _wheel.PartName);
        shopMain.AddPartAttribute("Traction:", _wheel.Traction.ToString());
        shopMain.AddPartAttribute("Fuel Efficiency:", _wheel.FuelEfficiency.ToString());
        shopMain.AddPartAttribute("Weight:", _wheel.Weight.ToString());
    }

    public void OnEngineItemClick()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            shopMain.PurchasePart(this);
        }
        shopMain.ClearPartAttributes();
        shopMain.SelectPart(this);
        PartEngine _engine = (PartEngine)partItem;
        shopMain.AddPartAttribute("Engine Type:", _engine.PartName);
        shopMain.AddPartAttribute("Horsepower:", _engine.Power.ToString());
        shopMain.AddPartAttribute("Speed Amplifier:", _engine.Speed.ToString());
        shopMain.AddPartAttribute("Fuel Efficiency:", _engine.FuelEfficiency.ToString());
        shopMain.AddPartAttribute("Weight:", _engine.Weight.ToString());
    }
}

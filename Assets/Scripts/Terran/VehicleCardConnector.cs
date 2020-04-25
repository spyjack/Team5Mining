using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleCardConnector : MonoBehaviour
{
    public Text shipNameText = null;

    public List<Image> vehicleVisualParts = new List<Image>();

    public GameObject soldOutOverlay = null;

    public GameObject shipStatList = null;

    public Button purchaseVehicleButton = null;

    public Text vehicleCostText = null;

    public ShopController shopMain = null;

    public VehicleClass vehicle = null;

    public float costMultiplier = 1;

    public bool isPurchased = false;

    private void Start()
    {
        SetUpButton();
    }

    void SetUpButton()
    {
        purchaseVehicleButton.onClick.AddListener(OnClickPurchaseVehicle);
    }

    void OnClickPurchaseVehicle()
    {
        shopMain.PurchaseVehicle(this);
    }
}

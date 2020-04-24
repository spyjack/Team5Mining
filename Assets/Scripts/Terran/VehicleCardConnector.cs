using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleCardConnector : MonoBehaviour
{
    public Text shipNameText = null;

    public List<Image> vehicleVisualParts = new List<Image>();

    public GameObject shipStatList = null;

    public Button purchaseVehicleButton = null;

    public Text vehicleCostText = null;

    public ShopController shopMain = null;

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
        print("Use Shop Purchase Vehicle");
    }
}

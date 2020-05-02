using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipSelector : MonoBehaviour
{
    [SerializeField]
    VehicleClass vehicleIndex = null;

    [SerializeField]
    ShopController shopMain = null;

    [SerializeField]
    Text tabText = null;

    [SerializeField]
    Button selectButton = null;

    [SerializeField]
    public string tabName = "Unnamed";

    public VehicleClass Vehicle
    {
        get { return vehicleIndex; }
        set { vehicleIndex = value; }
    }

    public ShopController Shop
    {
        get { return shopMain; }
        set { shopMain = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        tabName = vehicleIndex.ShipName;
        tabText.text = tabName;
        SetUpTab();
    }

    void SetUpTab()
    {
        selectButton.onClick.AddListener(SelectTab);
    }

    void SelectTab()
    {
        this.transform.localScale = new Vector3(1, 1.1f, 1);
        shopMain.SelectShipTab(vehicleIndex);
    }

    public void RefreshName()
    {
        tabName = vehicleIndex.ShipName;
        tabText.text = tabName;
    }
}

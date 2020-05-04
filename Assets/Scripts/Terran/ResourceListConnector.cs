using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceListConnector : MonoBehaviour
{
    public ResourceId resource;

    public Text resourceName = null;

    public Text resourceQuantity = null;

    public Text resourceCapacity = null;

    public Text resourceValue = null;

    public GameObject buyOneButton = null;

    public ShopController shopMain = null;

    public void SellAllClicked()
    {
        shopMain.SellResource(resource, shopMain.SelectedShip.Inventory.GetResourceAmount(resource));
        shopMain.RefreshResource(resource, this);
    }

    public void SellClicked()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            shopMain.SellResource(resource, 10);
            shopMain.RefreshResource(resource, this);
        }
        else
        {
            shopMain.SellResource(resource, 1);
            shopMain.RefreshResource(resource, this);
        }
        
    }

    public void BuyClicked()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            shopMain.BuyResource(resource, 10);
            shopMain.RefreshResource(resource, this);
        }
        else
        {
            shopMain.BuyResource(resource, 1);
            shopMain.RefreshResource(resource, this);
        }
    }
}

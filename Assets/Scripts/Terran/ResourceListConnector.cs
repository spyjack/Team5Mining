using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceListConnector : MonoBehaviour
{
    public ResourceId resourceId;

    public Text resourceName = null;

    public Text resourceQuantity = null;

    public Text resourceCapacity = null;

    public Text resourceValue = null;

    public GameObject buyOneButton = null;

    public ShopController shopMain = null;

    public void SellAllClicked()
    {
        shopMain.SellResource(resourceId, shopMain.SelectedShip.Inventory.GetResourceAmount(resourceId));
        shopMain.RefreshResource(resourceId, this);
    }

    public void SellClicked()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            shopMain.SellResource(resourceId, 10);
            shopMain.RefreshResource(resourceId, this);
        }
        else
        {
            shopMain.SellResource(resourceId, 1);
            shopMain.RefreshResource(resourceId, this);
        }
        
    }

    public void BuyClicked()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            shopMain.BuyResource(resourceId, 10);
            shopMain.RefreshResource(resourceId, this);
        }
        else
        {
            shopMain.BuyResource(resourceId, 1);
            shopMain.RefreshResource(resourceId, this);
        }
    }
}

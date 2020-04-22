using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField]
    PlayerController player = null;

    [SerializeField]
    Text debugText = null;

    bool sell = false;
    bool buy = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            sell = true;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            buy = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Vehicle")
        {
            debugText.text = "$";
            VehicleClass vehicle = other.transform.GetComponent<VehicleClass>();
            if (sell)
            {
                debugText.text = "$$$";
                debugText.color = Color.yellow;
                ResourceId[] resourceIds = new ResourceId[6] { ResourceId.Dirt, ResourceId.Copper, ResourceId.Gold, ResourceId.Iron, ResourceId.Stone, ResourceId.Lead};
                foreach(ResourceId resourceId in resourceIds)
                {
                    if (vehicle.Inventory.CheckForResource(resourceId))
                    {
                        player.Money += (int)vehicle.Inventory.GetResourceAmount(resourceId);
                        vehicle.Inventory.RemoveResource(resourceId);
                    }
                }
                sell = false;
            }

            if (buy)
            {
                debugText.text = "Gas$";
                debugText.color = Color.white;
                if (player.Money >= 10)
                {
                    vehicle.Inventory.AddResource(ResourceId.Fuel, 10);
                    player.Money -= 10;
                }
                buy = false;
            }
        }
    }
}

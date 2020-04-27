using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceInventoryClass
{
    [SerializeField]
    List<ResourceClass> resources = new List<ResourceClass>();

    [SerializeField]
    float maxCapacity;

    [SerializeField]
    float usedCapacity;

    public float UsedCapacity
    {
        get { return usedCapacity; }
    }

    public float Capacity
    {
        get { return maxCapacity; }
    }

    public ResourceInventoryClass(float _capacity)
    {
        maxCapacity = _capacity;
        usedCapacity = 0;
    }

    public void AddResource(ResourceId _id, float _amount)
    {
        if (usedCapacity >= maxCapacity)
            return;
        float _requiredCapacity;
        foreach (ResourceClass _valuable in resources)
        {
            if (_valuable.Id == _id)
            {
                
                _requiredCapacity = _valuable.EvaluateAmount(_amount);
                Debug.Log("Required Capacity Old: " + _requiredCapacity);
                if (usedCapacity + _requiredCapacity <= maxCapacity)
                {
                    usedCapacity -= _valuable.Weight;
                    _valuable.AddAmount(_amount);
                    usedCapacity += _valuable.Weight;
                }
                else if (usedCapacity + _requiredCapacity > maxCapacity)
                {
                    
                    float overCapacity = (usedCapacity + _requiredCapacity) - maxCapacity;
                    float newAmount = _amount - (overCapacity / _valuable.WeightPerUnit);
                    Debug.Log("Over Capacity " + overCapacity + ". Adding " + newAmount + " Resources Instead.");
                    usedCapacity -= _valuable.Weight;
                    _valuable.AddAmount(newAmount);

                    if (_valuable.EvaluateAmount(_valuable.Quantity) > maxCapacity)
                        Debug.LogError("ERROR: Too High Weight");

                    usedCapacity += _valuable.Weight;
                }
                return;
            }
        }//If it completes the loop, it doesn't contain the resource
        ResourceClass newResource = new ResourceClass(_id);

        _requiredCapacity = newResource.EvaluateAmount(_amount);
        Debug.Log("Required Capacity: " + _requiredCapacity);
        if (usedCapacity + _requiredCapacity <= maxCapacity)
        {
            newResource.AddAmount(_amount);
        }
        else
        {
            float overCapacity = (usedCapacity + _requiredCapacity) - maxCapacity;
            float newAmount = _amount - (overCapacity/newResource.WeightPerUnit);
            Debug.Log("Over Capacity " + overCapacity + ". Adding " + newAmount + " New Resources Instead.");
            newResource.AddAmount(newAmount);
        }

        resources.Add(newResource);
        usedCapacity += newResource.Weight;
        
        
    }

    public void RemoveResource(ResourceId _id)
    {
        foreach (ResourceClass _valuable in resources)
        {
            if (_valuable.Id == _id)
            {
                usedCapacity -= _valuable.EvaluateAmount(_valuable.Quantity);
                _valuable.AddAmount(-_valuable.Quantity);
            }
        }
    }

    public bool CheckForResource(ResourceId _id)
    {
        foreach (ResourceClass _valuable in resources)
        {
            if (_valuable.Id == _id)
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveResource(ResourceId _id, float _amount)
    {
        _amount = Mathf.Abs(_amount);
        foreach (ResourceClass _valuable in resources)
        {
            if (_valuable.Id == _id)
            {
                usedCapacity -= Mathf.Min(usedCapacity, _valuable.EvaluateAmount(_amount));
                _valuable.AddAmount(-_amount);
            }
        }
    }

    public float GetResourceCapacity(ResourceId resourceId)
    {
        foreach (ResourceClass _valuable in resources)
        {
            if (_valuable.Id == resourceId)
            {
                return  _valuable.Weight;
            }
        }
        return 0f;
    }

    public float GetResourceAmount(ResourceId resourceId)
    {
        foreach (ResourceClass _valuable in resources)
        {
            if (_valuable.Id == resourceId)
            {
                return _valuable.Quantity;
            }
        }
        return 0f;
    }

    public void AddRandomResource(float amount)
    {
        int num = Random.Range(0, 6);
        switch (num)
        {
            case 0:
                AddResource(ResourceId.Fuel, amount);
                break;
            case 1:
                AddResource(ResourceId.Dirt, amount);
                break;
            case 2:
                AddResource(ResourceId.Stone, amount);
                break;
            case 3:
                AddResource(ResourceId.Iron, amount);
                break;
            case 4:
                AddResource(ResourceId.Copper, amount);
                break;
            case 5:
                AddResource(ResourceId.Gold, amount);
                break;
        }
    }

    public string GetResourceName(ResourceId resource)
    {
        switch (resource)
        {
            case ResourceId.Fuel:
                return "Fuel";

            case ResourceId.Rations:
                return "Rations";

            case ResourceId.Dirt:
                return "Dirt";

            case ResourceId.Stone:
                return "Stone";

            case ResourceId.Iron:
                return "Iron Ore";

            case ResourceId.Copper:
                return "Copper Ore";

            case ResourceId.Gold:
                return "Gold Ore";

            case ResourceId.Lead:
                return "Lead Ore";

            default:
                return "Unkown Material";
                

        }
    }
}
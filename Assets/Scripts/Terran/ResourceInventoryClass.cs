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
                usedCapacity -= _valuable.Weight;
                _requiredCapacity = _valuable.EvaluateAmount(_amount);
                if (usedCapacity + _requiredCapacity <= maxCapacity)
                {
                    _valuable.AddAmount(_amount);
                }else
                {
                    float overCapacity = (usedCapacity + _requiredCapacity) - maxCapacity;
                    float newAmount = _amount - (overCapacity / _valuable.WeightPerUnit);
                    _valuable.AddAmount(newAmount);
                }
                usedCapacity += _valuable.Weight;
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
            Debug.Log(overCapacity);
            float newAmount = _amount - (overCapacity/newResource.WeightPerUnit);
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
                _valuable.AddAmount(_valuable.Quantity);
            }
        }
    }

    public void RemoveResource(ResourceId _id, float _amount)
    {
        foreach (ResourceClass _valuable in resources)
        {
            if (_valuable.Id == _id)
            {
                usedCapacity -= Mathf.Max(0, -_valuable.EvaluateAmount(_amount));
                _valuable.AddAmount(_amount);
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
}



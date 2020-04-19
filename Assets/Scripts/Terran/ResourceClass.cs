﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceClass
{
    [SerializeField]
    ResourceId identifier;

    [SerializeField]
    float weightPerUnit;

    [SerializeField]
    float quantity;

    [SerializeField]
    float totalWeight;

    public ResourceId Id
    {
        get { return identifier; }
    }

    public float Quantity
    {
        get { return quantity; }
    }

    public float Weight
    {
        get { return totalWeight; }
    }

    public float WeightPerUnit
    {
        get { return weightPerUnit; }
    }

    public ResourceClass(ResourceId _identifier)
    {
        identifier = _identifier;
        switch (_identifier)
        {
            case ResourceId.Fuel:
                weightPerUnit = 0.8f;
                break;
            case ResourceId.Rations:
                weightPerUnit = 9.5f;
                break;
            case ResourceId.Dirt:
                weightPerUnit = 18f;
                break;
            case ResourceId.Stone:
                weightPerUnit = 68f;
                break;
            case ResourceId.Iron:
                weightPerUnit = 222.7f;
                break;
            case ResourceId.Copper:
                weightPerUnit = 249.9f;
                break;
            case ResourceId.Gold:
                weightPerUnit = 547f;
                break;
            case ResourceId.Lead:
                weightPerUnit = 321.6f;
                break;
        }
    }

    public void AddAmount(float amount)
    {
        quantity += amount;
        totalWeight = quantity * weightPerUnit;
    }

    public float EvaluateAmount(float amount)
    {
        return amount * weightPerUnit;
    }
}

public enum ResourceId
{
    Fuel,
    Rations,
    Dirt,
    Stone,
    Iron,
    Copper,
    Gold,
    Lead

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Basic Part", menuName = "Basic Part", order = 51)]
public class PartBase : ScriptableObject
{
    [SerializeField]
    string _partName = "";

    [SerializeField]
    Sprite _image = null;

    [SerializeField]
    Sprite _uiIcon = null;

    [SerializeField]
    int _baseCost = 10;

    [SerializeField]
    int _tier = 1;

    [SerializeField]
    float _fuelEfficiency = 1;

    [SerializeField]
    float _weight = 10;

    public string PartName
    {
        get { return _partName; }
        set { _partName = value; }
    }

    public Sprite Image
    {
        get { return _image; }
        set { _image = value; }
    }

    public Sprite Icon
    {
        get { return _uiIcon; }
        set { _uiIcon = value; }
    }

    public float FuelEfficiency
    {
        get { return _fuelEfficiency; }
        set { _fuelEfficiency = value; }
    }

    public float Weight
    {
        get { return _weight; }
        set { _weight = value; }
    }

    public int Cost
    {
        get { return _baseCost; }
        set { _baseCost = value; }
    }

    public int Tier
    {
        get { return _tier; }
        set { _tier = value; }
    }
}
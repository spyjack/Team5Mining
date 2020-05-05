using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cabin", menuName = "Vehicle Parts/Cabin", order = 51)]
public class PartCabin : PartBase
{

    [SerializeField]
    int _fuelCapacity = 1; //How much fuel can be contained

    [SerializeField]
    int _crewCapacity = 1; //Max amount of crew

    [SerializeField]
    float _lightRangeBonus = 1; //Bonus health to the ship

    public int FuelCapacity
    {
        get { return _fuelCapacity; }
        set { _fuelCapacity = value; }
    }

    public int CrewCapacity
    {
        get { return _crewCapacity; }
        set { _crewCapacity = value; }
    }

    public float LightRange
    {
        get { return _lightRangeBonus; }
        set { _lightRangeBonus = value; }
    }
}
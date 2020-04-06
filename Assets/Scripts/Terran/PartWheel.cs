using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wheel", menuName = "Vehicle Parts/Wheel", order = 51)]
public class PartWheel : PartBase
{

    [SerializeField]
    float _traction = 1; //Traction on terrain

    public float Traction
    {
        get { return _traction; }
        set { _traction = value; }
    }

}
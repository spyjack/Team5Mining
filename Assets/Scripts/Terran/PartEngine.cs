using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Engine", menuName = "Vehicle Parts/Engine", order = 51)]
public class PartEngine : PartBase
{

    [SerializeField]
    float _horsePower = 1; //Rate to accelerate quickly

    [SerializeField]
    float _maxSpeed = 200; //Max speed the vehicle can go

    [SerializeField]
    AudioClip _engineSound = null;

    //How much fuel is used every over time;

    public float Power
    {
        get { return _horsePower; }
        set { _horsePower = value; }
    }

    public float Speed
    {
        get { return _maxSpeed; }
        set { _maxSpeed = value; }
    }

    public AudioClip Sound
    {
        get { return _engineSound; }
        set { _engineSound = value; }
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Drill", menuName = "Vehicle Parts/Drill", order = 51)]
public class PartDrill : PartBase
{

    [SerializeField]
    int _strengthLevel = 1; //What tier of terrain can it mine + how fast it mines

    [SerializeField]
    float _efficiency = 0.25f; //What percent of minerals it collects

    [SerializeField]
    float _range = 0.5f; //How far away the drill mines

    public int Strength
    {
        get { return _strengthLevel; }
        set { _strengthLevel = value; }
    }

    public float Efficiency
    {
        get { return _efficiency; }
        set { _efficiency = value; }
    }

    public float Range
    {
        get { return _range; }
        set { _range = value; }
    }

}
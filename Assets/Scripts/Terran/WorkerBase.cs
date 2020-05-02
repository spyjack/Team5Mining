using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Worker", menuName = "Worker", order = 51)]
public class WorkerBase : ScriptableObject
{
    [SerializeField]
    string _workerName = "";

    [SerializeField]
    Sprite _portrait = null;

    [SerializeField]
    Gender _gender = Gender.Male;

    [SerializeField]
    WorkStation _equippedStation = WorkStation.None;

    [SerializeField]
    int currentHealth = 3;

    [SerializeField]
    int maxHealth = 3;

    [SerializeField]
    int motorskills = 1;

    [SerializeField]
    int engineering = 1;

    [SerializeField]
    int operating = 1;

    [SerializeField]
    float cost = 1;

    public string WorkerName
    {
        get { return _workerName; }
        set { _workerName = value; }
    }

    public Sprite Portrait
    {
        get { return _portrait; }
        set { _portrait = value; }
    }

    public Gender Gender
    {
        get { return _gender; }
        set { _gender = value; }
    }

    public int Motorskills
    {
        get { return motorskills; }
        set { motorskills = value; }
    }

    public int Engineering
    {
        get { return engineering; }
        set { engineering = value; }
    }

    public int Operating
    {
        get { return operating; }
        set { operating = value; }
    }

    public float Cost
    {
        get { return cost; }
        set { cost = value; }
    }

    public WorkStation Station
    {
        get { return _equippedStation; }
        set { _equippedStation = value; }
    }
}

public enum Gender
{
    Male,
    Female,
    Other
}

public enum WorkStation
{
    None,
    Cabin,
    Engine,
    Drill,
    Spare
}

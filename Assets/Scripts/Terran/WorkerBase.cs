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
    bool isStarving = false;

    [SerializeField]
    bool isDead = false;

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

    public bool Starvation
    {
        get { return isStarving; }
    }

    public bool Dead
    {
        get { return isDead; }
    }

    public WorkStation Station
    {
        get { return _equippedStation; }
        set { _equippedStation = value; }
    }

    public void Eat()
    {
        if (isStarving)
        {
            isStarving = false;
        }
        if (currentHealth < maxHealth)
            currentHealth++;
    }

    public void Starve()
    {
        if (!isStarving)
        {
            isStarving = true;
        }else if (isStarving)
        {
            if (currentHealth > 0)
            {
                currentHealth--;
            }else if(currentHealth <= 0)
            {
                int rand = Random.Range(1, 4);
                if (rand == 1)
                    motorskills = Mathf.Max(1, motorskills - 1);
                if (rand == 2)
                    operating = Mathf.Max(1, operating - 1);
                if (rand == 3)
                    engineering = Mathf.Max(1, engineering - 1);

                if (engineering == 1 && motorskills == 1 && motorskills == 1)
                {
                    isDead = true;
                }
            }
        }
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

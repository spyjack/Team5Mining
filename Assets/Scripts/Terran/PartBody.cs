using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Body", menuName = "Vehicle Parts/Body", order = 51)]
public class PartBody : PartBase
{
    [SerializeField]
    int _healthMax = 100;

    [SerializeField]
    float _capacity = 5000; //Amount of storage space

    [SerializeField]
    Sprite _windowSprite = null;

    public float Capacity
    {
        get { return _capacity; }
        set { _capacity = value; }
    }

    public int Health
    {
        get { return _healthMax; }
        set { _healthMax = value; }
    }

    public Sprite WindowSprite
    {
        get { return _windowSprite; }
        set { _windowSprite = value; }
    }
}
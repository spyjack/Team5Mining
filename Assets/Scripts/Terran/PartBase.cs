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

    public int Cost
    {
        get { return _baseCost; }
        set { _baseCost = value; }
    }
}
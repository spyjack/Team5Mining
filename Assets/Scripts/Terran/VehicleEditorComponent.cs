using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleEditorComponent : MonoBehaviour
{
    public PartType partType = PartType.Drill;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}

public enum PartType
{
    Drill,
    Cabin,
    Engine,
    Wheels,
    Upgrade
}


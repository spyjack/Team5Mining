using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthLighting : MonoBehaviour
{
    [SerializeField]
    Transform cameraPos = null;

    [SerializeField]
    Light sunLight = null;

    [SerializeField]
    float maxHeight = 0;

    [SerializeField]
    float maxDepth = -100;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        sunLight.intensity = Mathf.Min(1, Normalize(cameraPos.position.y * 4));
    }

    float Normalize(float _height)
    {
        return Mathf.Max(0,((_height - (maxDepth * 4)) / ((maxHeight * 4) - (maxDepth * 4))));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GIF : MonoBehaviour
{/*
    float up = -872.0f;
    public float add;
    void Update()
    {
        up += add;
        //transform.Translate(Vector2(0.0f, up, 0.0f) * Time.deltaTime, Space.World);
        transform.localPosition = new Vector2(0.0f, up);
    }
    */
    [SerializeField]
    Vector3 target;

    [SerializeField]
    float speed;

    Vector3 dirNormalized;

    void Start()
    {
        dirNormalized = (target - transform.position).normalized;
    }

    void Update()
    {/*
        if (Vector3.Dist(target, transform.position) <= 1)
        {
            enabled = false;  // causes that Update() of this MonoBehavior is not called anymore (until enabled is set back to true)
                              // Do whatever you want when the object is close to its target here
        }
        else*/
        {
            transform.position = transform.position + dirNormalized * speed * Time.deltaTime;
        }
    }
}

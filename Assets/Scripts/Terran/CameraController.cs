﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform cam;

    [SerializeField]
    BoxCollider2D cameraPlane;

    [SerializeField]
    float zoomModifier = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newCamPos = new Vector3(0, 0, -10);
        newCamPos.x = Mathf.Min(Mathf.Max(cameraPlane.bounds.min.x, cam.position.x + Input.GetAxis("Horizontal")), cameraPlane.bounds.max.x);
        newCamPos.y = Mathf.Min(Mathf.Max(cameraPlane.bounds.min.y, cam.position.y + Input.GetAxis("Vertical")), cameraPlane.bounds.max.y);
        cam.position = newCamPos;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f && zoomModifier > 2) // forward
        {
            zoomModifier -= 0.25f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && zoomModifier < 15) // backwards
        {
            zoomModifier += 0.25f;
        }
        Camera.main.orthographicSize = zoomModifier;
    }
}
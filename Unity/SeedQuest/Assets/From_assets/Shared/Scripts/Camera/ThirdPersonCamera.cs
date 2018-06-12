﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    public float Y_ANGLE_MIN = 0.0f;
    public float Y_ANGLE_MAX = 50.0f;

    private float currentX = 0.0f;
    private float currentY = 0.0f;
    public float distance = 6.0f;

    public Transform lookAt;

    public bool inverted = false;
    public float mouseSensitivityX = 5.0f;
    public float mouseSensitivityY = 2.0f;
    public float keySensitivityX = 50.0f;
    public Vector3 offset = new Vector3(0.0f, 2.0f, 0.0f);
    public Vector3 lookAtOffset = new Vector3(0.0f, 2.0f, 0.0f);

    private Vector3 altOffset;

	// Use this for initialization
	void Start () {

        altOffset = lookAt.transform.position - transform.position;

	}

    private void Update()
    {
        currentX += Input.GetAxis("Mouse X") * mouseSensitivityX;
        if(inverted)
            currentY += Input.GetAxis("Mouse Y") * mouseSensitivityY;
        else
            currentY += -Input.GetAxis("Mouse Y") * mouseSensitivityY;
        
        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    // Update is called once per frame
    void LateUpdate () {



        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = lookAt.position + rotation * dir + offset;
        transform.LookAt(lookAt.position + lookAtOffset);


        // This code is for not using the mouse to move the camera
        /*
        float desiredAngle = lookAt.transform.eulerAngles.y;
        rotation = Quaternion.Euler(0, desiredAngle, 0);
        transform.position = lookAt.transform.position - (rotation * altOffset);
        transform.LookAt(lookAt.transform);
        */
	}
}

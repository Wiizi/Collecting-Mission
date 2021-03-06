﻿using System.Collections;
using UnityEngine;
using System;

public class CameraMovement : MonoBehaviour {
    [Header("Panning")]
    public float panSpeed = 5f;
    public float panBorderThickness = 15f;
    public float panLimitX = 10f;
    public Vector2 panLimitZ = new Vector2(-16f, 1f);

    [Header("Rotating")]
    public float rotateSpeed = 2f;
    public Vector2 verticalRotationLimit = new Vector2(10f, 60f);
    private float yaw;
    private float pitch;

    [Header("Zooming")]
    public float zoomSpeed = 50f;
    public Vector2 zoomLimit = new Vector2(30f, 50f);

    const int mouseLftClick = 0;
    const int mouseRightClick = 1;

    void Start()
    {
        pitch = transform.eulerAngles.x;
    }

    void Update()
    {
        transform.position = StartPanning();
        Camera.main.fieldOfView = StartZooming();
        transform.eulerAngles = StartRotating();
    }

    /*
     * Move the camera on its XZ-plane
     */
    Vector3 StartPanning()
    {
        Vector3 panPosition = transform.position;
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0f;
        right.y = 0f;

        /*
         *  Move right if (Input.mousePosition.x >= Screen.width - panBorderThickness) / if (Input.GetKey(KeyCode.D))
         *  or left if (Input.mousePosition.x <= panBorderThickness) / if (Input.GetKey(KeyCode.A))
         */
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            panPosition += right * panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            panPosition -= right * panSpeed * Time.deltaTime;
        }
        /*
         * Move forward if (Input.mousePosition.y >= Screen.height - panBorderThickness) / if (Input.GetKey(KeyCode.W))
         * or backward if (Input.mousePosition.y <= panBorderThickness) / if (Input.GetKey(KeyCode.S))
         */
        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            panPosition += forward * panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= panBorderThickness)
        {
            panPosition -= forward * panSpeed * Time.deltaTime;
        }
        panPosition.x = Mathf.Clamp(panPosition.x, -panLimitX, panLimitX);
        panPosition.z = Mathf.Clamp(panPosition.z, panLimitZ.x, panLimitZ.y);

        return panPosition;
    }

    /*
     * Move camera along X and Y axis
     */
    Vector3 StartRotating()
    {
        if (Input.GetMouseButton(mouseRightClick))
        {
            yaw += Input.GetAxis("Mouse X") * rotateSpeed;
            pitch -= Input.GetAxis("Mouse Y") * rotateSpeed;
        }
        return new Vector3
                (
                    Mathf.Clamp(pitch, verticalRotationLimit.x, verticalRotationLimit.y),
                    yaw,
                    0f
                );
    }

    /*
     * Move camera linearly along Z axis
     */
    float StartZooming()
    {
        float scrollValue = Input.mouseScrollDelta.y;
        float FOV = Camera.main.fieldOfView;

        if (scrollValue != 0)
        {
            FOV -= scrollValue;
        }

        return Mathf.Clamp(FOV, zoomLimit.x, zoomLimit.y);
    }
}
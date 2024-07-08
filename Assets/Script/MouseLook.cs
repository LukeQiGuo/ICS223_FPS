using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : ActiveDuringGameplay
{

    public enum RotationAxes
    {
        MonseXAndY,
        MouseX,
        MouseY
    }

    public RotationAxes axes = RotationAxes.MonseXAndY;

    public float sensitiveHoriz = 9.0f;
    public float sensitiveVert = 9.0f;
    public float minVert = -45.0f;
    public float maxVert = 45.0f;
    public float rotationX = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (axes == RotationAxes.MouseX)
        {
            float deltHoriz = Input.GetAxis("Mouse X") * sensitiveHoriz;
            transform.Rotate(Vector3.up * deltHoriz);

        }
        else if (axes == RotationAxes.MouseY)
        {
            rotationX -= Input.GetAxis("Mouse Y") * sensitiveVert;
            rotationX = Mathf.Clamp(rotationX, minVert, maxVert);

            transform.localEulerAngles = new Vector3(rotationX, 0, 0);
        }
        else
        {
            float deltaHor = Input.GetAxis("Mouse X") * sensitiveHoriz;
            rotationX -= Input.GetAxis("Mouse Y") * sensitiveVert;
            rotationX = Mathf.Clamp(rotationX, minVert, maxVert);
            float rotationY = transform.localEulerAngles.y + deltaHor;

            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
        }

    }
}

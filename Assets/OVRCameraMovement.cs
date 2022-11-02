/*
Author: Lisa Pothoven
OVR Camera Movement
28 July 2020
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRCameraMovement : MonoBehaviour
{
    public float maxSpeed = 100f;
    public float maxRotationAngle = 30f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float speed = 0;
        float rotation = 0;
        // forward movement (speed)
        Vector3 position = transform.position;
        Vector2 Lthumb = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.Touch);
        speed = -maxSpeed * OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger, OVRInput.Controller.Touch) + 
                maxSpeed * OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger, OVRInput.Controller.Touch);
        position.z += speed;
        transform.position = position;

        // side movement (rotation)
        Vector3 euler = new Vector3(0,0,0);
        rotation = maxRotationAngle * Lthumb.x;
        euler.y = rotation;
        transform.rotation = Quaternion.Euler(euler);

        // check where terrain is - adjust y position
    }
}

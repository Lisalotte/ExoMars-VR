using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{

    Camera m_MainCamera;
    float smooth = 5.0f;
    float tiltAngle = 60.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_MainCamera = Camera.main;        
    }

    // Update is called once per frame
    void Update()
    {   
        float tiltAroundY = Input.GetAxis("Mouse X");// * tiltAngle;
        float tiltAroundX = -1*Input.GetAxis("Mouse Y");// * tiltAngle;

        Vector3 target = new Vector3(tiltAroundX, tiltAroundY, 0);

        m_MainCamera.transform.Rotate(target); // Quaternion.Slerp(m_MainCamera.transform.rotation, target,  Time.deltaTime * smooth);
    }
}

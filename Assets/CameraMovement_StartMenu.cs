using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement_StartMenu : MonoBehaviour
{
    public Camera cam;

    private Vector3 prevPos;
    
    private int timer = 0;
    private float t = 0;
    public float r = 50;

    public float dt = 0.0001f;

    // Start is called before the first frame update
    void Start()
    {
        prevPos = transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        float x = r * Mathf.Cos(t);
        float z = r * Mathf.Sin(t);

        if (t <= 2*Mathf.PI) {
            t += dt * Mathf.PI;
        } else t = 0;
        Debug.Log("x: " + x + ", z: " + z);

        transform.Translate(new Vector3(x, 0, z));
        //Vector3 orientation = transform.position - prevPos;
        cam.transform.LookAt(transform);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheelsMovement : MonoBehaviour
{

    public GameObject wheelMechanism;
    public float scale;
    public float direction = 1;
    private Vector3 p_prev;
    private Quaternion r;

    WheelCollider wheelCollider;

    private float dy;
    private float suspension_length;

    // Start is called before the first frame update
    void Start()
    {
        wheelCollider = GetComponent<WheelCollider>();

        //wheelMechanism = GameObject.Find("Wheel_mechanism_FL");
        wheelCollider.GetWorldPose(out p_prev, out r);

        suspension_length = Mathf.Abs(wheelMechanism.transform.position.z - wheelCollider.transform.position.z)*scale;
        //Debug.Log(wheelMechanism.transform.position +" "+ wheelCollider.transform.position + " " +p_prev);
        //p_prev.y = Terrain.activeTerrain.SampleHeight(new Vector3(wheelPos.x, 0, wheelPos.z));
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector3 p;
        wheelCollider.GetWorldPose(out p, out r);

        // get the terrain height at the random position
        //p.y = Terrain.activeTerrain.SampleHeight(new Vector3(p.x, 0, p.z));
        
        float angle = (Mathf.Asin(p.y/suspension_length) - Mathf.Asin(p_prev.y/suspension_length)) * 180f/Mathf.PI;
        //dy = (p_prev.y - p.y)*scale*direction;
        Debug.Log("Angle: " + angle);
        //float dx = p.x - p_prev.x;
        //float dz = p.z - p_prev.z;
        p_prev = p;

        //float rotation = Mathf.Atan(dy/suspension_length) * 180f/Mathf.PI;
        
        wheelMechanism.transform.Rotate(new Vector3(angle,0f,0f));

    }
}

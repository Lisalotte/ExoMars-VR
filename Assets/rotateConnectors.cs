using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateConnectors : MonoBehaviour
{

    public GameObject connector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float tiltAroundY = transform.rotation.y;
        Quaternion target = new Quaternion();
        target.Set(0, -1*tiltAroundY, 0, 1);
        Debug.Log(transform.rotation);
        connector.transform.rotation = target;
    }
}

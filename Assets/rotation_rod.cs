using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation_rod : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        position.x += 0.5f;
        transform.position = position;
    }
}

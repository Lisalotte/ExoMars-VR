using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTornado : MonoBehaviour
{
    private Vector3[] path;
    public Terrain terrain;
    public float speed = 0.06f;
    private float fraction = 0.0f;
    private Transform trans;

    private float time;
 
    void Start () {
        time = 0;
    }
    
    float pos;
    void Update () {
        if (time >= 20) {
            pos = Random.Range(-speed,speed);
            time = 0;
        }
        time += 1;
        gameObject.transform.Translate(pos, 0, pos);
        Vector3 position = gameObject.transform.position;
        position.y = terrain.SampleHeight(transform.position);
        gameObject.transform.position = position;
    }
}

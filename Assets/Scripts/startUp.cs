// A 10 second delay at the start of the game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startUp : MonoBehaviour
{

    private float counter;
    public bool startingUp;

    // Start is called before the first frame update
    void Start()
    {
        counter = 0f;    
        startingUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter >= 10f) {
            startingUp = false;
        }
    }
}

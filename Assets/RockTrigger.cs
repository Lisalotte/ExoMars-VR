using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RockTrigger : MonoBehaviour
{
    
    public rockDistributer rockScript;

    // Start is called before the first frame update
    void Start()
    {
        //Terrain marsTerrain = Terrain.activeTerrain;
        rockScript = GameObject.Find("Terrain").GetComponent<rockDistributer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    void OnTriggerEnter(Collider other) {
        Debug.Log("Object Entered the trigger");
        if (other.tag == "rock") {            
            Debug.Log("'tis a rock");
        }
    }*/
    
    
    void OnTriggerExit(Collider other) {
        Debug.Log("Object Exited the trigger!");
        if (other.tag == "rock") {            
            Debug.Log("Rocks: " + rockScript.currentObjects);

            rockScript.changeObject(other.gameObject);

            /*
            Destroy(other); // remove rock from terrain
            if(rockScript.currentObjects > 0) {
                rockScript.currentObjects -= 1;
            }
            Debug.Log("Rocks: " + rockScript.currentObjects);
            */
        }
    }
}

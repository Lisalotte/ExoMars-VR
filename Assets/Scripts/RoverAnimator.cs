using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverAnimator : MonoBehaviour
{
    private Animation anim;
    public GameObject rover;
    // Start is called before the first frame update
    public Camera cam;

    public startUp startUpScript;

    public bool roverInStartingPos;

    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        startUpScript = GameObject.Find("Scripts").GetComponent<startUp>();

        roverInStartingPos = true;
    }

    int frameCount = 0;

    // Update is called once per frame
    void Update()
    {
        if (!anim.isPlaying) {            
            anim.enabled = false;
        }

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (roverInStartingPos) {
            if (startUpScript.startingUp == false) {
                if ((Physics.Raycast(ray, out hit) && hit.transform.name == "rover") || (Time.fixedTime > 20f)) {
                    if (null != GetComponent<Animation>()) {
                        anim.enabled = true;
                        anim.PlayQueued("Rover leaves again");
                        anim.PlayQueued("Mast_movement");
                        anim.PlayQueued("Rover_navigation");
                        anim.PlayQueued("Drill_down");
                        anim.PlayQueued("Drill_up");
                        roverInStartingPos = false;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (null != GetComponent<Animation>()) {
                    anim.enabled = true;
                    anim.PlayQueued("Rover leaves again");
                    anim.PlayQueued("Mast_movement");
                    anim.PlayQueued("Rover_navigation");
                    anim.PlayQueued("Drill_down");
                    anim.PlayQueued("Drill_up");
                    roverInStartingPos = false;
                }
            }
        }
        else {  
            if (OVRInput.Get(OVRInput.Button.Two)) {
                if (null != GetComponent<Animation>()) {
                    anim.enabled = true;
                    anim.Play("Drill_down");
                    anim.PlayQueued("Drill_up");
                }
            }  
            if (OVRInput.Get(OVRInput.Button.Three)) {
                if (null != GetComponent<Animation>()) {
                    anim.enabled = true;
                    anim.Play("Mast_movement");
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
                anim.Stop();   
                anim.enabled = false;
            }
        }
    }
}

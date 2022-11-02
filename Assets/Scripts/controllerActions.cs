using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllerActions : MonoBehaviour
{

    private DayNightCycleTEST script_dayNightCycle;
    private RoverController script_roverController;
    private RoverAnimator script_roverAnimator;    
    private ParticleScript particleScript;
    private AudioControl script_AudioControl;
    public GameObject worldManager;
    public GameObject rover;


    private GameObject roverCamPos, mainCamPos;
    private GameObject mainCam;
    private GameObject cameraPos;
    public GameObject controls;

    public Transform trackingSpace;
    public OVRInput.Controller controllerL;
    public OVRInput.Controller controllerR;

    private float previousTime;
    private int timer;
    private bool OVRdetected;

    // Start is called before the first frame update
    void Start()
    {
        script_dayNightCycle = worldManager.GetComponent<DayNightCycleTEST>();
        script_AudioControl = worldManager.GetComponent<AudioControl>();
        previousTime = script_dayNightCycle.degreesPerSec;
        rover = GameObject.Find("rover");
        script_roverController = rover.GetComponent<RoverController>();
        script_roverAnimator = rover.GetComponent<RoverAnimator>();
        
        //controls = GameObject.Find("controls");

        if (GameObject.Find("OVRCameraRig")) {
            mainCam = GameObject.Find("OVRCameraRig");
            OVRdetected = true;
        } else {
            mainCam = GameObject.Find("Camera");
            OVRdetected = false;
        }

        roverCamPos = rover.transform.Find("roverCamPos").gameObject;
        mainCamPos = GameObject.Find("mainCamPos");
        cameraPos = mainCamPos;

        particleScript = worldManager.transform.Find("DustStorm").gameObject.GetComponent<ParticleScript>();

        timer = 0;
    }


    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();

        // Oculus VR
        if (OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger, OVRInput.Controller.Touch) > 0.1f) {
            Debug.Log("test");
            script_dayNightCycle.degreesPerSec = previousTime + 10 * OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger, OVRInput.Controller.Touch);
            //particleScript.changeSpeed(10 * OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger, OVRInput.Controller.Touch));
        } else {
            script_dayNightCycle.degreesPerSec = previousTime;
            //particleScript.changeSpeed(0);
        }        

        if (OVRInput.Get(OVRInput.Button.Four, OVRInput.Controller.Touch)) {
            //if (!controls.activeInHierarchy) controls.SetActive(true);
            //else controls.SetActive(false);
            controls.SetActive(true);
            Debug.Log("controls");
        } else if (Time.fixedTime > 10f) controls.SetActive(false);
        
        // Keyboard
        if (Input.GetKey(KeyCode.LeftControl)) {
            script_dayNightCycle.degreesPerSec = previousTime + 10;
        } else {
            script_dayNightCycle.degreesPerSec = previousTime;
        }

        if ( (OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger, OVRInput.Controller.Touch) > 0.7f) || Input.GetKeyDown(KeyCode.Z) ) {
            script_AudioControl.Next();
        } 
        if (script_roverAnimator.roverInStartingPos == false) {
            if ( (OVRInput.Get(OVRInput.Button.One) || Input.GetKeyDown(KeyCode.Tab) ) && timer > 12) {
                if (!script_roverController.roverView) {
                    script_roverController.roverView = true;
                    cameraPos = roverCamPos;
                    timer = 0;
                    rover.GetComponent<Rigidbody>().drag = 0.5f;
                } else {
                    script_roverController.roverView = false;
                    cameraPos = mainCamPos;
                    timer = 0;
                    rover.GetComponent<Rigidbody>().drag = 50f;
                }
            }
        }
        timer += 1;

        mainCam.transform.position = cameraPos.transform.position;
        if (OVRdetected) mainCam.transform.rotation = cameraPos.transform.rotation;
    }
}

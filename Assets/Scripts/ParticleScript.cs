using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; // in order to access SerializedObject

public class ParticleScript : MonoBehaviour
{
    //SerializedObject thisParticleSystem;
    ParticleSystem thisParticleSystem;
    bool isChanging;
    public float transitionSpeed = 10.0f;
    public float min_radius = 1.0f;
    public float max_radius = 10.0f;
    public float min_angle = 1.0f;
    public float max_angle = 30.0f;
    public ParticleSystem.ShapeModule shape;  
    public ParticleSystem.MainModule main;
    public ParticleSystem.EmissionModule emission;
    public ParticleSystem.ColorOverLifetimeModule lifetimeColor;
    [SerializeField] internal Gradient dustColor;
    private float previousSpeed;
    private int countdown;

    bool active; 
    // Start is called before the first frame update
    void Start()
    {
        thisParticleSystem = GetComponent<ParticleSystem>();
        shape = thisParticleSystem.shape;
        shape.scale = new Vector3(200f, 10f, 200f); // maybe do a changing scale size

        isChanging = false;
        previousSpeed = thisParticleSystem.main.simulationSpeed;

        countdown = 60;
        active = false;
        
        main = thisParticleSystem.main;
        lifetimeColor = thisParticleSystem.colorOverLifetime;
        emission = thisParticleSystem.emission;
    }

    void Update() {
        if (countdown > 1) countdown -= 1;
        else {
            if (Random.Range(0,1f) > 0.8f) {
                active = true;
                emission.rateOverTime = 500;
                //gameObject.SetActive(true);
                Debug.Log("true");
                countdown = 240;
            }
            else {
                active = false;
                //gameObject.SetActive(false);
                emission.rateOverTime = 0;
                Debug.Log("false");
                countdown = 460;
            }
        }
    }

    public void changeColor(float _cycleStage) { //Color color) {
        if (active) {
            main.startColor = dustColor.Evaluate(_cycleStage);
            lifetimeColor.color = dustColor.Evaluate(_cycleStage);
        }
    }

    public void changeSpeed(float speed) {
        main = thisParticleSystem.main;
        main.simulationSpeed = previousSpeed + speed;
    }
}

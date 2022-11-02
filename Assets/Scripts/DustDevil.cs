using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; // in order to access SerializedObject

public class DustDevil : MonoBehaviour
{
    //SerializedObject thisParticleSystem;
    ParticleSystem dustDevil;
    public ParticleSystem.ShapeModule shape;  
    public ParticleSystem.MainModule main;
    public ParticleSystem.ColorOverLifetimeModule lifetimeColor;
    public ParticleSystem.EmissionModule emission;
    public ParticleSystem.ForceOverLifetimeModule force;
    public ParticleSystem.VelocityOverLifetimeModule velocity;
    public ParticleSystem.Particle[] allParticles;
    [SerializeField] internal Gradient dustColor;
    [SerializeField] internal float degreesPerSec = 10f;
    [SerializeField] private float time;

    private ParticleSystem.MinMaxCurve forcex, forcey, forcez;
    
    public float maxHeight = 20f;
    public float maxRadius = 10f;

    private Vector3 currentPos;

    // Start is called before the first frame update
    void Start()
    {
        dustDevil = GetComponent<ParticleSystem>();
        shape = dustDevil.shape;
        main = dustDevil.main;
        emission = dustDevil.emission;
        force = dustDevil.forceOverLifetime;
        velocity = dustDevil.velocityOverLifetime;

        main.maxParticles = 20000;
        main.startSize = 0.3f;

        allParticles = new ParticleSystem.Particle[main.maxParticles];

        currentPos = gameObject.transform.position;
       
        emission.rateOverTime = 1000;
        //shape.scale = new Vector3(10f,20f,10f);

        forcex = dustDevil.forceOverLifetime.x;
        forcey = dustDevil.forceOverLifetime.y;
        forcez = dustDevil.forceOverLifetime.z;

        time = 0;
        newCurve();
    }

    float radius = 1f;
    void Update() 
    {
        time += Time.deltaTime;
        if (time >= main.duration)
        {
            time = 0;
        }
        /*    
        int numAlive = dustDevil.GetParticles(allParticles);
        
        currentPos = gameObject.transform.position;

        for (int i=0; i<dustDevil.particleCount; i++) {
            float x, y, z;
            //radius = radius + 0.1f*time;
            //x = currentPos.x + radius * Mathf.Cos(time);
            //z = currentPos.z + radius * Mathf.Sin(time);
            y = 0.5f * time;

            //random shifts
            //gameObject.transform.Translate(Random.value, 0, Random.value);
            //allParticles[i].position = new Vector3(x, y, z);
        }*/

        //dustDevil.SetParticles(allParticles, numAlive);
        //tornado(time);
        
    }

    public void tornado(float time) {
        
        // spiral shape
        float cycleStage = time / main.duration;
        float angle = cycleStage * 360f * Mathf.PI / 180f;
        Debug.Log(angle);

        // Smaller spirals + large spiral
        force.x = forcex.Evaluate(cycleStage) * radius * Mathf.Cos(angle);
        force.z = forcez.Evaluate(cycleStage) * radius * Mathf.Sin(angle);
        //force.y = maxHeight*cycleStage;

        //velocity.orbitalY = maxRadius*cycleStage;// * Mathf.Cos(time);
        force.y = forcey.Evaluate(cycleStage);
        
    }

    private void newCurve() {
        AnimationCurve curveX = new AnimationCurve();
        AnimationCurve curveZ = new AnimationCurve();

        for (int i=0; i<10; i++) {
            float t = i / 10f * 2 * Mathf.PI;
            float x = Mathf.Cos(t);
            curveX.AddKey(x,t);

            float z = Mathf.Sin(t);
            curveZ.AddKey(z,t);

        }
        float scalar = 1f;
        velocity.x = new ParticleSystem.MinMaxCurve(scalar, curveX);
        velocity.z = new ParticleSystem.MinMaxCurve(scalar, curveZ);
    }

}

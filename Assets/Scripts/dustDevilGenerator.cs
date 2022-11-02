using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dustDevilGenerator : MonoBehaviour
{
    ParticleSystem dustDevil;
    public ParticleSystem.Particle[] allParticles;
    public ParticleSystem.ShapeModule shape;  
    public ParticleSystem.MainModule main;
    public ParticleSystem.EmissionModule emission;
    public ParticleSystem.VelocityOverLifetimeModule velocity;
    private GameObject parent;
    public GameObject followObject;
    private ParticleSystem.EmitParams emitParams;

    private float maxHeight = 90f;
    private float startSize = 2f;
    private float time;
    private int maxParticles = 6;
    private int lastParticleIndex = 0; //keeping track of oldest particle
    
    private Vector3 lastParentPosition;

    // Start is called before the first frame update
    void Start()
    {
        dustDevil = GetComponent<ParticleSystem>();
        shape = dustDevil.shape;
        main = dustDevil.main;
        emission = dustDevil.emission;
        velocity = dustDevil.velocityOverLifetime;

        maxParticles = main.maxParticles;
        allParticles = new ParticleSystem.Particle[maxParticles];

        //main.startSize = startSize;

        parent = dustDevil.transform.parent.gameObject;
        lastParentPosition = parent.transform.position;

        time = 0;
    }

    // Create particles at a random position within the dust devil
    void CreateParticle(Vector3 position, float size, Vector3 velocity, float angularVelocity) {
        int activeParticles = dustDevil.GetParticles(allParticles);

        if (activeParticles >= maxParticles) {
            allParticles[lastParticleIndex].remainingLifetime = -1;
            allParticles[lastParticleIndex].startLifetime = 1; // set to normal value instead of infinite
            
            lastParticleIndex++;
            if (lastParticleIndex >= maxParticles) lastParticleIndex = 0;

            dustDevil.SetParticles(allParticles, allParticles.Length);
        }
        
        emitParams.angularVelocity = 1;
        emitParams.position = position;
        emitParams.startSize = size;
        emitParams.velocity = velocity;
        //emitParams.startLifetime = float.MaxValue; // infinite lifetime

        dustDevil.Emit(emitParams, 1);
        dustDevil.Play();
    }

    void ChangeParticle(float angle, int i) {
        int activeParticles = dustDevil.GetParticles(allParticles);

        float vx = 0.5f * Mathf.Cos(angle);
        float vy = allParticles[i].velocity.y;
        float vz = 0.5f * Mathf.Sin(angle);
        allParticles[i].velocity = new Vector3(vx, vy, vz);

        float x = allParticles[i].position.x + Random.value;
        float y = allParticles[i].position.y + Random.value;
        float z = allParticles[i].position.z + Random.value;
        allParticles[i].position = new Vector3(x, y ,z); 

        //allParticles[i].size = Mathf.Max(startSize, allParticles[i].position.y/maxHeight * startSize);
        
        dustDevil.Play();
    }

    void detectMovement(int i, float dt) {

        float particleMass = 1f;
        float dx = followObject.transform.position.x - lastParentPosition.x;
        float dy = followObject.transform.position.y - lastParentPosition.y;
        float dz = followObject.transform.position.z - lastParentPosition.z;

        Vector3 movementDir = -1*(new Vector3(dx, dy, dz)).normalized;

        velocity.orbitalX = movementDir.x;
        velocity.orbitalY = movementDir.y;
        velocity.orbitalZ = movementDir.z;

        //Vector3 velocity = new Vector3(dx,dy,dz) / dt;

        //Vector3 force = particleMass * new Vector3(dx,dy,dz) / (dt*dt);
        //allParticles[i].velocity = velocity;

        //Debug.Log("force: " + dx);

    }

    // Update is called once per frame
    void Update()
    {        
        time += Time.deltaTime;
        if (time >= main.duration)
        {
            time = 0;
        }
        
        //float cycleStage = time / main.duration;
        //float angle = cycleStage Random.value;


        float angle = Random.value;
        float x = Mathf.Cos(angle);
        float y = dustDevil.shape.scale.y * Random.value;
        float z = Mathf.Sin(angle);   

        //int numAlive = dustDevil.GetParticles(allParticles);

        //for (int i=0; i<numAlive; i++) {
            //ChangeParticle(angle, i);
            //detectMovement(i, Time.deltaTime);
        //}

        detectMovement(0,Time.deltaTime);
        //dustDevil.SetParticles(allParticles, numAlive); 

    }
}

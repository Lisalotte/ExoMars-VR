using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; // in order to access SerializedObject

public class dustDevilColour : MonoBehaviour
{
    //SerializedObject thisParticleSystem;
    ParticleSystem thisParticleSystem;
    public ParticleSystem.MainModule main;
    public ParticleSystem.ColorOverLifetimeModule lifetimeColor;
    [SerializeField] internal Gradient dustColor;

    // Start is called before the first frame update
    void Start()
    {
        thisParticleSystem = GetComponent<ParticleSystem>();
    }

    public void changeColor(float _cycleStage) { //Color color) {
        main = thisParticleSystem.main;
        main.startColor = dustColor.Evaluate(_cycleStage);
        Debug.Log("Stage: " + _cycleStage);
        
        lifetimeColor = thisParticleSystem.colorOverLifetime;
        lifetimeColor.color = dustColor.Evaluate(_cycleStage);
    }
}

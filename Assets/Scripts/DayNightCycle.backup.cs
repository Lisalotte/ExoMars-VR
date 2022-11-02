using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public ParticleScript particleScript;
    public dustDevilColour dustDevilColour;
    public DustDevil dustDevil;
    internal static DayNightCycle instance;

    [SerializeField] internal float degreesPerSec = 0.004f;

    [SerializeField] private float time;

    [SerializeField] internal Gradient ambientColor;
    [SerializeField] internal Gradient skyTint;
    
    [Header("Sun")]
    [SerializeField] internal Light sun;
    [SerializeField] internal Gradient sunColor;
    [SerializeField] internal AnimationCurve SunGlareStrength;

    [Header("Fog")]
    [SerializeField] internal Gradient fogColor;
    [SerializeField] internal AnimationCurve fogStartDistance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }        
        else if (instance != this) {
            Debug.Log("Instance already exists, destroying object.");
            Destroy(this);
        }
        
        particleScript = GameObject.Find("DustStorm").GetComponent<ParticleScript>();
        //dustDevilColour = GameObject.Find("DustDevil").GetComponent<dustDevilColour>();
        //dustDevil = GameObject.Find("DustDevil").GetComponent<DustDevil>();
    }

    // Update is called once per frame
    private void Update()
    {
        time += degreesPerSec * Time.deltaTime;
        if (time >= 360f)
        {
            time -= 360f;
        }

        sun.transform.eulerAngles = new Vector3(time, -90f, 0f); //time / 360f;

        float _cycleStage = time / 360f;
        RenderSettings.ambientLight = ambientColor.Evaluate(_cycleStage);
        RenderSettings.fogColor = fogColor.Evaluate(_cycleStage);
        RenderSettings.fogStartDistance = fogStartDistance.Evaluate(_cycleStage);
        RenderSettings.skybox.SetFloat("_SunGlareStrength", SunGlareStrength.Evaluate(_cycleStage));
        RenderSettings.skybox.SetColor("_SkyTint", skyTint.Evaluate(_cycleStage));
        sun.color = sunColor.Evaluate(_cycleStage);
        if (particleScript) particleScript.changeColor(_cycleStage); //fogColor.Evaluate(_cycleStage));
        //dustDevilColour.changeColor(_cycleStage);
        //dustDevil.tornado(time);
    }

    internal void SetTime(float _time) {
        time = _time;
    }
}

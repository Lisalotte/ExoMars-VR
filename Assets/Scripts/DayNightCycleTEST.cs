using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycleTEST : MonoBehaviour
{
    public ParticleScript particleScript;
    public dustDevilColour dustDevilColour;
    public DustDevil dustDevil;
    public Shader MarsSkyboxBlended;

    public Material SkyboxDay;
    public Material SkyboxNight;
    internal static DayNightCycleTEST instance;

    Renderer rend;

    [SerializeField] public float degreesPerSec = 0.004f;

    [SerializeField] private float time;

    [SerializeField] internal Gradient ambientColor;
    [SerializeField] internal Gradient skyTint;
    [SerializeField] internal AnimationCurve starVis;
    
    [Header("Sun")]
    [SerializeField] internal Light sun;
    [SerializeField] internal Gradient sunColor;
    [SerializeField] internal AnimationCurve SunGlareStrength;

    [Header("Fog")]
    [SerializeField] internal Gradient fogColor;
    [SerializeField] internal AnimationCurve fogStartDistance;

    [Header("Star Rotation")]
    [SerializeField] float _spinSpeed = 10;
    [SerializeField] float _pitchSpeed = 1;
    [SerializeField] float _pitchAmount = 10;

    private bool isNight, isDay;
    private float initialFogStartD;

    private GameObject dustStorm;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer> ();
        if (instance == null)
        {
            instance = this;
        }        
        else if (instance != this) {
            Debug.Log("Instance already exists, destroying object.");
            Destroy(this);
        }
        
        dustStorm = transform.Find("DustStorm").gameObject;
        particleScript = dustStorm.GetComponent<ParticleScript>();
        //dustDevilColour = GameObject.Find("DustDevil").GetComponent<dustDevilColour>();
        //dustDevil = GameObject.Find("DustDevil").GetComponent<DustDevil>();

        RenderSettings.skybox.SetInt("isNight", 0);
        initialFogStartD = RenderSettings.fogStartDistance;
    }

    
    // Update is called once per frame
    private void Update()
    {
        time += degreesPerSec * Time.deltaTime;
        RenderSettings.skybox.SetFloat("_CustomTime", time);
        if (time >= 360f)
        {
            time -= 360f;
        }

        sun.transform.eulerAngles = new Vector3(-time+180f, 0f, 65f); //time / 360f;
        //RenderSettings.skybox.SetVector("_Rotation1", new Vector4(time, 0, 0, 0));

        float _cycleStage = time / 360f;
        RenderSettings.ambientLight = ambientColor.Evaluate(_cycleStage);
        RenderSettings.fogColor = fogColor.Evaluate(_cycleStage);
        //RenderSettings.fogStartDistance = 50f * fogStartDistance.Evaluate(_cycleStage);

        // Day
        RenderSettings.skybox.SetFloat("_SunGlareStrength", SunGlareStrength.Evaluate(_cycleStage));
        RenderSettings.skybox.SetColor("_SkyTint", skyTint.Evaluate(_cycleStage));
        //SkyboxDay.SetFloat("_SunGlareStrength", SunGlareStrength.Evaluate(_cycleStage));
        //SkyboxDay.SetColor("_SkyTint", skyTint.Evaluate(_cycleStage));
        
        // Night
        //SkyboxNight.SetFloat("_starVis", starVis.Evaluate(_cycleStage));
        RenderSettings.skybox.SetFloat("_starVis", starVis.Evaluate(_cycleStage));
        //RenderSettings.skybox.SetColor("_SkyTint", skyTint.Evaluate(_cycleStage));
        
        sun.color = sunColor.Evaluate(_cycleStage);
        if (particleScript) particleScript.changeColor(_cycleStage);
        //dustDevilColour.changeColor(_cycleStage);
        //dustDevil.tornado(time);

        
        if (time < 180f || time > 350f) {
            isDay = true;
            isNight = false;
            RenderSettings.skybox.SetInt("isNight", 0);
            sun.GetComponent<Light>().intensity = 1.5f;
            //particleScript.enabled = true;
            //dustStorm.SetActive(true);
        } else {
            isDay = false;
            isNight = true;
            RenderSettings.skybox.SetInt("isNight", 1);
            sun.GetComponent<Light>().intensity = 0;
            //dustStorm.SetActive(false);
        }
        
    }

    internal void SetTime(float _time) {
        time = _time;
    }
}

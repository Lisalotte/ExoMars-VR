using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPaint : MonoBehaviour
{

    public Shader drawShader;
    public Terrain _terrain;
    public Transform[] _wheel;
    private Material paintMaterial;
    private Material groundMaterial;
    private RenderTexture _splatmap;
    RaycastHit _groundHit;
    public float pixWidth = 256;
    public float pixHeight = 256;

    int _layerMask;
    public float xOrg;
    public float yOrg;
    public float scale = 1.0F;
    private Texture2D noiseTex;
    private Color[] pixels;
    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        //runCode();
    }

    [ContextMenu("Paint the Terrain")]
    void runCode() {
        _layerMask = LayerMask.GetMask("Ground");
        paintMaterial = new Material(drawShader);
        paintMaterial.SetVector("_PaintColor", Color.red);

        _terrain = Terrain.activeTerrain;
        groundMaterial = _terrain.materialTemplate;
        _splatmap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        //groundMaterial.SetTexture("_Paint", noiseTex);

        //rend = GetComponent<Renderer>();
        //noiseTex = new Texture2D((int)pixWidth, (int)pixHeight);
        noiseTex = Resources.Load("Materials/textures/terrainDetail", typeof(Texture2D)) as Texture2D;
        //noiseTex.ReadPixels(new Rect(0, 0, _splatmap.width, _splatmap.height), 0, 0);
        //noiseTex.Apply();
        
        pixels = new Color[noiseTex.width * noiseTex.height];
        //rend.material.mainTexture = noiseTex;

        // create perlin noise texture
        CalcNoise();
        groundMaterial.SetTexture("_Paint", noiseTex);
    }
    
    void CalcNoise() {
        float y = 0.0f;

        while (y < noiseTex.height) {
            float x = 0.0f;
            while (x < noiseTex.width) {
                Debug.Log("...");   
                float xCoord = xOrg + x / noiseTex.width * scale;
                float yCoord = yOrg + y / noiseTex.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord) * Random.Range(0.0f, 1.0f);
                pixels[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
                x++;
            }
            y++;
        }
        noiseTex.SetPixels(pixels);
        noiseTex.Apply();
    }

    private void Update() {
    }
}

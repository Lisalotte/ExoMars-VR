using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPaintOld : MonoBehaviour
{

    [System.Serializable]
    public class SplatHeights {
        public int textureIndex;
        public int startingHeight;
    }
    public SplatHeights[] splatHeights;
    public float xOrg;
    public float yOrg;
    public float scale = 1.0F;
    private Texture2D noiseTex;
    private Color[] pixels;
    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        TerrainData terrainData = Terrain.activeTerrain.terrainData;
        //float[, ,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers]; 
        float pixWidth = terrainData.size.x;
        float pixHeight = terrainData.size.z;

        rend = GetComponent<Renderer>();
        noiseTex = new Texture2D((int)pixWidth, (int)pixHeight);
        pixels = new Color[noiseTex.width * noiseTex.height];
        rend.material.mainTexture = noiseTex;

        // create perlin noise texture
        CalcNoise();
        

        // for values > 0.5
        // display second texture + normal map
        // else
        // display first texture + normal map

        /*
        for (int y=0; y < terrainData.alphamapHeight; y++) {
            for (int x=0; x < terrainData.alphamapWidth; x++) {
                float terrainHeight = terrainData.GetHeight(y,x);
                float[] splat = new float[splatHeights.Length];

                for (int i=0; i < splatHeights.Length; i++) {
                    if(terrainHeight >= splatHeights[i].startingHeight)
                        splat[i] = 1;
                }
                for (int j=0; j < splatHeights.Length; j++) {
                    splatmapData[x,y,j] = splat[j];
                }                
            }
        }
        terrainData.SetAlphamaps(0,0,splatmapData);
        */
    }

    void CalcNoise() {
        float y = 0.0f;

        while (y < noiseTex.height) {
            float x = 0.0f;
            while (x < noiseTex.width) {
                float xCoord = xOrg + x / noiseTex.width * scale;
                float yCoord = yOrg + y / noiseTex.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
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

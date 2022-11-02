using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTracks : MonoBehaviour
{
    public Shader drawShader;
    public GameObject _terrain;
    public Transform[] _wheel;
    private Material trackMaterial;
    private Material groundMaterial;
    private RenderTexture _splatmap;
    RaycastHit _groundHit;
    int _layerMask;
    [Range(0,1)]
    public float _brushSize; //0.08
    [Range(0,1)]
    public float _brushStrenght; //0.6
    
    // Start is called before the first frame update
    void Start()
    {
        _layerMask = LayerMask.GetMask("Ground");
        trackMaterial = new Material(drawShader);
        trackMaterial.SetVector("_Color", Color.red);

        groundMaterial = Terrain.activeTerrain.materialTemplate;
        _splatmap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        groundMaterial.SetTexture("_Splat", _splatmap);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _wheel.Length; i++) {
            if (Physics.Raycast(_wheel[i].position, -Vector3.up, out _groundHit)) //, 1f, _layerMask))
            {
                trackMaterial.SetVector("_Coordinate", new Vector4(_groundHit.textureCoord.x, _groundHit.textureCoord.y, 0,0));
                trackMaterial.SetFloat("_Strength", _brushStrenght);
                trackMaterial.SetFloat("_Size", _brushSize);
                RenderTexture temp = RenderTexture.GetTemporary(_splatmap.width, _splatmap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(_splatmap, temp);
                Graphics.Blit(temp, _splatmap, trackMaterial);
                RenderTexture.ReleaseTemporary(temp);                
            }
        }
    }
}

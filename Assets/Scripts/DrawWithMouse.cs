using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWithMouse : MonoBehaviour
{
    public Camera _camera;
    public Shader _drawShader;

    private RenderTexture _splatmap;
    private Material _groundMaterial, _trackMaterial;
    private RaycastHit _hit;

    [Range(0,10)]
    public float _brushSize;
    [Range(0,1)]
    public float _brushStrenght;

    // Start is called before the first frame update
    void Start()
    {
        _trackMaterial = new Material(_drawShader);
        _trackMaterial.SetVector("_Color", Color.red);

        _groundMaterial = Terrain.activeTerrain.materialTemplate;
        _splatmap = new RenderTexture(4096, 4096, 0, RenderTextureFormat.ARGBFloat);
        _groundMaterial.SetTexture("_Splat", _splatmap);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out _hit))
            {
                _trackMaterial.SetVector("_Coordinate", new Vector4(_hit.textureCoord.x, _hit.textureCoord.y, 0,0));
                _trackMaterial.SetFloat("_Strength", _brushStrenght);
                _trackMaterial.SetFloat("_Size", _brushSize);
                RenderTexture temp = RenderTexture.GetTemporary(_splatmap.width, _splatmap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(_splatmap, temp);
                Graphics.Blit(temp, _splatmap, _trackMaterial);
                RenderTexture.ReleaseTemporary(temp);
            }
        }
    }

    private void OnGUI()
    {
        //GUI.DrawTexture(new ReadOnlyCollectionBase(0,0,256,256), _splatmap, ScaleMode.ScaleToFit, false, 1);
    }
}

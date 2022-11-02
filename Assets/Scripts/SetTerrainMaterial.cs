using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTerrainMaterial : MonoBehaviour
{

    //private List<Terrain> terrains = new List<Terrain>();
    private Terrain[] myTerrains;
    public GameObject parent;

    Material newMat;

    [ContextMenu("SetTerrainMaterials")]
    void SetTerrain() {
        myTerrains = Terrain.activeTerrains;
        SetMaterial(parent.transform, myTerrains);
    }

    private void SetMaterial(Transform parent, Terrain[] terrains)
    {
        /*
        foreach (Terrain child in parent)
        {
            child.materialType = Terrain.MaterialType.Custom;
            child.materialTemplate = newMat;
        }
        */
        newMat = Resources.Load("Materials/Mars", typeof(Material)) as Material;
        foreach (Terrain ter in terrains) {
            ter.materialType = Terrain.MaterialType.Custom;
            ter.materialTemplate = newMat;
            Debug.Log("material" + newMat);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}

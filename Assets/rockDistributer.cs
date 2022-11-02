using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockDistributer : MonoBehaviour
{

    public List<GameObject> rocks = new List<GameObject>();
    public float range;    
    //public Terrain terrain;
    public int numberOfObjects; // number of objects to place
    public GameObject camera;

    private static ILogger logger = Debug.unityLogger;
    private TerrainData marsTerrainData;
    public int currentObjects; // number of placed objects
    private int terrainWidth; // terrain size (x)
    private int terrainLength; // terrain size (z)
    private int terrainPosX; // terrain position x
    private int terrainPosZ; // terrain position z

    private void AddDescendantsWithTag(Transform parent, string tag, List<GameObject> list)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.tag == tag)
            {
                list.Add(child.gameObject);
            }
            AddDescendantsWithTag(child, tag, list);
        }
    }

    // Start is called before the first frame update
    void Start()
    {        
        marsTerrainData = Terrain.activeTerrain.terrainData;
        
        // terrain size x
        terrainWidth = (int)marsTerrainData.size.x;
        // terrain size z
        terrainLength = (int)marsTerrainData.size.z;
        // terrain x position
        terrainPosX = (int)Terrain.activeTerrain.transform.position.x;
        // terrain z position
        terrainPosZ = (int)Terrain.activeTerrain.transform.position.z;

        Terrain[] terrains = Terrain.activeTerrains;
        logger.Log(terrains);
    }

    void generateObject() {        
        //int posx = Random.Range(terrainPosX, terrainPosX + terrainWidth);
        int posx = (int)Random.Range(camera.transform.position.x - range, camera.transform.position.x + range);
        // generate random z position
        int posz = (int)Random.Range(camera.transform.position.z - range, camera.transform.position.z + range);
        // get the terrain height at the random position
        float posy = Terrain.activeTerrain.SampleHeight(new Vector3(posx, 0, posz));
        // create new gameObject on random position

        // Randomly select a rock prefab and deploy on terrain with random size and orientation
        float random = Random.Range(0f, rocks.Count);
        GameObject objectToPlace = rocks[(int)random];

        // Find its local position scaled by the terrain size (to find the real world position)        
        float chooseSize = Random.value;
        float sizemax, sizemin;
        if (chooseSize < 0.95) {
            sizemin = 0.05f;
            sizemax = 0.4f;
        } else if (chooseSize < 0.98) {
            sizemin = 0.4f;
            sizemax = 0.8f;
        } else {
            sizemin = 1.0f;
            sizemax = 1.2f;
        }
        float scale = Random.Range(sizemin, sizemax);
        objectToPlace.transform.localScale = new Vector3(scale,scale,scale);
        objectToPlace.tag = "rock";
        SphereCollider collider = objectToPlace.AddComponent<SphereCollider>();
        collider.center = Vector3.zero;
        collider.radius = scale*0.5f;

        GameObject newObject = (GameObject)Instantiate(objectToPlace, new Vector3(posx, posy, posz), Random.rotation);
    }
    
    public void changeObject(GameObject rock) {
        int posx = (int)Random.Range(camera.transform.position.x - range, camera.transform.position.x + range);
        int posz = (int)Random.Range(camera.transform.position.z - range, camera.transform.position.z + range);
        float posy = Terrain.activeTerrain.SampleHeight(new Vector3(posx, 0, posz));

        float chooseSize = Random.value;
        float sizemax, sizemin;
        if (chooseSize < 0.95) {
            sizemin = 0.05f;
            sizemax = 0.4f;
        } else if (chooseSize < 0.98) {
            sizemin = 0.4f;
            sizemax = 0.8f;
        } else {
            sizemin = 1.0f;
            sizemax = 1.2f;
        }
        float scale = Random.Range(sizemin, sizemax);
        rock.transform.localScale = new Vector3(scale,scale,scale);
        rock.transform.position = new Vector3(posx,posy,posz);
    }

    // Update is called once per frame
    void Update()
    {
        
        
        // generate objects
        if(currentObjects <= numberOfObjects)
        {
            generateObject();
            currentObjects += 1;
        }

        if(currentObjects == numberOfObjects)
        {
            Debug.Log("Generate objects complete!");
        }
        
    }
}



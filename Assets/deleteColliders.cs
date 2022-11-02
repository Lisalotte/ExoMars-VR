using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteColliders : MonoBehaviour
{

    public GameObject rock;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Destroy colliders")]
    void destroyThem() {        
        SphereCollider[] cols = rock.GetComponents<SphereCollider>();
        foreach(SphereCollider col in cols)
            DestroyImmediate(col);
            Debug.Log("Collider destroyed");
    }
}

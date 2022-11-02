using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class drawUI : MonoBehaviour
{
    // When added to an object, draws colored rays from the
    // transform position.
    public int lineCount = 100;
    public float radius = 3.0f;

    public Camera referenceCamera;

    static Material lineMaterial;

    
    private float w, h;
    private float scale = 100f;
    
    private float xRot, yRot, zRot;

    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    // Will be called after all regular rendering is done
    public void OnRenderObject()
    {
        CreateLineMaterial();
        // Apply the line material
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        // Set transformation matrix for drawing to
        // match our transform
        GL.MultMatrix(transform.localToWorldMatrix);

        // Draw lines
        GL.Begin(GL.LINES);
        /*
        for (int i = 0; i < lineCount; ++i)
        {
            float a = i / (float)lineCount;
            float angle = a * Mathf.PI * 2;

            // Vertex colors change from red to green
            GL.Color(new Color(a, 1 - a, 0, 0.8F));
            // One vertex at transform position
            GL.Vertex3(-0.45f*w - 0.5f*i, 0.25f*h, 0);
            // Another vertex at edge of circle
            //GL.Vertex3(w - Mathf.Cos(angle) * radius, h - Mathf.Sin(angle) * radius, 0);
            GL.Vertex3(-0.45f*w - 0.5f*i, -0.25f*h, 0);

            GL.Vertex3(-0.4f*w, -0.3f*h + 0.5f*i, 0);
            GL.Vertex3(0.4f*w, -0.3f*h + 0.5f*i, 0);
        }*/
        GL.End();

        float x = -0.5f*w;
        float y = 0.05f*h;

        GL.Begin(GL.QUADS);
        GL.Color(new Color(1, 1, 1, 0.8F));
        GL.Vertex3(x, xRot+y, 0);
        GL.Vertex3(x+0.2f*scale, xRot+y+0.15f*scale, 0);
        GL.Vertex3(x+0.2f*scale, xRot+y-0.15f*scale, 0);
        GL.Vertex3(x, xRot+y, 0);
        GL.End();

        GL.PopMatrix();
    }

    public Text textVer;
    public Text textHor;
    public Camera trackingCam;

    public GameObject arrowVer;

    private void Start() {
        w = referenceCamera.pixelWidth;
        h = referenceCamera.pixelHeight;
    }

    private void Update() {
        
        xRot = -1*trackingCam.transform.rotation.eulerAngles.x;
        if (xRot < -180) {
            xRot += 360;
        }
        yRot = -1*trackingCam.transform.rotation.eulerAngles.y+180f;
        if (yRot < -180) {
            yRot += 360;
        }

        textVer.text = (xRot).ToString("F0");
        textHor.text = (yRot).ToString("F0");

               
    }
}

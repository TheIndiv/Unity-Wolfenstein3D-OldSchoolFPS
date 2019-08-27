using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class paintScript : MonoBehaviour
{
    Mesh mesh;
    Color32[] colors;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().sharedMesh;
        colors = new Color32[mesh.vertices.Length];
    }

    public void ApplyPaint()
    {
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.red;
        }
        mesh.colors32 = colors;
    }
}

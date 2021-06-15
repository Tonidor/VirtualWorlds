using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tesselation : MonoBehaviour
{
    [Range(1, 1000)]
    public int tesselation = 100;

    [Range(1, 100)]
    public float size = 5;

    Vector3 p = new Vector3();
    Vector2 uv = new Vector2();

    public void Tesselate() {
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        // Creating a mesh object.
        Mesh mesh = meshFilter.sharedMesh;
        if (mesh)
            mesh.Clear(false);
        else {
            mesh = new Mesh();
            mesh.name = "Terrain2DMesh";
            meshFilter.sharedMesh = mesh;
        }

        Vector3[] vertices = new Vector3[(tesselation + 1) * (tesselation + 1)];
        Vector2[] uvs = new Vector2[(tesselation + 1) * (tesselation + 1)];

        int i = 0;
        float scale = size / tesselation;
        float offW = -size / 2f;
        float offD = -size / 2f;
        for (int d = 0; d <= tesselation; d++) {
            uv.y = d / (float)tesselation;

            for (int w = 0; w <= tesselation; w++) {
                float x = scale * w + offW;
                float z = scale * d + offD;
                float y = 0;

                uv.x = w / (float)tesselation;

                uvs[i] = uv;

                p.Set(x, y, z);
                vertices[i] = p; // new Vector3(w, 0, d) - new Vector3(width / 2f, 0, depth / 2f);

                i++;
            }
        }

        int[] triangles = new int[tesselation * tesselation * 2 * 3]; // 2 - polygon per quad, 3 - corners per polygon

        for (int d = 0; d < tesselation; d++) {
            for (int w = 0; w < tesselation; w++) {
                // quad triangles index.
                int ti = (d * (tesselation) + w) * 6; // 6 - polygons per quad * corners per polygon

                // First tringle
                triangles[ti] = (d * (tesselation + 1)) + w;
                triangles[ti + 1] = ((d + 1) * (tesselation + 1)) + w;
                triangles[ti + 2] = ((d + 1) * (tesselation + 1)) + w + 1;

                // Second triangle
                triangles[ti + 3] = (d * (tesselation + 1)) + w;
                triangles[ti + 4] = ((d + 1) * (tesselation + 1)) + w + 1;
                triangles[ti + 5] = (d * (tesselation + 1)) + w + 1;
            }
        }

        // Assigning vertices, triangles and UV to the mesh.
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        //		Debug.Log("Tesselate");

        //var e = GetComponent<Environment2D>();
        //if (e != null) e.UpdateTerrain();
    }
}
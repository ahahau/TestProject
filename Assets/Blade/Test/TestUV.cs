using System;
using UnityEngine;

namespace Blade.Test
{
    public class TestUV : MonoBehaviour
    {
        private MeshFilter _meshFilter;

        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
        }

        private void Start()
        {
            Mesh mesh = new Mesh();

            Vector3[] vertices = new Vector3[4];
            Vector2[] uvs = new Vector2[4];
            int[] triangles = new int[6];

            vertices[0] = new Vector3(0, 0);
            vertices[1] = new Vector3(0, 5);
            vertices[2] = new Vector3(5, 5);
            vertices[3] = new Vector3(5, 0);

            //uv는 반드시 0~1까지의 값을 가진다.
            uvs[0] = new Vector2(0, 0);
            uvs[1] = new Vector2(0, 1);
            uvs[2] = new Vector2(1, 1);
            uvs[3] = new Vector2(1, 0);
            
            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            triangles[3] = 0;
            triangles[4] = 2;
            triangles[5] = 3;
            
            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            
            _meshFilter.mesh = mesh;
        }
    }    
}

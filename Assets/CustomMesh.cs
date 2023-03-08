using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMesh : MonoBehaviour
{
    //public int MeshGridSize = 4;
    public int MeshGridSize_x ;
    public int MeshGridSize_y ;
    public float MeshGridLength;
    private Vector3[] Verts;
    private Vector2[] UVs;
    private int[] tris;
    private int counter = 0;
    private int triscounter = 0;
    private Vector2[] perlinArr;

    public Mesh mesh;
    public GameObject TargetModel;



    private void CreatePerlinArr()
    {
        perlinArr = new Vector2[MeshGridSize_x * MeshGridSize_y];
        
        for (int i = 0; i< perlinArr.Length; i++)
        {
            perlinArr[i] = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
        }
    }
    private int getVertix(int x, int y)
    {
        return y * MeshGridSize_x + x;
    }

    private int getTriix(int x, int y)
    {
        return (y * (MeshGridSize_x - 1) + x) * 6;
    }

    void Awake()
    {
        
        CreatePerlinArr();
        mesh = new Mesh();
        Verts = new Vector3[MeshGridSize_x*MeshGridSize_y];
         UVs = new Vector2[MeshGridSize_x*MeshGridSize_y];
         tris = new int[(MeshGridSize_x-1) *(MeshGridSize_y-1)*6];

        
        GetComponent<MeshFilter>().mesh = mesh;

        for (int y = 0; y < MeshGridSize_y; y++)
        {
            for (int x= 0; x < MeshGridSize_x; x++)
            {
                int ix = getVertix(x, y);
                float dx = x - MeshGridSize_x / 2;
                float dy = y - MeshGridSize_y / 2;

                float r2 = dx*dx+dy*dy;
                float alpha = 0.1f;
                float coeff = (1f + alpha * Mathf.Pow(r2, 0.25f));

                Verts[ix] = new Vector3(MeshGridSize_x/2 + dx*coeff, 0, Mathf.Pow(y,1.25f));

                UVs[ix] = new Vector2(y, x);
            }
        }
        MeshGridLength =  Mathf.Pow(MeshGridSize_y-1, 1.25f);

        for (int y = 0; y<MeshGridSize_y-1; y++)
        {
            for (int x =0; x<MeshGridSize_x-1; x++)
            {
                int triix = getTriix(x, y);
                tris[triix] = getVertix(x, y);
                tris[triix + 1] = getVertix(x, y + 1);
                tris[triix + 2] = getVertix(x + 1, y + 1);
                tris[triix + 3] = getVertix(x + 1, y + 1);
                tris[triix + 4] = getVertix(x + 1, y);
                tris[triix + 5] = getVertix(x, y);

            }
           
        }

        mesh.vertices = Verts;
        mesh.uv = UVs;
        mesh.triangles = tris;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();


        GetComponent<Renderer>().material.SetFloat("_gridSize", MeshGridSize_x);
    }

    // Update is called once per frame
    void Update()
    {
        //MergeToModel();
        //PerlineGrid();
        
        
    }

    public void PerlineGrid()
    {
        //mesh = new Mesh();
        mesh = GetComponent<MeshFilter>().mesh;

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            //float xCoord = Verts[i].x / 4 + Time.time;
            //float yCoord = Verts[i].z / 4 + Time.time;
            Verts[i] = new Vector3(Verts[i].x, Mathf.PerlinNoise(perlinArr[i].x + (Time.time / 10), perlinArr[i].y + (Time.time / 10)) * 10, Verts[i].z);
            
        }
        //Debug.Log("vert 1 " + Verts[1].y);
       // Debug.Log("vert 2 " + Verts[20].y);
        mesh.vertices = Verts;

        //Debug.Log(mesh.vertices.Length);
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
    public void MergeToModel()
    {

            mesh.vertices = TargetModel.GetComponent<MeshFilter>().mesh.vertices;

        //mesh.vertices[1] = new Vector3(0, 100, 0);
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}

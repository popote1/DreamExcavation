using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TerrainGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public int cellSize = 1;
    public float DepthIntencity = 2;
    public float Flatlevel = 30;
    public bool Inupdate;
    [Range(0,1)]public float Zome = 0.5f;
    public Gradient Gradient;
    public GameObject prefhabeCell;

    public void Update()
    {
        if (Inupdate)
        {
            MakeNewMesh();
        }
    }


    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangle;
    private Color[] colors;
    private float _depthMax;
    private float _depthMin;
    private GameObject[,] cells;

void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
    }

    // Update is called once per frame
    
    [ContextMenu("CreateMesh")]
    void MakeNewMesh()
    {
        GenerateMesh();
        UpdateMesh();
    }

    public void GenerateMesh()
    {
        _depthMax = Mathf.NegativeInfinity;
        _depthMin = Mathf.Infinity;
        _vertices = new Vector3[(height + 1) * (width + 1)];
        for (int i = 0, y = 0; y <= height; y++)
        {
            for (int x = 0; x <= width; x++)
            {
                float z = Mathf.PerlinNoise(x * Zome, y * Zome) * DepthIntencity;
                if (z < Flatlevel) z = Flatlevel-1;

                if (z > _depthMax) _depthMax = z;
                if (z < _depthMin) _depthMin = z;
                
                
                _vertices[i] = new Vector3(x * cellSize, y * cellSize, z);
                i++;
            }
        }

        _triangle = new int[width * height * 6];
        int vert = 0;
        int tris = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                _triangle[tris + 0] = vert + 0;
                _triangle[tris + 1] = vert + width + 1;
                _triangle[tris + 2] = vert + 1;
                _triangle[tris + 3] = vert + 1;
                _triangle[tris + 4] = vert + width + 1;
                _triangle[tris + 5] = vert + width + 2;
                vert++;
                tris += 6;
            }

            vert++;
        }

        colors = new Color[_vertices.Length];
        for (int i=0,y = 0; y < height; y++) {
            for (int x = 0; x < width; x++)
            {
                float depth =Mathf.InverseLerp(_depthMin, _depthMax ,_vertices[i].z);
                colors[i] = Gradient.Evaluate(depth);
                i++;
            }
        }
    }

    public void UpdateMesh()
    {
        _mesh.Clear();

        _mesh.vertices = _vertices;
        _mesh.triangles = _triangle;
        _mesh.colors = colors;
        _mesh.RecalculateNormals();
    }
[ContextMenu("Generate playble aera")]
    public void GeneratePlaybleMap()
    {
        cells = new GameObject[width, height];
        int vert = 0;
        int tris = 0;

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                if (_vertices[vert + 0].z == _vertices[vert + width + 1].z &&
                    _vertices[vert + 1].z == _vertices[vert + width + 2].z &&
                    _vertices[vert + 1].z == _vertices[vert + width + 1].z)
                {
                    cells[x, y] = Instantiate(prefhabeCell,
                        new Vector3(x * cellSize, y * cellSize, _vertices[vert + 1].z - 0.5f) + new Vector3(0.5f, 0.5f),
                        quaternion.identity);
                }
                vert++;
                tris += 6;
            }
            vert++;
        }
    }
}


using System.Collections.Generic;
using Scripts.Main;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshFilter))]
public class TerrainGeneratorV2 : MonoBehaviour
{
    public int width;
    public int height;
    public int cellSize = 1;
    [Range(0,1)]public float Flatlevel = 0.5f;
    public bool Inupdate;
    [Range(0,1)]public float Zome = 0.5f;
    public Vector2 GroundOffSet = new Vector2(1,1);
    public Gradient Gradient;
    public Vector2Int MinGRoundPos;
    public Vector2Int MaxGRoundPos;
    public PlayGridV2 playgrid;

    [Header("Rouds paramettres")] 
    public List<Vector2Int> roudsStartPos;
    [Range(0, 100)] public float ChanceDeDevier=25;
    public GameObject PrefabPont;
    public int LimiteBorders = 2;
    private List<Vector2Int> _roudCells = new List<Vector2Int>();
    
    
    [Header("TreeGenerator")] 
    public bool TreeInUpdate;
    public GameObject prefhabeCell;
    [Range(0,1)]public float TreeZome = 0.5f;
    public Vector2 TreeOffSet = new Vector2(1,1);
    public bool IsUsTreeTreashHosld;
    [Range(0,1)]public float TreeThreshHold = 0.5f;
    public GameObject PrefabTree;

    [Header("TreeModifier1")] 
    public bool IsUsingTreeMidifier;
    [Range(0,1)]public float TreeModifier1StartStrenght = 0.5f;
    [Range(0,100)]public float TreeModifier1StartNewNoise = 50f;
    [Range(0,1)]public float TreeModifier1Zome = 0.5f;
    public Vector2 TreeModifier1OffSet = new Vector2(1,1);
    
    [Header("TreeModifier2")]
    public bool IsUsingTreeMidifier2;
    [Range(0,1)]public float TreeModifier2StartStrenght = 0.5f;
    [Range(0,100)]public float TreeModifier2StartNewNoise = 50f;
    
    
    public void Update()
    {
        if (Inupdate) MakeNewMesh();
        if (TreeInUpdate)GeneratTreeMap();
        
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
    public void MakeNewMesh()
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
                float z;
                if(x>MinGRoundPos.x&x<MaxGRoundPos.x&&y>MinGRoundPos.y&&y<MaxGRoundPos.y)
                {
                    z = 0;}
                else
                {
                    z = Mathf.PerlinNoise(GroundOffSet.x+x * Zome, GroundOffSet.y+y * Zome);
                    if (z < Flatlevel) z = 0;
                    else z = 2;
                }


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
        bool[,] waterMap = new bool[width, height];

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                if (_vertices[vert + 0].z ==0 && _vertices[vert + 1].z == 0 && _vertices[vert + width + 1].z == 0&& _vertices[vert + width + 2].z == 0)
                {
                    cells[x, y] = Instantiate(prefhabeCell,
                        new Vector3(x * cellSize, y * cellSize, _vertices[vert + 1].z - 0.5f) + new Vector3(0.5f, 0.5f), 
                        quaternion.identity);
                    waterMap[x, y] = false;
                }
                else
                {
                    waterMap[x, y] = true;
                }
                vert++;
                tris += 6;
            }
            vert++;
        }
        playgrid.InjectTerraindata(waterMap);
    }

    [ContextMenu("Generate Rouds")]
    public void GeneratRouds()
    {
        bool[,] RoadMap = new bool[width, height];
        foreach (Vector2Int startPo in roudsStartPos)
        {
            int x = startPo.x;
            int y = startPo.y;
            while (y < height-1) {
                if (Random.Range(0, 100) < ChanceDeDevier) {
                    if (Random.Range(0, 100) < 50) {
                        x++;
                        if (x > width - LimiteBorders) x -= 2;
                    }
                    else {
                        x--;
                        if (x < LimiteBorders) x += 2;
                    }
                }
                else {
                    y++;
                }


                if (cells[x, y] != null) {
                    _roudCells.Add(new Vector2Int(x, y));
                }
                else {
                    cells[x, y] = Instantiate(prefhabeCell,
                        new Vector3(x * cellSize, y * cellSize, 0.5f) + new Vector3(0.5f, 0.5f),
                        quaternion.identity);
                    Instantiate(PrefabPont, new Vector3(x+0.5f, y+0.5f, 0.5f), Quaternion.identity);
                    _roudCells.Add(new Vector2Int(x, y));
                }
            }
        }
        foreach (Vector2Int cell in _roudCells) {
            cells[cell.x,cell.y].GetComponent<SpriteRenderer>().color = Color.red;
            RoadMap[cell.x, cell.y] = true;
        }
        playgrid.InjectRoadMap(RoadMap);
    }
    
    [ContextMenu("Generate Tree Map")]
    public void GeneratTreeMap()
    {
        for (int i = 0, y = 0; y <= height-1; y++)
        {
            for (int x = 0; x <= width-1; x++)
            {
                if (cells[x, y] != null)
                {

                    float cellValue = Mathf.PerlinNoise(TreeOffSet.x + x * TreeZome, TreeOffSet.y + y * TreeZome);

                    if (IsUsingTreeMidifier)
                    {
                        float cellValue2 = Mathf.PerlinNoise(TreeModifier1OffSet.x + x * TreeModifier1Zome, TreeModifier1OffSet.y + y * TreeModifier1Zome);
                        float a = TreeModifier1StartNewNoise * height / 100;
                        float c = (y - a)*TreeModifier1StartStrenght / (height - a);
                        cellValue = Mathf.Lerp(cellValue, cellValue2,  c);
                    }

                    if (IsUsingTreeMidifier2)
                    {
                        float a = TreeModifier2StartNewNoise * height / 100;
                        float c = (y - a) *TreeModifier2StartStrenght/ (height - a);
                        cellValue = Mathf.Lerp(cellValue, 0,  c);
                        
                    }


                    if (IsUsTreeTreashHosld)
                    {
                        if (cellValue < TreeThreshHold) {
                            cells[x, y].GetComponent<SpriteRenderer>().color = Color.white;
                        }
                        else {
                            cells[x, y].GetComponent<SpriteRenderer>().color = Color.black;
                        }
                    }
                    else
                    {
                        cells[x, y].GetComponent<SpriteRenderer>().color = Color.white * cellValue;
                    }
                }
            }
        }
    }
    [ContextMenu("spawn Tree")]
    public void SpawnTree()
    {
        bool[,] TreeMap = new bool[width, height];
        TreeInUpdate = false;
        for (int i = 0, y = 0; y <= height-1; y++) {
            for (int x = 0; x <= width-1; x++) {
                if (cells[x, y] != null) {
                    if (cells[x, y].GetComponent<SpriteRenderer>().color == Color.black&&!_roudCells.Contains(new Vector2Int(x,y))) {
                        GameObject gO = Instantiate(PrefabTree, cells[x, y].transform.position, Quaternion.identity);
                        gO.transform.localScale = gO.transform.localScale * Random.Range(0.7f, 1.1f);
                        Destroy(cells[x,y]);
                        TreeMap[x, y] = true;
                    }
                    else {
                        Destroy(cells[x,y]);
                    }
                }
            }
        }
        playgrid.InjectTreMap(TreeMap);
    }
}

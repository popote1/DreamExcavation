using Scripts.Helper;
using Unity.Mathematics;
using UnityEngine;

namespace Scripts.Main
{
    public class PlayGrid : MonoBehaviour
    {
        public int Height;
        public int Widht;
        public float CellSize = 1;
        public Vector3 Origin;
        public Cell[,] Cells;

        [Header("Flow Field Calulation")] public Vector2Int originePos;

        [Header("Debug")] 
        public bool ShowDebug;
        public Color GridColor= Color.green;
        public CellDebug CellDebug;
        public GameObject[,] arrows;
        void Start()
        {
            Cells = new Cell[Widht,Height];
            for (int x = 0; x < Widht; x++) {
                for (int y = 0; y < Height; y++) {
                    Cells[x,y] = new Cell(new Vector2Int(x,y),0 );
                    Cells[x, y].CellDebug = Instantiate(CellDebug, new Vector3(x + 0.5f, 0, y + 0.5f), quaternion.identity);
                }
            }
        }
        
        void Update()
        {
            if (ShowDebug)
            {
                Debug.Log("trace la grille");
                for (int x = 0; x < 1+Widht; x++) {
                    Debug.DrawLine(new Vector3(x,0,0),new Vector3(x,0,Height),GridColor);
                    Debug.Log("trace une logne de"+new Vector3(x,0,0)+"a"+new Vector3(x,0,Height));
                }
                for (int y = 0; y < Height+1; y++) {
                    Debug.DrawLine(new Vector3(0,0,y),new Vector3(Widht,0,y),GridColor);
                }
            }
            
        }

        public Vector2Int? GetCellByWorldPos(Vector3 worldPos) {
            Vector2Int value = new Vector2Int(Mathf.FloorToInt(worldPos.x/CellSize) ,Mathf.FloorToInt( worldPos.z/CellSize));
            if (CheckIsInGrid(value)) return value;
            return null;
        }

        public bool CheckIsInGrid(Vector2Int pos) {
            if (pos.x > -1 && pos.x < Widht && pos.y > -1 && pos.y < Height) return true;
            return false;
        }

        public Vector3 GetCellWorldPosByCell(Vector2Int pos) {
            return new Vector3(pos.x * CellSize, 0, pos.y * CellSize) + Origin;
        }
        public Vector3 GetCellCenterWorldPosByCell(Vector2Int pos) {
            return new Vector3(pos.x * CellSize, 0, pos.y * CellSize) + Origin+Vector3.one*CellSize/2;
        }

        public Cell GetCell(Vector2Int pos) {
            if (CheckIsInGrid(pos)) return Cells[pos.x, pos.y];
            return null;
        }

        [ContextMenu("Calculate FlowField")]
        public void CalculateFlowField() {
            FlowFieldHelper.CalculatFlowField(this,originePos);
        }
    }
}
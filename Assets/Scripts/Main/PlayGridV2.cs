using System.Collections;
using Scripts.Helper;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Scripts.Main
{
    public class PlayGridV2
     : MonoBehaviour
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

        public GameObject PrfabColider2D;
       /* void Start()
        {
            Cells = new Cell[Widht,Height];
            for (int x = 0; x < Widht; x++) {
                for (int y = 0; y < Height; y++) {
                    Cells[x,y] = new Cell(new Vector2Int(x,y),0 , this);
                    Cells[x, y].CellDebug = Instantiate(CellDebug, new Vector3(x + 0.5f, 0, y + 0.5f), quaternion.identity);
                }
            }
        }*/

       public void InjectTerraindata(bool[,]waterMap)
       {
           Cells = new Cell[Widht,Height];
           for (int x = 0; x < Widht; x++) {
               for (int y = 0; y < Height; y++) {
                   Cells[x,y] = new Cell(new Vector2Int(x,y),0 , this);
                   Cells[x, y].CellDebug = Instantiate(CellDebug, new Vector3(x + 0.5f, y + 0.5f,-0.05f ), quaternion.identity);
                   Cells[x, y].Iswall = waterMap[x, y];
                   if (waterMap[x, y])
                   {
                       Cells[x, y].IndividualMoveValue = 1000;
                   }
               }
           }
       }

       public void InjectRoadMap(bool[,] RoadMap)
       {
           for (int x = 0; x < Widht; x++) {
               for (int y = 0; y < Height; y++) {
                   if (RoadMap[x, y])
                   {
                       Cells[x, y].IndividualMoveValue = 0;
                       Cells[x, y].Iswall = false;
                   }
               }
           }
       }
       public void InjectTreMap(bool[,] TreeMap)
       {
           for (int x = 0; x < Widht; x++) {
               for (int y = 0; y < Height; y++) {
                   if (TreeMap[x, y])
                   {
                       Cells[x, y].IndividualMoveValue = 100;
                       Cells[x, y].DradFactor = 3;
                   }
               }
           }
       }
        void Update()
        {
            if (ShowDebug)
            {
               
                for (int x = 0; x < 1+Widht; x++) {
                    Debug.DrawLine(new Vector3(x,0,0),new Vector3(x,Height,0),GridColor);
                }
                for (int y = 0; y < Height+1; y++) {
                    Debug.DrawLine(new Vector3(0,y,0),new Vector3(Widht,y,0),GridColor);
                }
            }
            
        }

        public Vector2Int GetCellByWorldPos(Vector3 worldPos) {
            Vector2Int value = new Vector2Int(Mathf.FloorToInt(worldPos.x/CellSize) ,Mathf.FloorToInt( worldPos.y/CellSize));
            if (CheckIsInGrid(value)) return value;
            return new Vector2Int(-1,-1);
        }

        public bool CheckIsInGrid(Vector2Int pos) {
            if (pos.x > -1 && pos.x < Widht && pos.y > -1 && pos.y < Height) return true;
            return false;
        }

        public Vector3 GetCellWorldPosByCell(Vector2Int pos) {
            return new Vector3(pos.x * CellSize,  pos.y * CellSize) + Origin;
        }
        public Vector3 GetCellCenterWorldPosByCell(Vector2Int pos) {
            return new Vector3(pos.x * CellSize,  pos.y * CellSize) + Origin+Vector3.one*CellSize/2;
        }

        public Cell GetCell(Vector2Int pos) {
            if (CheckIsInGrid(pos)) return Cells[pos.x, pos.y];
            return null;
        }

        [ContextMenu("Calculate FlowField")]
        public void CalculateFlowField() {
           // FlowFieldHelper.CalculatFlowField(this,originePos);
           StartCoroutine(CalculateFlowFieldCorutine());
        }

        public List<Vector2Int> GetNeibors(Vector2Int pos)
        {
            List<Vector2Int> neibors = new List<Vector2Int>();
            if(CheckIsInGrid(pos+new Vector2Int(1,0)))neibors.Add(pos+new Vector2Int(1,0));
            if(CheckIsInGrid(pos+new Vector2Int(1,1)))neibors.Add(pos+new Vector2Int(1,1));
            if(CheckIsInGrid(pos+new Vector2Int(0,1)))neibors.Add(pos+new Vector2Int(0,1));
            if(CheckIsInGrid(pos+new Vector2Int(-1,1)))neibors.Add(pos+new Vector2Int(-1,1));
            if(CheckIsInGrid(pos+new Vector2Int(-1,0)))neibors.Add(pos+new Vector2Int(-1,0));
            if(CheckIsInGrid(pos+new Vector2Int(-1,-1)))neibors.Add(pos+new Vector2Int(-1,-1));
            if(CheckIsInGrid(pos+new Vector2Int(0,-1)))neibors.Add(pos+new Vector2Int(0,-1));
            if(CheckIsInGrid(pos+new Vector2Int(1,-1)))neibors.Add(pos+new Vector2Int(1,-1));
            return neibors;
        }

        public bool CheckCellIsWall(Vector2Int pos)
        {
            if (CheckIsInGrid(pos)) {
                return GetCell(pos).Iswall;
                
            }
            return true;
        }
       IEnumerator CalculateFlowFieldCorutine()
        {
            Debug.Log("Cacule le flowfield");
            List<Vector2Int> OpenList = new List<Vector2Int>();
            List<Vector2Int> temporalToAdd = new List<Vector2Int>();
            foreach (Cell cell in Cells) {
                cell.SetMoveValue(int.MaxValue);
            }
            OpenList.Add(originePos);
            GetCell(originePos).SetMoveValue(0);
            while (OpenList.Count > 0)
            {
                foreach (Vector2Int cell in OpenList) {
                    foreach (Vector2Int neibors in GetNeibors(cell)) {
                        if ((neibors-cell).magnitude > 1) {
                            if (GetCell(neibors).MoveValue >GetCell(cell).MoveValue + 14 + GetCell(neibors).IndividualMoveValue&&!CheckCellIsWall(new Vector2Int(cell.x,neibors.y))&&!CheckCellIsWall(new Vector2Int(neibors.x,cell.y))) {
                               GetCell(neibors).SetMoveValue( GetCell(cell).MoveValue + 14 +GetCell(neibors).IndividualMoveValue);
                                temporalToAdd.Add(neibors);
                                Vector2Int oriantation = cell - neibors;
                                GetCell(neibors).SetFlowField2DVector(new Vector2(oriantation.x,oriantation.y));
                            } 
                        }
                        else {
                            if (GetCell(neibors).MoveValue > GetCell(cell).MoveValue + 10 + GetCell(neibors).IndividualMoveValue) {
                                GetCell(neibors).SetMoveValue( GetCell(cell).MoveValue + 10 + GetCell(neibors).IndividualMoveValue);
                                temporalToAdd.Add(neibors);
                                Vector2Int oriantation = cell - neibors;
                                GetCell(neibors).SetFlowField2DVector(new Vector3(oriantation.x,oriantation.y));
                            } 
                        }
                    }
                    
                }
                OpenList.Clear();
                OpenList.AddRange(temporalToAdd);
                temporalToAdd.Clear();
                yield return new WaitForSeconds(0.01f);
            }
            yield return null;
        }

       [ContextMenu("GeneratwaterColliders")]
       public void GeneratWarterColiders()
       {
           for (int x = 0; x < Widht; x++) {
               for (int y = 0; y < Height; y++) {
                   if ( Cells[x, y].Iswall)
                   {
                       Instantiate(PrfabColider2D, GetCellCenterWorldPosByCell(new Vector2Int(x, y)),
                           quaternion.identity);
                   }
               }
           }
       }

        public List<Vector2Int> GetNeigbor(Vector2Int pos)
        {
            List<Vector2Int> neibors = new List<Vector2Int>();
            if (CheckIsInGrid(pos+new Vector2Int(1,0))){neibors.Add(pos+new Vector2Int(1,0));}
            if (CheckIsInGrid(pos+new Vector2Int(1,1))){neibors.Add(pos+new Vector2Int(1,1));}
            if (CheckIsInGrid(pos+new Vector2Int(0,1))){neibors.Add(pos+new Vector2Int(0,1));}
            if (CheckIsInGrid(pos+new Vector2Int(0,-1))){neibors.Add(pos+new Vector2Int(0,-1));}
            if (CheckIsInGrid(pos+new Vector2Int(-1,-1))){neibors.Add(pos+new Vector2Int(-1,-1));}
            if (CheckIsInGrid(pos+new Vector2Int(-1,0))){neibors.Add(pos+new Vector2Int(-1,0));}
            if (CheckIsInGrid(pos+new Vector2Int(1,-1))){neibors.Add(pos+new Vector2Int(1,-1));}
            if (CheckIsInGrid(pos+new Vector2Int(-1,1))){neibors.Add(pos+new Vector2Int(-1,1));}
            return neibors;
        }
    }
}
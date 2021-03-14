using System;
using Scripts.Actors;
using Scripts.Main;
using Unity.Mathematics;
using UnityEngine;

namespace Scripts.Interaction
{
    public class GridController :MonoBehaviour
    {
        public GameObject PrefabCursor;
        public PlayGrid Grid;
        [Range(0, 50)] public float cursorSmoothFactor = 9f;
        public MoveActor prefabCube;
        
        
        private Camera _camera;
        private Vector3 _cursorPos = Vector3.zero;
        private Transform _cursor;

        private void Awake()
        {
            _camera = Camera.main;
            _cursor = Instantiate(PrefabCursor, Vector3.zero, quaternion.identity).transform;
        }

        private void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit)) {
                Vector2Int selectedCellT = Grid.GetCellByWorldPos(hit.point);
                Vector2Int selectedCell;
                if (selectedCellT != new Vector2Int(-1,-1)) {
                    selectedCell = selectedCellT;
                    _cursorPos = Grid.GetCellCenterWorldPosByCell(selectedCell);
                    if (Input.GetButtonDown("Fire1"))
                    {
                        Grid.Cells[selectedCell.x,selectedCell.y].AddMoveValue(10);
                    }

                    if (Input.GetButtonDown("Fire2"))
                    {
                        Grid.GetCell(selectedCell).Iswall = true;
                        Grid.Cells[selectedCell.x,selectedCell.y].AddMoveValue(500);
                    }

                    if (Input.GetButtonDown("Jump"))
                    {
                        Grid.originePos = selectedCell;
                        Grid.CalculateFlowField();
                    }

                    if (Input.GetKey("a"))
                    {
                       MoveActor MA= Instantiate(prefabCube, hit.point+new Vector3(0,0.5f,0), quaternion.identity);
                       MA.grid = Grid;
                    }
                }
            }
            _cursor.position = Vector3.Lerp(_cursor.position, _cursorPos, cursorSmoothFactor * Time.deltaTime);
        }
    }
}
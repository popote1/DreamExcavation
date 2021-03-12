using System;
using Scripts.GridActor;
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
        public GridMoverBase GridMoverBase;
        
        
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
                Vector2Int? selectedCellT = Grid.GetCellByWorldPos(hit.point);
                Vector2Int selectedCell;
                if (selectedCellT != new Vector2(-1,-1)) {
                    selectedCell = (Vector2Int) selectedCellT;
                    _cursorPos = Grid.GetCellCenterWorldPosByCell(selectedCell);
                    if (Input.GetButtonDown("Fire1"))
                    {
                        Grid.Cells[selectedCell.x,selectedCell.y].AddMoveValue();
                    }

                    if (Input.GetButtonDown("Jump"))
                    {
                        Grid.originePos = selectedCell;
                        Grid.CalculateFlowField();
                    }

                    if (Input.GetKeyDown("a"))
                    {
                        GridMoverBase gameO =Instantiate(GridMoverBase, hit.point + new Vector3(0, 0.5f, 0), Quaternion.identity);
                        gameO.Grid = Grid;
                    }
                }
            }
            _cursor.position = Vector3.Lerp(_cursor.position, _cursorPos, cursorSmoothFactor * Time.deltaTime);
        }
    }
}
using System;
using Scripts.Actors;
using Scripts.Main;
using Unity.Mathematics;
using UnityEngine;

namespace Scripts.Interaction
{
    public class GridControllerV2 :MonoBehaviour
    {
        public GameObject PrefabCursor;
        public PlayGridV2 Grid;
        [Range(0, 50)] public float cursorSmoothFactor = 9f;
        public MoveActorV2 prefabCube;
        public inputState InputState;
        public bool IsOnUI;
        
        private Camera _camera;
        private Vector3 _cursorPos = Vector3.zero;
        private Transform _cursor;

        public enum inputState
        {
            none,Actor, Destination
        }
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
                    if (Input.GetButton("Fire1")&&!IsOnUI)
                    {
                        //Grid.Cells[selectedCell.x,selectedCell.y].AddMoveValue(10);
                        if (InputState == inputState.Actor)
                        {
                            MoveActorV2 MA= Instantiate(prefabCube, hit.point+new Vector3(0,0.5f,0), quaternion.identity);
                            MA.grid = Grid;
                        }

                        if (InputState == inputState.Destination)
                        {
                            Grid.originePos = selectedCell;
                            Grid.CalculateFlowField();
                            InputState = inputState.none;
                        }
                    }
/*
                    if (Input.GetButtonDown("Fire2"))
                    {
                        Grid.GetCell(selectedCell).Iswall = true;
                        Grid.Cells[selectedCell.x,selectedCell.y].AddMoveValue(500);
                    }
                     if (Input.GetKey("i"))
                   {
                       Debug.Log ("la célulle "+selectedCell+" a une move value de "+ Grid.GetCell(selectedCell).MoveValue)
                   }

                    if (Input.GetKey("a"))
                    {
                       MoveActorV2 MA= Instantiate(prefabCube, hit.point+new Vector3(0,0.5f,0), quaternion.identity);
                       MA.grid = Grid;
                    }
                   if (Input.GetButtonDown("Jump"))
                   {
                       Grid.originePos = selectedCell;
                       Grid.CalculateFlowField();
                   }
                  */
                }
            }
            _cursor.position = Vector3.Lerp(_cursor.position, _cursorPos, cursorSmoothFactor * Time.deltaTime);
        }

        public void SetInputStateOnAdd()
        {
            InputState = inputState.Actor;
        }

        public void SetInputStateOnDestination()
        {
            InputState = inputState.Destination;
        }

        public void OnUIEnter()
        {
            IsOnUI = true;
            
        }

        public void OnUIExit()
        {
            IsOnUI = false;
        }
    }
}
using System;
using System.Collections.Generic;
using Scripts.Actors;
using Scripts.Main;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Interaction
{
    public class GridControllerV2 : MonoBehaviour
    {
        public GameObject PrefabCursor;
        public PlayGridV2 Grid;
        [Range(0, 50)] public float cursorSmoothFactor = 9f;
        public MoveActorV2 prefabCube;
        public GameObject PrefabTower;
        public GameObject BPPower;
        private inputState _inputState;

        public inputState InputState
        {
            get => _inputState;
            set
            {
                if (_inputState == inputState.building)
                {
                    foreach (Vector2Int vec in _preselectedCell) Grid.GetCell(vec).BuildingCell.SetActive(false);
                    _preselectedCell.Clear();
                }

                _inputState = value;
            }
        }

        public bool IsOnUI;
        public Text FPSCounter;
        public Text EntityCounter;

        private Camera _camera;
        private Vector3 _cursorPos = Vector3.zero;
        private Transform _cursor;
        private int _entityCount;
        private List<Vector2Int> _preselectedCell = new List<Vector2Int>();
        private TourelleFSM _selectedTourelle;
        private GameObject _powerCursor;

        public enum inputState
        {
            none,
            Actor,
            Destination,
            building,
            UsPower
        }

        private void Awake() {
            _camera = Camera.main;
            _cursor = Instantiate(PrefabCursor, Vector3.zero, quaternion.identity).transform;
        }

        private void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit)) {
                Vector2Int selectedCellT = Grid.GetCellByWorldPos(hit.point);
                Vector2Int selectedCell;
                if (selectedCellT != new Vector2Int(-1, -1)) {
                    selectedCell = selectedCellT;
                    _cursorPos = Grid.GetCellCenterWorldPosByCell(selectedCell);
                    if (Input.GetButton("Fire1") && !IsOnUI) {
                        //Grid.Cells[selectedCell.x,selectedCell.y].AddMoveValue(10);
                        if (InputState == inputState.Actor) {
                            MoveActorV2 MA = Instantiate(prefabCube, hit.point + new Vector3(0, 0.5f, 0),
                                quaternion.identity);
                            MA.grid = Grid;
                            _entityCount++;
                            EntityCounter.text = "Entity: " + _entityCount;
                        }

                        if (InputState == inputState.Destination) {
                            Grid.originePos = selectedCell;
                            Grid.CalculateFlowField();
                            InputState = inputState.none;
                        }

                        if (InputState == inputState.none) {
                            if (Grid.GetCell(selectedCell).tourelle != null &&
                                Grid.GetCell(selectedCell).tourelle != _selectedTourelle) {
                                if (_selectedTourelle!=null)_selectedTourelle.OnDeselect();
                                _selectedTourelle = Grid.GetCell(selectedCell).tourelle;
                                _selectedTourelle.OnSelect();
                                BPPower.SetActive(true);
                            }
                            else if(Grid.GetCell(selectedCell).tourelle == null) {
                                if (_selectedTourelle!=null)_selectedTourelle.OnDeselect();
                                _selectedTourelle = null;
                                BPPower.SetActive(false);
                            }
                        }
                    }

                    if (InputState == inputState.building)
                    {
                        List<Vector2Int> selected=GetBuildingVec2by2(selectedCell);
                        List<Vector2Int> tepo = new List<Vector2Int>();
                        foreach (Vector2Int cell in _preselectedCell) {
                            if (!selected.Contains(cell)) {
                                Grid.GetCell(cell).BuildingCell.SetActive(false);
                                tepo.Add(cell);
                            }
                        }

                        foreach (Vector2Int vec in tepo) _preselectedCell.Remove(vec);
                        if (Input.GetButtonUp("Fire1")&&selected.Count==4) {
                            buid(selected);
                            InputState = inputState.none;
                        }
                    }

                    if (InputState == inputState.UsPower)
                    {
                        if (Input.GetButtonUp("Fire1"))
                        {
                            _selectedTourelle.DoPower();
                            Destroy(_powerCursor);
                            InputState = inputState.none;
                        }
                    }
                }
            }

            FPSCounter.text = "FPS: " + 1f / Time.deltaTime;
            _cursor.position = Vector3.Lerp(_cursor.position, _cursorPos, cursorSmoothFactor * Time.deltaTime);
            if(_powerCursor!=null)_powerCursor.transform.position = hit.point;
        }

        public void UIPressButtonPower()
        {
            
        }

        public void SetInputStateOnAdd() { InputState = inputState.Actor; }
        public void SetInputStateOnDestination() { InputState = inputState.Destination; }
        public void SetInputStateOnNone() { InputState = inputState.none; }
        public void SetInputStatOnBuilding() { InputState = inputState.building; }

        public void SetInputStatOnActivatePower()
        {
            InputState = inputState.UsPower;
            _powerCursor =Instantiate(_selectedTourelle.ZoneEffect, Vector3.zero, quaternion.identity);
            _powerCursor.transform.localScale = Vector3.one*_selectedTourelle.ZoneSize;
        }
        public void OnUIEnter() { IsOnUI = true; }
        public void OnUIExit() { IsOnUI = false; }

        private List<Vector2Int> GetBuildingVec2by2(Vector2Int origin)
        {
            List<Vector2Int> cells = new List<Vector2Int>();
            Vector2Int cell = origin;
            if (Grid.CheckIsInGrid(cell)) if(OperateBuildingCell(cell))cells.Add(cell);
            cell += Vector2Int.left;
            if (Grid.CheckIsInGrid(cell)) if(OperateBuildingCell(cell))cells.Add(cell);
            cell += Vector2Int.up;
            if (Grid.CheckIsInGrid(cell)) if(OperateBuildingCell(cell))cells.Add(cell);
            cell += Vector2Int.right;
            if (Grid.CheckIsInGrid(cell)) if(OperateBuildingCell(cell))cells.Add(cell);
            return cells;
        }

        private bool OperateBuildingCell(Vector2Int cell) {
            if (Grid.GetCell(cell).BuildingCell != null && Grid.GetCell(cell).IndividualMoveValue <= 20) {
                if (!_preselectedCell.Contains(cell)) {
                    _preselectedCell.Add(cell);
                    Grid.GetCell(cell).BuildingCell.SetActive(true);
                }
                return true;
            }
            return false;
        }

        private void buid(List<Vector2Int> buildingCells)
        {
            Vector3 pos =new Vector3();
            TourelleFSM tourelleFsm =Instantiate(PrefabTower,Vector3.zero, Quaternion.identity).GetComponent<TourelleFSM>();
            foreach (Vector2Int cell in buildingCells)
            {
                pos += Grid.GetCellCenterWorldPosByCell(cell);
                Grid.GetCell(cell).IndividualMoveValue = 100;
                Grid.GetCell(cell).tourelle = tourelleFsm;
            }
            pos =pos/4;
            tourelleFsm.transform.position = pos;
        }
    }
}
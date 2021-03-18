using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Scripts.Main
{
    public class Cell
    {
        public Vector2Int Position;
        public int MoveValue;
        public int IndividualMoveValue;
        public Vector3 FlowFieldOrientation;
        public Vector2 FlowField2DOrientation;
        public GameObject BuildingCell;
        public CellDebug CellDebug;
        public bool Iswall;
        public PlayGrid Grid;
        public PlayGridV2 GridV2;
        public float DradFactor = 1;

        public Cell(Vector2Int pos, int moveValue , PlayGrid grid)
        {
            Position = pos;
            MoveValue = moveValue;
            Grid = grid;
        }
        public Cell(Vector2Int pos, int moveValue , PlayGridV2 grid)
        {
            Position = pos;
            MoveValue = moveValue;
            GridV2 = grid;
        }

        public void AddMoveValue(int value)
        {
            IndividualMoveValue += value;
            if(CellDebug!=null)CellDebug.ChangeMoveIndidualValue(IndividualMoveValue);
            if (Iswall)CellDebug.ActivateCollider();
        }

        public void SetMoveValue(int value)
        {
            MoveValue = value;
            if(CellDebug!=null)CellDebug.ChangeMoveValue(MoveValue);
        }

        public void SetFlowFieldVector(Vector3 vec)
        {
            FlowFieldOrientation = vec;
            if(CellDebug!=null)CellDebug.ChangeVector(FlowFieldOrientation);
        }
        public void SetFlowField2DVector(Vector2 vec)
        {
            FlowField2DOrientation = vec;
            if(CellDebug!=null)CellDebug.ChangeVector(FlowField2DOrientation);
        }

        public Vector2 Get2DVector() {
            return FlowField2DOrientation;
        }
        
        

    }
}
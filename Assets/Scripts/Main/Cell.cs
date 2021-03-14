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
        public CellDebug CellDebug;
        public bool Iswall;

        public Cell(Vector2Int pos, int moveValue)
        {
            Position = pos;
            MoveValue = moveValue;
        }

        public void AddMoveValue(int value)
        {
            IndividualMoveValue += value;
            CellDebug.ChangeMoveIndidualValue(IndividualMoveValue);
            if (Iswall)CellDebug.ActivateCollider();
        }

        public void SetMoveValue(int value)
        {
            MoveValue = value;
            CellDebug.ChangeMoveValue(MoveValue);
        }

        public void SetFlowFieldVector(Vector3 vec)
        {
            FlowFieldOrientation = vec;
            CellDebug.ChangeVector(FlowFieldOrientation);
        }
        

    }
}
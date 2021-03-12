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

        public Cell(Vector2Int pos, int moveValue)
        {
            Position = pos;
            MoveValue = moveValue;
        }

        public void AddMoveValue()
        {
            IndividualMoveValue += 10;
            CellDebug.ChangeMoveIndidualValue(IndividualMoveValue);
        }

        public void calculatMoveValue(int addvalue)
        {
            MoveValue = addvalue + IndividualMoveValue;
            CellDebug.ChangeMoveValue(MoveValue);
        }

    }
}
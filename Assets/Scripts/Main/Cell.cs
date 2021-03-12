using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public PlayGrid Grid;

        public Cell(Vector2Int pos, int moveValue , PlayGrid gird)
        {
            Position = pos;
            MoveValue = moveValue;
            Grid = gird;
        }

        public void AddMoveValue()
        {
            IndividualMoveValue += 10;
            CellDebug.ChangeMoveIndidualValue(IndividualMoveValue);
        }

        public void calculatMoveValue(int addvalue)
        {
            MoveValue = addvalue;
            CellDebug.ChangeMoveValue(MoveValue);
        }

        public void SetFolwFild(Vector3 value)
        {
            FlowFieldOrientation = value;
            CellDebug.ChangeVectorOriantation(value);
        }

        public void CalculateFlowFieldValue()
        {
            Debug.Log("Il y a " + Grid.GetNeigbor(Position).Count + " de case voisine");
            foreach (Vector2Int cell in Grid.GetNeigbor(Position) )
            {
                if ((Grid.GetCell(cell).Position-Position).magnitude > 1) {
                    if (Grid.GetCell(cell).MoveValue > MoveValue + 14+ Grid.GetCell(cell).IndividualMoveValue) {
                        Grid.GetCell(cell).calculatMoveValue( MoveValue + 14 + Grid.GetCell(cell).IndividualMoveValue);
                        Grid.GetCell(cell).CalculateFlowFieldValue();
                    }
                }
                else {
                    if (Grid.GetCell(cell).MoveValue > MoveValue + 10+ Grid.GetCell(cell).IndividualMoveValue) {
                        Grid.GetCell(cell).calculatMoveValue( MoveValue + 10 + Grid.GetCell(cell).IndividualMoveValue); 
                        Grid.GetCell(cell).CalculateFlowFieldValue();
                    } 
                }
            }
        }

        public void CalculateFlowFieldVector()
        {
            if (MoveValue != 0)
            {
                List<Cell> neibors = new List<Cell>();
                foreach (Vector2Int cell in Grid.GetNeigbor(Position)) neibors.Add(Grid.GetCell(cell));

                Vector2Int pos = neibors.OrderBy(cell => cell.MoveValue).First().Position;
                Vector2 oriantation = pos - Position;
                SetFolwFild(new Vector3(oriantation.x, 0, oriantation.y));
            }
        }
    }
}
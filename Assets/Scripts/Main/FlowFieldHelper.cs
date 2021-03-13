using Scripts.Main;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Helper
{
    public class FlowFieldHelper
    {
       
        public static void CalculatFlowField(PlayGrid grid , Vector2Int origin)
        {
            
            

            List<Vector2Int> OpenList = new List<Vector2Int>();
            List<Vector2Int> temporalToAdd = new List<Vector2Int>();
            foreach (Cell cell in grid.Cells) {
                cell.SetMoveValue(int.MaxValue);
            }
            OpenList.Add(origin);
            grid.GetCell(origin).SetMoveValue(0);
            while (OpenList.Count > 0)
            {
                foreach (Vector2Int cell in OpenList) {
                    foreach (Vector2Int neibors in grid.GetNeibors(cell)) {
                        if ((neibors-cell).magnitude > 1) {
                            if (grid.GetCell(neibors).MoveValue > grid.GetCell(cell).MoveValue + 14 + grid.GetCell(neibors).IndividualMoveValue) {
                                grid.GetCell(neibors).SetMoveValue( grid.GetCell(cell).MoveValue + 14 + grid.GetCell(neibors).IndividualMoveValue);
                                temporalToAdd.Add(neibors);
                                Vector2Int oriantation = cell - neibors;
                                grid.GetCell(neibors).SetFlowFieldVector(new Vector3(oriantation.x,0,oriantation.y));
                            } 
                        }
                        else {
                            if (grid.GetCell(neibors).MoveValue > grid.GetCell(cell).MoveValue + 10 + grid.GetCell(neibors).IndividualMoveValue) {
                                grid.GetCell(neibors).SetMoveValue( grid.GetCell(cell).MoveValue + 10 + grid.GetCell(neibors).IndividualMoveValue);
                                temporalToAdd.Add(neibors);
                                Vector2Int oriantation = cell - neibors;
                                grid.GetCell(neibors).SetFlowFieldVector(new Vector3(oriantation.x,0,oriantation.y));
                            } 
                        }
                    }
                }
                OpenList.Clear();
                OpenList.AddRange(temporalToAdd);
                temporalToAdd.Clear();
            }
            
            
            
            
           /* OpenList.Add(origin);
            do
            {
                List<Vector2Int> temporalToRemouve = new List<Vector2Int>(); 
                List<Vector2Int> temporalTAdd = new List<Vector2Int>(); 
                foreach (Vector2Int pos in OpenList)
                {
                    Vector2Int testPos = pos + new Vector2Int(1, 0);
                    if (CheckCell(grid, testPos, closeList)) grid.GetCell(testPos).calculatMoveValue(10); temporalTAdd.Add(testPos);
                    testPos = pos + new Vector2Int(-1, 0);
                    if (CheckCell(grid, testPos, closeList)) grid.GetCell(testPos).calculatMoveValue(10); temporalTAdd.Add(testPos);
                    testPos = pos + new Vector2Int(0, 1);
                    if (CheckCell(grid, testPos, closeList)) grid.GetCell(testPos).calculatMoveValue(10); temporalTAdd.Add(testPos);
                    testPos = pos + new Vector2Int(0, -1);
                    if (CheckCell(grid, testPos, closeList)) grid.GetCell(testPos).calculatMoveValue(10); temporalTAdd.Add(testPos);
                    testPos = pos + new Vector2Int(1, 1);
                    if (CheckCell(grid, testPos, closeList)) grid.GetCell(testPos).calculatMoveValue(14); temporalTAdd.Add(testPos);
                    testPos = pos + new Vector2Int(-1, 1);
                    if (CheckCell(grid, testPos, closeList)) grid.GetCell(testPos).calculatMoveValue(14); temporalTAdd.Add(testPos);
                    testPos = pos + new Vector2Int(1, -1);
                    if (CheckCell(grid, testPos, closeList)) grid.GetCell(testPos).calculatMoveValue(14); temporalTAdd.Add(testPos);
                    testPos = pos + new Vector2Int(-1, -1);
                    if (CheckCell(grid, testPos, closeList)) grid.GetCell(testPos).calculatMoveValue(14); temporalTAdd.Add(testPos);
                    temporalToRemouve.Add(pos);
                }

                foreach (Vector2Int pos in temporalToRemouve) {
                    closeList.Add(pos);
                    OpenList.Remove(pos);
                }
                OpenList.AddRange(temporalTAdd);
                

            } while (OpenList.Count>0);*/
        }

        public static bool  CheckCell(PlayGrid grid,Vector2Int pos ,List<Vector2Int> closeList)
        {
            if (grid.CheckIsInGrid(pos) && !closeList.Contains(pos)) return true;
            return false;
        }
        
        //Code volé
        
       /* void IntegrationField::calculateIntegrationField(unsigned targetX, unsigned targetY)
        {
            unsigned int targetID = targetY * mArrayWidth + targetX;
 
            resetField();//Set total cost in all cells to 65535
            list openList;
 
            //Set goal node cost to 0 and add it to the open list
            setValueAt(targetID, 0);
            openList.push_back(targetID);
 
            while (openList.size() &gt; 0)
            {
                //Get the next node in the open list
                unsigned currentID = openList.front();
                openList.pop_front();
 
                unsigned short currentX = currentID % mArrayWidth;
                unsigned short currentY = currentID / mArrayWidth;
 
                //Get the N, E, S, and W neighbors of the current node
                std::vector neighbors = getNeighbors(currentX, currentY);
                int neighborCount = neighbors.size();
 
                //Iterate through the neighbors of the current node
                for (int i = 0; i &lt; neighborCount; i++)         {             //Calculate the new cost of the neighbor node             // based on the cost of the current node and the weight of the next node             unsigned int endNodeCost = getValueByIndex(currentID)                          + getCostField()-&gt;getCostByIndex(neighbors[i]);
 
                    //If a shorter path has been found, add the node into the open list
                    if (endNodeCost &lt; getValueByIndex(neighbors[i]))
                    {
                        //Check if the neighbor cell is already in the list.
                        //If it is not then add it to the end of the list.
                        if (!checkIfContains(neighbors[i], openList))
                        {
                            openList.push_back(neighbors[i]);
                        }
 
                        //Set the new cost of the neighbor node.
                        setValueAt(neighbors[i], endNodeCost);
                    }
                }
            }
        }*/
    }
}
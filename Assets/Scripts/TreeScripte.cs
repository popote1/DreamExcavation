using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Main;
using UnityEngine;

public class TreeScripte : MonoBehaviour
{
    public PlayGridV2 grid;
    public Vector2Int pos;

    private void OnDestroy()
    {
        grid.GetCell(pos).IndividualMoveValue = 0;
        grid.GetCell(pos).DradFactor =1;
        grid.RefresfFlowField = true;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering.VirtualTexturing;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{
   private PathCreator creator;
   private CurvePath _curvePath;

   private void OnSceneGUI()
   {
       
       Draw();
       Input();
   }

   private void Input()
   {
       Event guiEvent = Event.current;
       Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

       if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
       {
           Undo.RecordObject(creator , "Add segment");
           _curvePath.AddSegment(mousePos);
       }
   }
   

   private void Draw()
   {
       for (int i = 0; i < _curvePath.NumSegments; i++)
       {
           Vector2[] points = _curvePath.GetPointsInSegment(i);
           Handles.color = Color.black;
           Handles.DrawLine(points[1],points[0]);
           Handles.DrawLine(points[2],points[3]);
           Handles.DrawBezier(points[0],points[3],points[1],points[2],Color.green, null, 2);
       }
       Handles.color = Color.red;
       for (int i = 0; i < _curvePath.NumPoints; i++)
       {
           Vector2 newPos = Handles.FreeMoveHandle(_curvePath[i], Quaternion.identity, 0.1f, Vector2.zero, Handles.CylinderHandleCap);
           if (_curvePath[i] != newPos)
           {
               Undo.RecordObject(creator , "Move point");
               _curvePath.MovePoint(i,newPos);
           }
       }
   }

   private void OnEnable()
   {
       creator = (PathCreator) target;
       if (creator.curvePath == null)
       {
           creator.CreatePath();
       }

       _curvePath = creator.curvePath;
   }
   
}

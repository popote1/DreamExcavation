using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PathCreator : MonoBehaviour
{
    [FormerlySerializedAs("Path")] [HideInInspector] public CurvePath curvePath;

    public void CreatePath() {
        curvePath = new CurvePath(transform.position);
    }
}

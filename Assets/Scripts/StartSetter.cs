using System.Collections;
using System.Collections.Generic;
using Scripts.Interaction;
using Scripts.Main;

using UnityEngine;

public class StartSetter : MonoBehaviour
{
    public TerrainGeneratorV2 TerrainGeneratorV2;
    public PlayGridV2 PlayGridV2;
    public GridControllerV2 GridControllerV2; 
    void Start()
    {
        TerrainGeneratorV2.MakeNewMesh();
        TerrainGeneratorV2.GeneratePlaybleMap();
        TerrainGeneratorV2.GeneratRouds();
        TerrainGeneratorV2.gameObject.isStatic = true;
        TerrainGeneratorV2.IsUsingTreeMidifier = true;
        TerrainGeneratorV2.IsUsingTreeMidifier2 = true;
        TerrainGeneratorV2.IsUsingTreeModifier3 = true;
        TerrainGeneratorV2.IsUsTreeTreashHosld = true;
        TerrainGeneratorV2.GeneratTreeMap();
        TerrainGeneratorV2.SpawnTree();
        PlayGridV2.GeneratWarterColiders();
        PlayGridV2.enabled = true;
        GridControllerV2.enabled = true;
        PlayGridV2.GeneratTerrainBorders();
        
        //StaticOcclusionCulling.Compute();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

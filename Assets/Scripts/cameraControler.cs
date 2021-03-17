using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class cameraControler : MonoBehaviour
{
    public Scrollbar Scrollbar;
    public TerrainGeneratorV2 TerrainGeneratorV2;
    public float YOffsettLimite;

    private float _yMax;
    private float _yMin;
    private float _yPosNormaliz;
    void Start()
    {
        _yMin = YOffsettLimite;
        _yMax = TerrainGeneratorV2.height - YOffsettLimite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CameraMove()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(_yMin, _yMax, Scrollbar.value),
            transform.position.z);
    }
}

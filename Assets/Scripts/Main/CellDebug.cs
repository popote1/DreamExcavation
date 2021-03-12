﻿using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Main
{
    public class CellDebug:MonoBehaviour
    {
        public SpriteRenderer Cellscare;
        public Text value;
        public Text IndividualValue;
        public GameObject Arrow;

        public void ChangeMoveIndidualValue(int newValue)
        {
            IndividualValue.text = newValue.ToString();
            Cellscare.color = new Color(1-newValue/100f,1-newValue/100f,1-newValue/100f,1);
                              //new Color(255-newValue , 255-newValue , 255-newValue, 255);
        }
        public void ChangeMoveValue(int newValue)
        {
           value.text = newValue.ToString();
        }
    }
}
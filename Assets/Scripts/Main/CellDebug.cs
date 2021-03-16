using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Main
{
    public class CellDebug:MonoBehaviour
    {
        public SpriteRenderer Cellscare;
        //public Text value;
        //public Text IndividualValue;
        public Vector3 flowFieldVecotr;

        private Collider _collider;

        private void Start()
        {
            _collider = GetComponent<Collider>();
            _collider.enabled = false;
        }

        private void Update()
        {
            Debug.DrawLine(transform.position , transform.position+flowFieldVecotr/2, Color.red);
        }

        public void ChangeMoveIndidualValue(int newValue)
        {
           // IndividualValue.text = newValue.ToString();
            Cellscare.color = new Color(1-newValue/100f,1-newValue/100f,1-newValue/100f,1);
                              //new Color(255-newValue , 255-newValue , 255-newValue, 255);
        }
        public void ChangeMoveValue(int newValue)
        {
          // value.text = newValue.ToString();
        }

        public void ChangeVector(Vector3 vec)
        {
            flowFieldVecotr = vec;
        }

        public void ActivateCollider()
        {
            _collider.enabled = true;
        }
    }
}
using System;
using Scripts.Main;
using UnityEngine;

namespace Scripts.GridActor
{
    [RequireComponent(typeof(Rigidbody))]
    public class GridMoverBase :MonoBehaviour
    {
        public bool Move;
        public float MaxSpeed = 5;
        public PlayGrid Grid;
        public Rigidbody Rigidbody;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (Move)
            {
                Vector2Int posOnGrid = new Vector2Int(Mathf.FloorToInt(transform.position.x),
                    Mathf.FloorToInt(transform.position.z));
                if (Grid.CheckIsInGrid(posOnGrid))
                {
                    Rigidbody.AddForce(Grid.GetCell(posOnGrid).FlowFieldOrientation);
                }
            }
            Rigidbody.velocity= Vector3.ClampMagnitude(Rigidbody.velocity , MaxSpeed);
        }
    }
}
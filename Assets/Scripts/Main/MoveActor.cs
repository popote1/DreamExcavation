using System;
using Scripts.Main;
using UnityEngine;

namespace Scripts.Actors
{
    [RequireComponent(typeof(Rigidbody))]
    public class MoveActor:MonoBehaviour
    {
        public float MaxSpeed=10;
        public float Acceleration=1;
        [HideInInspector] public PlayGrid grid;
        private Rigidbody _rigidbody;
        
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            Vector2Int pos = new Vector2Int(Mathf.FloorToInt(transform.position.x),Mathf.FloorToInt(transform.position.z));
            if (grid.CheckIsInGrid(pos))
            {
                _rigidbody.AddForce(grid.GetCell(pos).FlowFieldOrientation*Acceleration);
            }
            _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, MaxSpeed);
        }
    }
}
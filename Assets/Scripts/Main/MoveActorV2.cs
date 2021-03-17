using System;
using Scripts.Main;
using UnityEngine;

namespace Scripts.Actors
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MoveActorV2:MonoBehaviour
    {
        public float MaxSpeed=10;
        public float Acceleration=1;
        [HideInInspector] public PlayGridV2 grid;
        private Rigidbody2D _rigidbody;
        
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            Vector2Int pos = new Vector2Int(Mathf.FloorToInt(transform.position.x),Mathf.FloorToInt(transform.position.y));
            if (grid.CheckIsInGrid(pos))
            {
                _rigidbody.AddForce(grid.GetCell(pos).Get2DVector()*Acceleration);
            }
            _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, MaxSpeed);
            _rigidbody.drag = grid.GetCell(pos).DradFactor;
        }
    }
}
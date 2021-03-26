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
        public GameObject AnimationSprite;
        public GameObject DeadBody;
        public float AttackRate = 2;
        [HideInInspector] public PlayGridV2 grid;
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private float _timer;
        private bool _isAttaking;
        private Vector2 target;


        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = AnimationSprite.GetComponent<Animator>();
        }

        private void Update()
        {
            if (AnimationSprite != null&&!_isAttaking)
            {
                AnimationSprite.transform.up = _rigidbody.velocity.normalized;
            }

            if (_isAttaking)
            {
                AnimationSprite.transform.up = target - (Vector2)transform.position;
            }

            if (_timer > 0)
            {
                if (_timer < AttackRate - 1f)
                {
                    _animator.SetBool("IsAttaking", false);
                    _timer -= Time.deltaTime;
                    _rigidbody.drag = 1;
                    
                }

                
            }
            else
            {
                _isAttaking = false;
            }
        }

        private void FixedUpdate()
        {
            Vector2Int pos = new Vector2Int(Mathf.FloorToInt(transform.position.x),Mathf.FloorToInt(transform.position.y));
            if (grid.CheckIsInGrid(pos))
            {
                _rigidbody.AddForce(grid.GetCell(pos).Get2DVector()*Acceleration);
            }
            _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, MaxSpeed);
            if(!_isAttaking)_rigidbody.drag = grid.GetCell(pos).DradFactor;
        }

        private void OnDestroy()
        {
            Instantiate(DeadBody,transform.position , AnimationSprite.transform.rotation);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            
            if (_timer <= 0&&other.transform.CompareTag("Tower"))
            {
                Debug.Log(" is attaking");
                _animator.SetBool("IsAttaking", true);
                _timer = AttackRate;
                _isAttaking = true;
                target = other.transform.position;
                _rigidbody.drag = 10;
            }
        }


    }
}
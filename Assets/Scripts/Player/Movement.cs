using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class Movement : MonoBehaviour
    {
        [SerializeField] private float speed = 1;
        
        private Rigidbody2D _rigidbody2D;
        private Vector2 _moveDirection;

        private void Awake() => _rigidbody2D = GetComponent<Rigidbody2D>();

        private void FixedUpdate() => _rigidbody2D.linearVelocity = _moveDirection * speed;

        public void SetMoveDirection(Vector2 direction) => _moveDirection = direction;
    }
}
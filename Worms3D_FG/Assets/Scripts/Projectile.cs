using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WormsGame.Combat
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float _speed;
        Rigidbody _rigidbody;
        Vector3 _direction = Vector3.zero;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }


        void FixedUpdate()
        {
            _rigidbody.velocity = _direction * (_speed * Time.deltaTime);
        }

        public void SetDirection(Vector3 newDirection)
        {
            _direction = newDirection;
        }
    }
}
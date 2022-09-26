using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WormsGame.Combat
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeField] protected LayerMask _collisionMask;
        [SerializeField] protected float _gravity = -10;
        [SerializeField] protected ParticleSystem _explosionParticle;

        protected Rigidbody _rigidbody;
        
        float _launchForce;
        Vector3 _direction = Vector3.zero;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        public virtual void SetupProjectile(Vector3 newDirection, Weapon weapon, float launchForce)
        {
            _direction = newDirection;
            _launchForce = launchForce;
            _rigidbody.velocity = _direction * _launchForce;

        }
        
    }
}
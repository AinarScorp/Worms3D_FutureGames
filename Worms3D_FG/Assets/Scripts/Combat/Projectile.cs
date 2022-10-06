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

        protected WeaponInfo _weaponInfo;
        float _launchForce;
        Vector3 _direction = Vector3.zero;
        
        protected GameObject _thisUnitHasShot;
        //cached
        protected Rigidbody _rigidbody;
        

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        public virtual void SetupProjectile(GameObject thisUnit,Vector3 newDirection, WeaponInfo weaponInfo, float launchForce)
        {
            _thisUnitHasShot = thisUnit;
            _direction = newDirection;
            _launchForce = launchForce;
            _rigidbody.velocity = _direction * _launchForce;

        }
        
    }


}
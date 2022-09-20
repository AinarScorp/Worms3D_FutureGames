using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.Inputs;

namespace WormsGame.Combat
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] protected LayerMask _collisionMask;
        [SerializeField] protected float _gravity = -10;
        protected Rigidbody _rigidbody;
        Vector3 _direction = Vector3.zero;
        float _launchForce;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }


        // void FixedUpdate()
        // {
        //     Vector3 vericalVelocity = Vector3.down * _gravity * Time.deltaTime;
        //     _rigidbody.AddForce(vericalVelocity, ForceMode.VelocityChange);
        // }

        public virtual void SetupProjectile(Vector3 newDirection, Weapon weapon, float launchForce)
        {
            _direction = newDirection;
            _launchForce = launchForce;
            _rigidbody.velocity = _direction * _launchForce;

        }

        // void OnTriggerEnter(Collider other)
        // {
        //     if ((_layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        //     {
        //         if (other.GetComponent<InputHandler>().enabled) return;
        //         Unit hitUnit = other.GetComponent<Unit>();
        //         hitUnit?.ModifyHealth(-_weapon.WeaponDamage);
        //         
        //         Debug.Log("Hit with Layermask");
        //         Destroy(this.gameObject);
        //         
        //     }
        // }
    }
}
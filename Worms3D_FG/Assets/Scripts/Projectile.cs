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
        [SerializeField] float _speed;
        [SerializeField]LayerMask _layerMask;
        Weapon _weapon;
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

        public void SetupProjectile(Vector3 newDirection, Weapon weapon)
        {
            _weapon = weapon;
            _direction = newDirection;
            Destroy(this.gameObject,20);
        }

        void OnTriggerEnter(Collider other)
        {
            if ((_layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                if (other.GetComponent<InputHandler>().enabled) return;
                Unit hitUnit = other.GetComponent<Unit>();
                hitUnit?.ModifyHealth(-_weapon.WeaponDamage);
                
                Debug.Log("Hit with Layermask");
                Destroy(this.gameObject);
                
            }
        }
    }
}
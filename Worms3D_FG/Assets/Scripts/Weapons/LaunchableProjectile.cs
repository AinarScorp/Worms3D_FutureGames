using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.Movement;

namespace WormsGame.Combat
{
    public class LaunchableProjectile : Projectile
    {
        [SerializeField] float _explosionRadius = 4.0f;
        LaunchableWeapon _weapon;
        bool hasCollided = false;
        [SerializeField] LayerMask _obstuctionMask;

        Vector3 _exlosionPoint;
        
        void FixedUpdate()
        {
            if (hasCollided) return;

            Vector3 vericalVelocity = Vector3.down * (_gravity * Time.deltaTime);
            if (hasCollided) return;

            _rigidbody.AddForce(vericalVelocity, ForceMode.VelocityChange);
        }

        public override void SetupProjectile(Vector3 newDirection, Weapon weapon, float launchForce)
        {
            base.SetupProjectile(newDirection, weapon, launchForce);
            _weapon = (LaunchableWeapon)weapon;
        }



        void OnCollisionEnter(Collision collision)
        {
            if ((_collisionMask.value & (1 << collision.collider.transform.gameObject.layer)) > 0)
            {
                PlayerController currentPlayer = collision.collider.GetComponent<PlayerController>();
                if (currentPlayer !=null && currentPlayer.enabled == true) return;
                _exlosionPoint = transform.position;
                Explode();
                //Debug.Log("Hit with Layermask");
            }
        }

        void Explode()
        {
            hasCollided = true;
            _rigidbody.velocity *= 0;
            Collider[] allColliders = Physics.OverlapSphere(transform.position, _explosionRadius,_collisionMask);
            foreach (var collider in allColliders)
            {
                Unit unit = collider.GetComponent<Unit>();
                if (unit !=null)
                {
                    CalculateDamage(unit);
                    //unit.ModifyHealth(-_weapon.MaxDamage);
                }
            }
        }

        void CalculateDamage(Unit targetUnit)
        {
            float distanceToTarget = Vector3.Distance(_exlosionPoint, targetUnit.transform.position);
            Vector3 directionToTarget = (targetUnit.transform.position - _exlosionPoint).normalized;
            RaycastHit hit;
            if (Physics.Raycast(_exlosionPoint, directionToTarget,out hit,distanceToTarget,_obstuctionMask.value))
            {
                
                Debug.DrawLine(_exlosionPoint,  _exlosionPoint+ directionToTarget * distanceToTarget,Color.blue, 50f);
                print($"{transform.position} + {targetUnit.name} + collider: {hit.collider.name}");
            }
        }

        void OnDrawGizmosSelected()
        {
            if (hasCollided)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_exlosionPoint,_explosionRadius);
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(transform.position,_explosionRadius);
            }
            
        }
    }
    
}

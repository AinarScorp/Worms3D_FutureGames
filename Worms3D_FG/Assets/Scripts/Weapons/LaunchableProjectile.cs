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
        [SerializeField] LayerMask _targetLayerMask;
        [SerializeField] LayerMask _obstuctionLayerMask;

        bool hasCollided;
        Vector3 _exlosionPoint;
        
        LaunchableWeapon _weapon;
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
                this.gameObject.SetActive(false);
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
                    unit.ModifyHealth(-DamageFromExplosion(unit));
                }
            }
        }
        //here I should have a way to calculate the obstacles and distance
        int DamageFromExplosion(Unit targetUnit)
        {
            bool wasObstucted = false;
            CharacterController characterController = targetUnit.GetComponent<CharacterController>();
            Vector3 targetCenter = targetUnit.transform.position + new Vector3(0.0f, characterController.height * 0.5f, 0.0f);
            RaycastHit hit;
            RaycastHit testHit;
            
            Vector3 directionToTarget = (targetCenter - _exlosionPoint).normalized;
            //this checks if there's a wall in between and if yes it will reduce radius check
            if (Physics.Raycast(_exlosionPoint, directionToTarget,out testHit,_explosionRadius,_obstuctionLayerMask.value))
            {
                print($"{transform.position} + {targetUnit.name} + collider: {testHit.collider.name}");
                wasObstucted = true;
            }

            float maxCheckDistance = wasObstucted ? _explosionRadius * 0.5f : _explosionRadius;
            if (Physics.Raycast(_exlosionPoint, directionToTarget,out hit,maxCheckDistance,_targetLayerMask.value))
            {
                float distanceToTarget = Vector3.Distance(_exlosionPoint, hit.point);
                
                int receivedDamage = Mathf.FloorToInt(Mathf.Lerp(_weapon.MaxDamage, _weapon.MinDamage, distanceToTarget / _explosionRadius));
                print($"would hit {targetUnit.name} for {receivedDamage}");
                //Debug.DrawLine(_exlosionPoint,  _exlosionPoint+ directionToTarget * distanceToTarget,Color.blue, 50f);
                //print($"{transform.position} + {targetUnit.name} + collider: {hit.collider.name}");
                return receivedDamage;
            }

            return 0;
        }

        void OnDrawGizmosSelected()
        {
            if (hasCollided)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_exlosionPoint,_explosionRadius);
            }
            
        }
    }
    
}

using System;
using UnityEngine;
using WormsGame.Units;


namespace WormsGame.Combat
{
    public class LaunchableProjectile : Projectile
    {
        [SerializeField] float _explosionRadius = 4.0f;
        [SerializeField] LayerMask _targetLayerMask;
        [SerializeField] LayerMask _obstuctionLayerMask;

        bool hasCollided;
        Vector3 _exlosionPoint;
        
        //LaunchableWeapon _weapon;
        WeaponInfo _weaponInfo;
        void Start()
        {
            SetParticleRadius();
        }

        void SetParticleRadius()
        {
            if (_explosionParticle != null)
            {
                ParticleSystem.ShapeModule shapeModule = _explosionParticle.shape;
                shapeModule.radius = _explosionRadius;
            }
        }

        void FixedUpdate()
        {
            if (hasCollided) return;

            Vector3 vericalVelocity = Vector3.down * (_gravity * Time.deltaTime);
            if (hasCollided) return;

            _rigidbody.AddForce(vericalVelocity, ForceMode.VelocityChange);
        }

        public override void SetupProjectile(GameObject thisUnit,Vector3 newDirection, WeaponInfo weaponInfo, float launchForce)
        {
            base.SetupProjectile(thisUnit,newDirection, weaponInfo, launchForce);
            _weaponInfo = weaponInfo;
            //_weapon = (LaunchableWeapon)weapon;
        }

        
        void OnCollisionEnter(Collision collision)
        {
            if ((_collisionMask.value & (1 << collision.collider.transform.gameObject.layer)) > 0)
            {
                //I do this check to make sure it doesn't collide with the one that has shot the projectile
                
                Unit hitUnit = collision.collider.GetComponent<Unit>();
                if (hitUnit!=null && _thisUnitHasShot == hitUnit.gameObject) return;
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

            if (_explosionParticle != null)
                Instantiate(_explosionParticle, _exlosionPoint, Quaternion.identity);
            

            Collider[] allColliders = Physics.OverlapSphere(transform.position, _explosionRadius,_collisionMask);
            foreach (var collider in allColliders)
            {
                Unit unit = collider.GetComponent<Unit>();
                if (unit !=null)
                {
                    Vector3 direction = unit.transform.position - _exlosionPoint;
                    int damage = DamageFromExplosion(unit);
                    unit.ModifyHealth(-damage);
                    unit.Push(direction, _weaponInfo.PushForce * damage);

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
                wasObstucted = true;
            }

            float maxCheckDistance = wasObstucted ? _explosionRadius * 0.5f : _explosionRadius;
            if (Physics.Raycast(_exlosionPoint, directionToTarget,out hit,maxCheckDistance,_targetLayerMask.value))
            {
                float distanceToTarget = Vector3.Distance(_exlosionPoint, hit.point);
                
                int receivedDamage = Mathf.FloorToInt(Mathf.Lerp(_weaponInfo.MaxDamage, _weaponInfo.MinDamage, distanceToTarget / _explosionRadius));
                //Debug.DrawLine(_exlosionPoint,  _exlosionPoint+ directionToTarget * distanceToTarget,Color.blue, 50f);
                //print($"{transform.position} + {targetUnit.name} + collider: {hit.collider.name}");
                return receivedDamage;
            }
            return 0;
        }

        void OnDrawGizmosSelected()
        {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_exlosionPoint,_explosionRadius);
        }
    }
    
}

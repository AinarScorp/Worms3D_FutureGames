using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace WormsGame.Combat
{    
    public abstract class  Weapon : ScriptableObject
    {
        [SerializeField] GameObject _weaponPrefab;
        [SerializeField] protected float pushForce = 0.1f;

        protected CombatController _thisUnit;
        Transform _handTransform;
        const string WEAPON_NAME = "Weapon";
        //public static event Action HasFired;

        public float PushForce => pushForce;

        public void SpawnWeapon(Transform handTransform)
        {
            _handTransform = handTransform;
            _thisUnit = handTransform.GetComponentInParent<CombatController>();
            DestroyOldWeapon();
            if (_weaponPrefab !=null)
            {
                GameObject weapon = Instantiate(_weaponPrefab, handTransform);
                weapon.name = WEAPON_NAME;
            }
        }

        public void DestroyOldWeapon()
        {
            Transform oldWeapon = _handTransform.Find(WEAPON_NAME);
            if (oldWeapon == null) return;

            oldWeapon.name = "BEING DESTROYED";
            Destroy(oldWeapon.gameObject);
        }

        public virtual void Fire(Vector3 spawnPos, float launchForce, Vector3 direction)
        {
            //HasFired?.Invoke();
        }

        public virtual void Fire(Vector3 shootFromPos,Vector3 direction)
        {
            Fire(shootFromPos, 0,direction);
        }
    }
}
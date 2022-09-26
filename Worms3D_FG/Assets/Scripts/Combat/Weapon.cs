using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace WormsGame.Combat
{    
    public abstract class  Weapon : ScriptableObject
    {
        [SerializeField] GameObject _weaponPrefab;
        Transform _handTransform;
        const string WEAPON_NAME = "Weapon";
        public static event Action HasFired;
        
        public void SpawnWeapon(Transform handTransform)
        {
            _handTransform = handTransform;

            DestroyOldWeapon();
            if (_weaponPrefab !=null)
            {
                GameObject weapon = Instantiate(_weaponPrefab, handTransform);
                weapon.name = WEAPON_NAME;
            }
        }

        void DestroyOldWeapon()
        {
            Transform oldWeapon = _handTransform.Find(WEAPON_NAME);
            if (oldWeapon == null)
            {
                return;
                
            }

            oldWeapon.name = "BEING DESTROYED";
            Destroy(oldWeapon.gameObject);
        }

        public virtual void Fire(Vector3 spawnPos, float launchForce, Vector3 direction)
        {
            DestroyOldWeapon();
            HasFired?.Invoke();
        }

        public virtual void Fire(Vector3 shootFromPos,Vector3 direction)
        {
            DestroyOldWeapon();
            HasFired?.Invoke();
        }
    }
}
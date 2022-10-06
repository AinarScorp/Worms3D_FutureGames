using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


namespace WormsGame.Combat
{        
    public struct WeaponInfo
    {
        float _minDamage, _maxDamage, _pushForce;
        
        public float MinDamage => _minDamage;
        public float MaxDamage => _maxDamage;
        public float PushForce => _pushForce;
        
        public WeaponInfo(float minDamage, float maxDamage, float pushForce)
        {
            _minDamage = minDamage;
            _maxDamage = maxDamage;
            _pushForce = pushForce;
        }
    }
    
    public abstract class  Weapon : ScriptableObject
    {
        [SerializeField] GameObject _weaponPrefab;
        [SerializeField] protected float pushForce = 0.1f;

        protected GameObject _gameObjectToIgnore;
        Transform _handTransform;
        const string WEAPON_NAME = "Weapon";


        public void SpawnWeapon(Transform handTransform, GameObject gameObjectToIgonre)
        {
            _handTransform = handTransform;
            _gameObjectToIgnore = gameObjectToIgonre;
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
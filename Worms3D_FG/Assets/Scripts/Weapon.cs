using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WormsGame.Combat
{    
    public abstract class  Weapon : ScriptableObject
    {
        [SerializeField] GameObject _weaponPrefab;
        
        const string WEAPON_NAME = "Weapon";

        
        public void SpawnWeapon(Transform handTransform)
        {
            DestroyOldWeapon(handTransform);
            if (_weaponPrefab !=null)
            {
                GameObject weapon = Instantiate(_weaponPrefab, handTransform);
                weapon.name = WEAPON_NAME;
            }
        }

        void DestroyOldWeapon(Transform handTransform)
        {
            Transform oldWeapon = handTransform.Find(WEAPON_NAME);
            if (oldWeapon == null)
            {
                return;
                
            }

            oldWeapon.name = "BEING DESTROYED";
            Destroy(oldWeapon.gameObject);
        }

        public virtual void Fire(Vector3 spawnPos, float launchForce, Vector3 direction)
        {
            Debug.Log(this.name + " with the name " + _weaponPrefab.name + " is Shooting");
        }
    }
}
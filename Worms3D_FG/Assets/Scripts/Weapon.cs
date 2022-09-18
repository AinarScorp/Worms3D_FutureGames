using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WormsGame.Combat
{    
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] float _weaponDamage = 10f;
        [SerializeField] Projectile projectilePrefab;
        [SerializeField] GameObject _weaponPrefab;
        [SerializeField] Transform _gunPoint;
        
        const string WEAPON_NAME = "Weapon";

        public float WeaponDamage => _weaponDamage;

        public void Fire(Vector3 spawnPos,Vector3 direction)
        {
            Debug.Log(this.name + " with the name " + _weaponPrefab.name + " is Shooting");
            
            if (projectilePrefab == null) return;
            
            Projectile projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
            projectile.SetDirection(direction);
        }
        
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
    }
}